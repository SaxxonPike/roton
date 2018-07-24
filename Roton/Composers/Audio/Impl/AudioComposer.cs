using System;
using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Data;

namespace Roton.Composers.Audio.Impl
{
    public class AudioComposer : IAudioComposer
    {
        public event EventHandler<AudioComposerDataEventArgs> BufferReady;
        
        private const int AccumulatorMultiplier = 1000;

        private readonly IDrumBank _drumBank;
        private readonly int _samplesPerDrumFrequency;
        private readonly int[] _frequencyDutyCycleTable;
        private readonly int _accumulatorLimit;

        private int _drumSoundSamplesRemaining;
        private int _drumSoundFrequenciesRemaining;
        private int _drumSoundFrequencyIndex;
        private IDrumSound _currentDrumSound;
        private int _accumulatorAmount;
        private bool _generating;
        private bool _dutyLevel;
        private int _toneAccumulator;
        private int _bufferAccumulator;
        private readonly int _bufferNumerator;
        private readonly int _bufferDenominator;

        public AudioComposer(IDrumBank drumBank, IConfig config)
        {
            _drumBank = drumBank;
            SampleRate = config.AudioSampleRate;
            _samplesPerDrumFrequency = config.AudioDrumRate;
            _accumulatorLimit = SampleRate * AccumulatorMultiplier;

            // base note is C-2 but our 440 frequency reference is A-2

            _frequencyDutyCycleTable =
                Enumerable.Range(0, 12 * 7)
                .Select(i => 440d * Math.Pow(2d, (double)(i - 45) / 12) * (double)AccumulatorMultiplier * 2d)
                .Select(i => (int)i)
                .ToArray();

            _bufferDenominator = 718;
            _bufferNumerator = SampleRate * 10;
        }

        private IEnumerable<float> ComposeAudio()
        {
            while (_bufferAccumulator > _bufferDenominator)
            {
                _bufferAccumulator -= _bufferDenominator;
                if (!_generating)
                {
                    yield return 0;
                    continue;
                }

                if (_drumSoundSamplesRemaining > 0)
                {
                    _drumSoundSamplesRemaining--;
                    if (_drumSoundSamplesRemaining <= 0)
                    {
                        if (_drumSoundFrequenciesRemaining <= 0)
                        {
                            _generating = false;
                            yield return 0;
                            continue;
                        }
                        else
                        {
                            _drumSoundFrequenciesRemaining--;
                            _drumSoundFrequencyIndex++;
                            _accumulatorAmount = _currentDrumSound[_drumSoundFrequencyIndex] * AccumulatorMultiplier * 2;
                            _drumSoundSamplesRemaining = _samplesPerDrumFrequency;
                        }
                    }
                }

                if (_generating)
                {
                    _toneAccumulator += _accumulatorAmount;
                    while (_toneAccumulator > _accumulatorLimit)
                    {
                        _toneAccumulator -= _accumulatorLimit;
                        _dutyLevel = !_dutyLevel;
                    }
                    yield return _dutyLevel ? 1 : -1;
                }
                else
                {
                    yield return 0;
                }
            }
        }

        public void PlayDrum(int index)
        {
            _drumSoundSamplesRemaining = _samplesPerDrumFrequency;
            _currentDrumSound = _drumBank[index];
            _drumSoundFrequenciesRemaining = _currentDrumSound.Count;
            _drumSoundFrequencyIndex = 0;
            _accumulatorAmount = _currentDrumSound[0] * AccumulatorMultiplier;
            _generating = true;
        }

        public void PlayNote(int note)
        {
            _drumSoundSamplesRemaining = 0;
            _accumulatorAmount = _frequencyDutyCycleTable[note];
            _toneAccumulator = 0;
            _generating = true;
        }

        public void StopNote()
        {
            _generating = false;
        }

        public void Tick()
        {
            _bufferAccumulator += _bufferNumerator;
            var args = new AudioComposerDataEventArgs
            {
                Data = ComposeAudio().ToArray()
            };
            BufferReady?.Invoke(this, args);
        }

        public int SampleRate { get; }
    }
}