using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Roton;
using Roton.Composers.Audio;
using Roton.Emulation.Core;
using Roton.Infrastructure;

namespace Lyon.Common.Presenters.Impl;

/// <inheritdoc cref="IAudioPresenter"/>
/// <inheritdoc cref="IDisposable"/>
// ReSharper disable once UnusedMember.Global
[Context(Context.Original)]
[Context(Context.Super)]
public sealed unsafe class AudioPresenter(
    IConfig config,
    IAudioComposer composer,
    IScheduler scheduler)
    : IDisposable, IAudioPresenter
{
    /// <summary>
    /// Returns true if <see cref="Dispose"/> has been called.
    /// </summary>
    private bool _isDisposed;

    /// <summary>
    /// Returns true if the presenter is currently processing audio data.
    /// </summary>
    private bool _running;

    /// <summary>
    /// Audio data buffer.
    /// </summary>
    private readonly Queue<float> _buffer = [];

    /// <summary>
    /// Mutex for modifying the audio data buffer.
    /// </summary>
    private readonly Lock _bufferLock = new();

    /// <summary>
    /// Current SDL audio stream.
    /// </summary>
    private SDL_AudioStream* _stream;

    /// <summary>
    /// Used for reference counting SDL subsystems.
    /// </summary>
    private SdlContext? _sdlContext;

    /// <summary>
    /// Cache of all presenters, used by the static SDL callback handler.
    /// </summary>
    private static readonly Dictionary<nint, AudioPresenter> Presenters = [];

    /// <summary>
    /// Handler for SDL audio callbacks.
    /// </summary>
    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void OnCallback(nint userData, SDL_AudioStream* stream, int required, int total)
    {
        // If we aren't tracking the stream, don't do anything with it.
        if (!Presenters.TryGetValue((nint)stream, out var presenter))
            return;

        // We ask for 2x the buffer size so that there's a double
        // buffer of audio data.
        var want = required / sizeof(float) * 2;
        var floats = (stackalloc float[want]);
        int count;

        lock (presenter._bufferLock)
        {
            var have = presenter._buffer.Count;
            if (have < want)
            {
                Debug.WriteLine($"Audio buffer underflow: need {want}, got {have}");
                return;
            }

            // Fill the buffer.
            count = Math.Min(have, floats.Length);
            for (var i = 0; i < count; i++)
                floats[i] = presenter._buffer.Dequeue();
        }

        // Send the buffer to SDL.
        fixed (float* floatsPtr = floats)
            SDL_PutAudioStreamData(stream, (IntPtr)floatsPtr, count * sizeof(float));
    }

    /// <inheritdoc />
    public void Start()
    {
        // If already running, bail.
        if (_running)
            return;
        _running = true;

        // Configure audio settings.
        var sampleRate = config.Audio.SampleRate;
        var spec = new SDL_AudioSpec
        {
            channels = 1,
            format = SDL_AUDIO_F32,
            freq = config.Audio.SampleRate
        };

        // Start the SDL audio subsystem.
        _sdlContext = SdlContext.Create(SDL_InitFlags.SDL_INIT_AUDIO);
        composer.BufferReady += OnComposerBufferReady;

        // Create the audio stream.
        _stream = SDL_OpenAudioDeviceStream(SDL_AUDIO_DEVICE_DEFAULT_PLAYBACK, &spec, &OnCallback, 0);
        if (_stream == null)
            throw new SdlException("Failed to create audio stream");
        Presenters.Add((nint)_stream, this);

        // Set up event handlers.
        composer.SampleRate = sampleRate;

        // Connect the engine timer to the composer.
        scheduler.Tick += OnEngineTick;

        // Start playback.
        SDL_ResumeAudioStreamDevice(_stream);
    }

    /// <summary>
    /// Handles when the engine runs a tick.
    /// </summary>
    private void OnEngineTick(object? sender, EventArgs e) =>
        composer.Tick();

    /// <summary>
    /// Handles when the composer is ready to provide a buffer.
    /// </summary>
    private void OnComposerBufferReady(object? sender, AudioDataEventArgs e)
    {
        var data = e.Data;

        lock (_bufferLock)
        {
            _buffer.EnsureCapacity(_buffer.Count + data.Length);

            foreach (var sample in data)
                _buffer.Enqueue(sample);
        }

        e.Memory.Dispose();
    }

    /// <inheritdoc />
    public void Stop()
    {
        // If not running, bail.
        if (!_running)
            return;

        _running = false;
        composer.BufferReady -= OnComposerBufferReady;
        scheduler.Tick -= OnEngineTick;

        // If the last presenter is shut down, also shut down the SDL audio subsystem.
        _sdlContext!.Dispose();
        _sdlContext = null;
    }

    /// <inheritdoc />
    public float Gain
    {
        get => SDL_GetAudioStreamGain(_stream);
        set => SDL_SetAudioStreamGain(_stream, value);
    }

    public void Dispose()
    {
        if (_isDisposed)
            return;

        _isDisposed = true;

        // Clean up.
        Stop();
    }
}