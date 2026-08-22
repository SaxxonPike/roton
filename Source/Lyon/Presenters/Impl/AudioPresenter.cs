using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Roton;
using Roton.Composers.Audio;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Lyon.Presenters.Impl;

/// <inheritdoc cref="IAudioPresenter"/>
/// <inheritdoc cref="IDisposable"/>
// ReSharper disable once UnusedMember.Global
[Context(Context.Startup)]
public sealed unsafe class AudioPresenter(
    IConfig config, 
    IAudioComposer composer)
    : IDisposable, IAudioPresenter
{
    /// <summary>
    /// Current engine that the presenter is processing audio for.
    /// </summary>
    private IEngine? _engine;
    
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
                Console.WriteLine($"Audio buffer underflow: need {want}, got {have}");
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
    public void Start(IEngine engine)
    {
        // If already running, bail.
        if (_running)
            return;
        _running = true;
        _engine = engine;

        // Configure audio settings.
        SampleRate = config.AudioSampleRate;
        var spec = new SDL_AudioSpec
        {
            channels = 1,
            format = SDL_AUDIO_F32,
            freq = config.AudioSampleRate
        };

        // Start the SDL audio subsystem.
        if (Presenters.Count == 0)
        {
            if (!SDL_InitSubSystem(SDL_InitFlags.SDL_INIT_AUDIO))
                throw new SdlException("Failed to initialize SDL audio subsystem");
            composer.BufferReady += OnComposerBufferReady;
        }

        // Create the audio stream.
        _stream = SDL_OpenAudioDeviceStream(SDL_AUDIO_DEVICE_DEFAULT_PLAYBACK, &spec, &OnCallback, 0);
        if (_stream == null)
            throw new SdlException("Failed to create audio stream");
        SDL_SetAudioStreamGain(_stream, 0.07f);
        Presenters.Add((nint)_stream, this);

        // Set up event handlers.
        composer.SampleRate = SampleRate;

        // Connect the engine timer to the composer.
        engine.Tick += OnEngineTick;

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
    private void OnComposerBufferReady(object? sender, AudioComposerDataEventArgs e)
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

    /// <summary>
    /// Sampling rate of the audio stream.
    /// </summary>
    public int SampleRate { get; private set; }

    /// <inheritdoc />
    public void Stop()
    {
        // If not running, bail.
        if (!_running)
            return;
        _running = false;
        _engine?.Tick -= OnEngineTick;
        
        // If the last presenter is shut down, also shut down the SDL audio subsystem.
        if (Presenters.Remove((nint)_stream) && Presenters.Count == 0)
        {
            composer.BufferReady -= OnComposerBufferReady;
            SDL_QuitSubSystem(SDL_InitFlags.SDL_INIT_AUDIO);
        }
    }

    /// <summary>
    /// Output gain of the audio signal. Defaults to 0.07f. Due to the output
    /// signal being pure square waves, it is generally recommended to keep this
    /// value relatively low (it is very loud for its peak level.)
    /// </summary>
    public float Volume
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