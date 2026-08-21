using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Roton;
using Roton.Composers.Audio;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Lyon.Presenters.Impl;

[Context(Context.Startup)]
// ReSharper disable once UnusedMember.Global
public sealed unsafe class AudioPresenter : IDisposable, IAudioPresenter
{
    private bool _isDisposed;
    private bool _running;
    private readonly Queue<float> _buffer;
    private readonly Lock _bufferLock = new();
    private readonly SDL_AudioStream* _stream;
    private static readonly Dictionary<nint, AudioPresenter> Presenters = [];

    public AudioPresenter(IConfig config, IEngine engine, IAudioComposer composer)
    {
        _buffer = [];

        SampleRate = config.AudioSampleRate;
        var spec = new SDL_AudioSpec
        {
            channels = 1,
            format = SDL_AUDIO_F32,
            freq = config.AudioSampleRate
        };

        // Create the audio stream.
        if (!SDL_Init(SDL_InitFlags.SDL_INIT_AUDIO))
            throw new Exception($"Failed to initialize SDL audio subsystem: {SDL_GetError()}");

        _stream = SDL_OpenAudioDeviceStream(SDL_AUDIO_DEVICE_DEFAULT_PLAYBACK, &spec, &OnCallback, 0);
        if (_stream == null)
            throw new Exception($"Failed to create audio stream: {SDL_GetError()}");

        Presenters.Add((nint)_stream, this);
        Volume = 0.1f;

        composer.BufferReady += (_, a) => Update(a);
        composer.SampleRate = SampleRate;

        Start();

        engine.Tick += (_, _) => composer.Tick();
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void OnCallback(nint userData, SDL_AudioStream* stream, int required, int total)
    {
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

            count = Math.Min(have, floats.Length);
            for (var i = 0; i < count; i++)
                floats[i] = presenter._buffer.Dequeue();
        }

        fixed (float* floatsPtr = floats)
            SDL_PutAudioStreamData(stream, (IntPtr)floatsPtr, count * sizeof(float));
    }

    public void Start()
    {
        if (_running)
            return;

        SDL_ResumeAudioStreamDevice(_stream);
        _running = true;
    }

    public void Update(AudioComposerDataEventArgs e)
    {
        if (_buffer == null)
            return;

        var data = e.Data;

        lock (_bufferLock)
        {
            _buffer.EnsureCapacity(_buffer.Count + data.Length);

            foreach (var sample in data)
                _buffer.Enqueue(sample * Volume);
        }

        e.Memory.Dispose();
    }

    public int SampleRate { get; private set; }

    public void Stop()
    {
        if (!_running)
            return;

        _running = false;
    }

    public float Volume { get; set; }

    public void Dispose()
    {
        if (_isDisposed)
            return;

        _isDisposed = true;
        Stop();
        SDL_QuitSubSystem(SDL_InitFlags.SDL_INIT_AUDIO);
    }
}