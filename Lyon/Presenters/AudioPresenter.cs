using System;
using System.Collections.Generic;
using System.Linq;
using DotSDL.Audio;
using Roton.Composers.Audio;

namespace Lyon.Presenters
{
    public class AudioPresenter : IDisposable, IAudioPresenter
    {
        private readonly Func<IAudioComposer> _getAudioComposer;
        private bool _isDisposed;
        private bool _running;
        private readonly List<double> _buffer;
        private readonly Playback _audio;

        private IAudioComposer AudioComposer => _getAudioComposer();

        public AudioPresenter(Func<IAudioComposer> getAudioComposer)
        {
            _buffer = new List<double>();
            _getAudioComposer = getAudioComposer;
            _audio = new Playback(44100, AudioFormat.Integer16, 1);
            Volume = 0.2;
            
            _audio.BufferEmpty += BufferEmpty;
            _audio.Play();
        }

        private void BufferEmpty(object sender, AudioBuffer e) {
            if(_buffer.Count < _audio.BufferSizeSamples) return;
            
            var samples = _buffer.Take(_audio.BufferSizeSamples).ToArray();
            for(var i = 0; i < e.Samples.Length; i++)
                e.Samples[i] = samples[i];
            
            _buffer.RemoveRange(0, _audio.BufferSizeSamples);
        }

        public void Start()
        {
            if (_running)
                return;

            _running = true;
            AudioComposer.BufferReady += Update;
        }

        private void Update(object sender, AudioComposerDataEventArgs e)
        {
            // Buffer incoming data.
            var data = e.Data.Select(i => i * Volume).ToArray();
            _buffer.AddRange(data);
        }

        public void Stop()
        {
            if (!_running)
                return;

            AudioComposer.BufferReady -= Update;
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