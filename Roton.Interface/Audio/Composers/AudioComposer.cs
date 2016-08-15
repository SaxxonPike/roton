using System;
using System.Collections.Generic;
using System.Linq;
using Roton.Core;

namespace Roton.Interface.Audio.Composers
{
    public class AudioComposer : IAudioComposer
    {
        private const double AccumulatorMultiplier = 1000;

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
        private int _accumulator;

        public AudioComposer(IDrumBank drumBank, int outputSampleRate, int samplesPerDrumFrequency)
        {
            _drumBank = drumBank;
            _samplesPerDrumFrequency = samplesPerDrumFrequency;
            _accumulatorLimit = (int)(outputSampleRate * AccumulatorMultiplier);

            // base note is C-2 (24) but our 440 frequency reference is A-2 (33)

            _frequencyDutyCycleTable =
                Enumerable.Range(0, 12 * 6)
                .Select(i => outputSampleRate * AccumulatorMultiplier / (440d * Math.Pow(2d, (double)(i - 33) / 12)))
                .Select(i => (int)i)
                .ToArray();
        }

        public IEnumerable<int> ComposeAudio()
        {
            if (!_generating)
                yield return 0;

            if (_drumSoundSamplesRemaining > 0)
            {
                _drumSoundSamplesRemaining--;
                if (_drumSoundSamplesRemaining <= 0)
                {
                    if (_drumSoundFrequenciesRemaining <= 0)
                    {
                        _generating = false;
                        yield return 0;
                    }
                    else
                    {
                        _drumSoundFrequenciesRemaining--;
                        _drumSoundFrequencyIndex++;
                        _accumulatorAmount = _currentDrumSound[_drumSoundFrequencyIndex];
                        _drumSoundSamplesRemaining = _samplesPerDrumFrequency;
                    }
                    _drumSoundFrequencyIndex++;
                }
            }

            if (_generating)
            {
                _accumulator += _accumulatorAmount;
                while (_accumulator > _accumulatorLimit)
                {
                    _accumulator -= _accumulatorLimit;
                    _dutyLevel = !_dutyLevel;
                }
            }

            yield return _dutyLevel ? 1 : -1;
        }

        public void PlayDrum(int index)
        {
            _drumSoundSamplesRemaining = _samplesPerDrumFrequency;
            _currentDrumSound = _drumBank[index];
            _drumSoundFrequenciesRemaining = _currentDrumSound.Count;
            _drumSoundFrequencyIndex = 0;
            _accumulatorAmount = _currentDrumSound[0];
            _generating = true;
        }

        public void PlayNote(int note)
        {
            _drumSoundSamplesRemaining = 0;
            _accumulatorAmount = _frequencyDutyCycleTable[note];
            _generating = true;
        }
    }
}