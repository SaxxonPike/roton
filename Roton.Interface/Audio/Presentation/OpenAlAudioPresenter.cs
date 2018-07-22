using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using Roton.Emulation.Core;
using Roton.Interface.Audio.Composition;

namespace Roton.Interface.Audio.Presentation
{
    public class OpenAlAudioPresenter : IDisposable, IAudioPresenter
    {
        private readonly IClockFactory _clockFactory;
        private readonly IAudioComposer _audioComposer;
        private const int BufferSampleCount = 128;
        private bool _isDisposed;
        private readonly int _alSourceHandle;
        private readonly Queue<int> _alBufferHandles;
        private short _highLevel;
        private short _lowLevel;
        private const short MidLevel = 0;
        private double _volume;

        public OpenAlAudioPresenter(IClockFactory clockFactory, IAudioComposer audioComposer)
        {
            _clockFactory = clockFactory;
            _audioComposer = audioComposer;
            _alBufferHandles = new Queue<int>();
            _context = new AudioContext();
            _context.MakeCurrent();
            Volume = 0.2;

            _alSourceHandle = AL.GenSource();
            AL.Source(_alSourceHandle, ALSourcef.Gain, 1.0f);
        }

        private readonly AudioContext _context;
        private IClock _clock;

        public void Start()
        {
            if (_clock != null)
                return;
            
            _clock = _clockFactory.Create(_audioComposer.SampleRate, BufferSampleCount);
            _clock.OnTick += (sender, e) => Update();
            _clock.Start();
        }

        private void AssertIfError()
        {
            var error = AL.GetError();
            if (error != ALError.NoError)
            {
                throw new Exception(error.ToString());
            }
        }

        private void Update()
        {
            var data = _audioComposer
                .ComposeAudio()
                .Take(BufferSampleCount)
                .Select(s => s == 0 ? MidLevel : (s > 0 ? _highLevel : _lowLevel))
                .SelectMany(s => new[] { (byte)(s & 0xFF), (byte)((s >> 8) & 0xFF) })
                .ToArray();

            AL.GetSource(_alSourceHandle, ALGetSourcei.BuffersProcessed, out var buffersToUnload);
            AssertIfError();
            if (buffersToUnload > 0)
            {
                AL.SourceUnqueueBuffers(_alSourceHandle, buffersToUnload);
                AssertIfError();
                for (var i = 0; i < buffersToUnload; i++)
                {
                    var bufferToDelete = _alBufferHandles.Dequeue();
                    AL.DeleteBuffer(bufferToDelete);
                }
            }

            var alBufferHandle = AL.GenBuffer();
            AssertIfError();
            _alBufferHandles.Enqueue(alBufferHandle);
            AL.BufferData(alBufferHandle, ALFormat.Mono16, data, data.Length, _audioComposer.SampleRate);
            AssertIfError();
            AL.SourceQueueBuffer(_alSourceHandle, alBufferHandle);
            AssertIfError();
            AL.SourcePlay(_alSourceHandle);
            AssertIfError();
            _context.Process();
        }

        public void Stop()
        {
            _clock.Stop();
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