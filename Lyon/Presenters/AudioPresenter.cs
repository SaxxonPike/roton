using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using Roton.Composers.Audio;

namespace Lyon.Presenters
{
    public class AudioPresenter : IDisposable, IAudioPresenter
    {
        private readonly Func<IAudioComposer> _getAudioComposer;
        private const int BufferSampleCount = 512;
        private bool _isDisposed;
        private readonly int _alSourceHandle;
        private short _highLevel;
        private short _lowLevel;
        private const short MidLevel = 0;
        private double _volume;
        private bool _running;
        private readonly List<byte> _buffer;
        private int _bufferCount;
        private readonly AudioContext _context;

        private IAudioComposer AudioComposer => _getAudioComposer();

        public AudioPresenter(Func<IAudioComposer> getAudioComposer)
        {
            _buffer = new List<byte>();
            _getAudioComposer = getAudioComposer;
            _context = new AudioContext();
            _context.MakeCurrent();
            Volume = 0.2;

            _alSourceHandle = AL.GenSource();
            AL.Source(_alSourceHandle, ALSourcef.Gain, 1.0f);
        }

        public void Start()
        {
            if (_running)
                return;

            _running = true;
            AudioComposer.BufferReady += Update;
        }

        private void AssertIfError()
        {
            var error = AL.GetError();
            if (error != ALError.NoError)
            {
                throw new Exception(error.ToString());
            }
        }

        private void Update(object sender, AudioComposerDataEventArgs e)
        {
            // Buffer incoming data.
            var data = e.Data
                .Select(s => s == 0 ? MidLevel : (s > 0 ? _highLevel : _lowLevel))
                .SelectMany(s => new[] { (byte)(s & 0xFF), (byte)((s >> 8) & 0xFF) })
                .ToArray();
            _buffer.AddRange(data);
            
            // Unload spent OpenAL buffers.
            AL.GetSource(_alSourceHandle, ALGetSourcei.BuffersProcessed, out var buffersToUnload);
            AssertIfError();
            if (buffersToUnload > 0)
            {
                _bufferCount -= buffersToUnload;
                
                var bufferIds = AL.SourceUnqueueBuffers(_alSourceHandle, buffersToUnload);
                AssertIfError();
                AL.DeleteBuffers(bufferIds);
                AssertIfError();
            }
            
            AL.GetSource(_alSourceHandle, ALGetSourcei.SourceState, out var sourceStateValue);
            var sourceState = (ALSourceState) sourceStateValue;

            // Transfer the buffer to OpenAL if it is large enough.
            while (_buffer.Count >= BufferSampleCount)
            {
                if (_bufferCount < 20)
                {
                    var output = _buffer
                        .Take(BufferSampleCount)
                        .ToArray();
                    
                    var alBufferHandle = AL.GenBuffer();
                    AssertIfError();
                    AL.BufferData(alBufferHandle, ALFormat.Mono16, output, output.Length, AudioComposer.SampleRate);
                    AssertIfError();
                    AL.SourceQueueBuffer(_alSourceHandle, alBufferHandle);
                    AssertIfError();
                    _bufferCount++;
                }

                if (sourceState != ALSourceState.Playing && _bufferCount > 1)
                {
                    if (_bufferCount > 1)
                        AL.SourcePlay(_alSourceHandle);
                    AssertIfError();                    
                }
                
                _buffer.RemoveRange(0, BufferSampleCount);
            }
            
            _context.Process();
        }

        public void Stop()
        {
            if (!_running)
                return;

            AudioComposer.BufferReady -= Update;
            _running = false;
        }

        public double Volume {
            get => _volume;
            set
            {
                if (value > 1)
                    value = 1;
                else if (value < 0)
                    value = 0;

                _volume = value;
                _highLevel = (short) (short.MaxValue*value);
                _lowLevel = (short)-_highLevel;
            }
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;
            Stop();
            _context.Dispose();
        }
    }
}