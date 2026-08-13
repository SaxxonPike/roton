using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DotSDL.Audio;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Lyon.Presenters.Impl;

[Context(Context.Startup)]
// ReSharper disable once UnusedMember.Global
public sealed class AudioPresenter : IDisposable, IAudioPresenter
{
    private bool _isDisposed;
    private bool _running;
    private readonly List<double> _buffer;
    private readonly object _bufferLock = new();
    private readonly Playback _audio;

    public AudioPresenter(IConfig config)
    {
        _buffer = new List<double>();
        _audio = new Playback(config.AudioSampleRate, AudioFormat.Integer16, ChannelCount.Mono,
            (ushort) config.AudioBufferSize);
        Volume = 0.1;

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
            var samples = _buffer.Take(count).ToArray();
            Buffer.BlockCopy(samples, 0, e.Samples[Channel.Mono], 0, count * 8);
            _buffer.RemoveRange(0, count);
        }
    }

    public void Start()
    {
        if (_running)
            return;

        _running = true;
    }

    public void Update(IEnumerable<float> buffer)
    {
        // Buffer incoming data.
        var data = buffer.Select(i => i * Volume).ToArray();
        lock (_bufferLock)
            _buffer.AddRange(data);
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