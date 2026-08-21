using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using DotSDL.Audio;
using Roton;
using Roton.Composers.Audio;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Lyon.Presenters.Impl;

[Context(Context.Startup)]
// ReSharper disable once UnusedMember.Global
public sealed class AudioPresenter : IDisposable, IAudioPresenter
{
    private bool _isDisposed;
    private bool _running;
    private readonly Queue<double> _buffer;
    private readonly Lock _bufferLock = new();
    private readonly Playback _audio;

    public AudioPresenter(IConfig config, IEngine engine, IAudioComposer composer)
    {
        _buffer = [];
        _audio = new Playback(config.AudioSampleRate, AudioFormat.Integer16, ChannelCount.Mono,
            (ushort)config.AudioBufferSize);
        Volume = 0.1;

        composer.BufferReady += (_, a) => Update(a);
        composer.SampleRate = SampleRate;
        Start();

        engine.Tick += (_, _) => composer.Tick();

        _audio.BufferEmpty += BufferEmpty;
        _audio.Play();
    }

    private void BufferEmpty(object sender, AudioBuffer e)
    {
        lock (_bufferLock)
        {
            if (_buffer.Count < e.Length)
            {
                Debug.WriteLine($"Audio buffer underflow: need {e.Length}, got {_buffer.Count}");
                return;
            }

            var count = Math.Min(_buffer.Count, e.Length);

            for (var i = 0; i < count; i++)
                e.Samples[Channel.Mono][i] = _buffer.Dequeue();
        }
    }

    public void Start()
    {
        if (_running)
            return;

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

    public int SampleRate => _audio.Frequency;

    public void Stop()
    {
        if (!_running)
            return;

        _running = false;
    }

    public double Volume { get; set; }

    public void Dispose()
    {
        if (_isDisposed)
            return;

        _isDisposed = true;
        Stop();
        _audio.Close();
    }
}