using System;
using System.Collections.Concurrent;
using System.Numerics;
using System.Runtime.InteropServices;
using Roton.Emulation.Core;
using Roton.Infrastructure;

namespace Roton.Composers.Audio.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class AudioComposer(
    IConfig config,
    ISynth synth)
    : IAudioComposer
{
    public event EventHandler<AudioDataEventArgs>? BufferReady;

    private int _sampleRate;
    private long _bufferAccumulator;
    private long _bufferNumerator;
    private long _bufferDenominator;
    private readonly ConcurrentQueue<SpeakerTone> _speakerTones = new();
    private int _durationSamples;

    private int ComposeAudio(Span<float> buffer)
    {
        _bufferAccumulator -= _bufferDenominator * buffer.Length;

        var tempBuffer = buffer;

        while (tempBuffer.Length > 0)
        {
            if (_durationSamples > 0)
            {
                var requested = Math.Min(_durationSamples, tempBuffer.Length);
                var rendered = synth.Render(tempBuffer.Slice(0, requested));
                tempBuffer = tempBuffer.Slice(rendered);
                _durationSamples -= rendered;
                continue;
            }

            if (!_speakerTones.TryDequeue(out var nextTone))
            {
                var rendered = synth.Render(tempBuffer);
                _durationSamples -= rendered;
                tempBuffer = tempBuffer.Slice(rendered);
                break;
            }

#if NET10_0_OR_GREATER
            _durationSamples = (int)MathF.Round(nextTone.Duration * _sampleRate);
#else
            _durationSamples = (int)Math.Round(nextTone.Duration * _sampleRate);
#endif
            synth.SetFrequency(nextTone.Frequency);
        }

        // SIMD assisted amplification.

        var gain = config.Audio.PreGain * config.Audio.Gain;
        if (gain != 1.0f)
        {
            var outBuffer = buffer.Slice(0, buffer.Length - tempBuffer.Length);
            var vecBuffer = MemoryMarshal.Cast<float, Vector<float>>(outBuffer);
            var gainVector = new Vector<float>(gain);

            for (var i = 0; i < vecBuffer.Length; i++)
                vecBuffer[i] *= gainVector;

            var vecLeftover = outBuffer.Slice(vecBuffer.Length * Vector<float>.Count);
            for (var i = 0; i < vecLeftover.Length; i++)
                vecLeftover[i] *= gain;
        }

        return buffer.Length;
    }

    public void PlayToneSequence(ReadOnlySpan<SpeakerTone> tones)
    {
        StopTone();
        foreach (var tone in tones)
            _speakerTones.Enqueue(tone);
    }

    public void PlayTone(float frequency)
    {
        StopTone();
        _speakerTones.Enqueue(new SpeakerTone(frequency, 0));
    }

    public void StopTone()
    {
        while (_speakerTones.TryDequeue(out _))
        {
        }

        synth.SetFrequency(0);
    }

    public void Tick()
    {
        _bufferAccumulator += _bufferNumerator;

        var length = (int)(_bufferAccumulator / _bufferDenominator);
        var mem = new TempMemory<float>(length);
        var buffer = mem.Span;

        var actual = ComposeAudio(buffer);
        var args = new AudioDataEventArgs(mem, actual);
        BufferReady?.Invoke(this, args);
    }

    private void SetSampleRate(int value)
    {
        _sampleRate = value;
        _bufferDenominator = config.Engine.MasterClockDenominator;
        _bufferNumerator = _sampleRate * config.Engine.MasterClockNumerator;
        _bufferAccumulator = 0;
    }

    public int SampleRate
    {
        get => _sampleRate;
        set
        {
            if (_sampleRate != value)
                SetSampleRate(value);
        }
    }
}