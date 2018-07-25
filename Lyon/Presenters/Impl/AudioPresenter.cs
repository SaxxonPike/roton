using System;
using System.Collections.Generic;
using System.Linq;
using DotSDL.Audio;
using Roton.Emulation.Data;

namespace Lyon.Presenters.Impl
{
    public class AudioPresenter : IDisposable, IAudioPresenter
    {
        private bool _isDisposed;
        private bool _running;
        private readonly List<double> _buffer;
        private readonly Playback _audio;

        public AudioPresenter(IConfig config)
        {
            _buffer = new List<double>();
            _audio = new Playback(config.AudioSampleRate, AudioFormat.Integer16, 1);
            Volume = 0.2;

            _audio.BufferEmpty += BufferEmpty;
            _audio.Play();
        }

        private void BufferEmpty(object sender, AudioBuffer e)
        {
            if (_buffer.Count < _audio.BufferSizeSamples)
                return;

            var samples = _buffer.Take(_audio.BufferSizeSamples).ToArray();
            Array.Copy(samples, e.Samples, Math.Min(_audio.BufferSizeSamples, e.Samples.Length));
            _buffer.RemoveRange(0, _audio.BufferSizeSamples);
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
            _buffer.AddRange(data);
        }

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
}