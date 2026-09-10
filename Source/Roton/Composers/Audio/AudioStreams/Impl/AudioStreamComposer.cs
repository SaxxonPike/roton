using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Roton.Composers.Audio.Drums;
using Roton.Composers.Audio.Steps;
using Roton.Composers.Audio.Tones;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Composers.Audio.AudioStreams.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class AudioStreamComposer(
    IDrumSoundList drumBank,
    IConfig config,
    IDrumComposer drumComposer,
    IToneComposer toneComposer,
    IStepComposer stepComposer)
    : IAudioStreamComposer
{
    public event EventHandler<AudioStreamDataEventArgs>? BufferReady;

    private int _sampleRate;
    private long _bufferAccumulator;
    private long _bufferNumerator;
    private long _bufferDenominator;

    private int ComposeAudio(Span<float> buffer)
    {
        _bufferAccumulator -= _bufferDenominator * buffer.Length;

        var tempBuffer = buffer;

        var stepLen = stepComposer.ComposeStep(tempBuffer);
        tempBuffer = tempBuffer.Slice(stepLen);

        var drumLen = drumComposer.ComposeDrum(tempBuffer);
        tempBuffer = tempBuffer.Slice(drumLen);

        var toneLen = toneComposer.ComposeTone(tempBuffer);
        tempBuffer = tempBuffer.Slice(toneLen);

        var count = buffer.Length - tempBuffer.Length;
        var outBuffer = buffer.Slice(0, count);
        tempBuffer.Clear();

        if (count > 0)
        {
            // SIMD assisted amplification.

            var vecBuffer = MemoryMarshal.Cast<float, Vector4>(outBuffer);
            var vecLeftover = outBuffer.Slice(vecBuffer.Length * 4);
            var gain = config.Audio.PreGain * config.Audio.Gain;

            for (var i = 0; i < vecBuffer.Length; i++)
                vecBuffer[i] *= gain;
        
            for (var i = 0; i < vecLeftover.Length; i++)
                vecLeftover[i] *= gain;
        }
        
        return buffer.Length;
    }

    private void Clear()
    {
        stepComposer.ClearStep();
        toneComposer.ClearTone();
        drumComposer.ClearDrum();
    }

    public void PlayDrum(int index)
    {
        var drum = drumBank[index];
        int len = drum[0];
        var src = drum.Slice(1, len);
        var dest = (stackalloc int[len]);

        for (var i = 0; i < src.Length; i++)
            dest[i] = src[i];

        Clear();
        drumComposer.SetDrum(dest, config.Audio.SampleRate / (float)config.Audio.DrumSpeed);
    }

    public void PlayNote(int note)
    {
        Clear();
        toneComposer.SetTone(note);
    }

    public void PlayStep()
    {
        Clear();
        stepComposer.SetStep();
    }

    public void StopNote()
    {
        Clear();
    }

    public void Tick()
    {
        _bufferAccumulator += _bufferNumerator;

        var length = (int)(_bufferAccumulator / _bufferDenominator);
        var mem = new TempMemory<float>(length);
        var buffer = mem.Span;

        var actual = ComposeAudio(buffer);
        var args = new AudioStreamDataEventArgs(mem, actual);
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