using System;
using System.Buffers;
using Roton.Composers.Audio.Drums;
using Roton.Composers.Audio.Steps;
using Roton.Composers.Audio.Tones;
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

        tempBuffer.Clear();
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
        var drumValues = (stackalloc int[drum.Count]);

        for (var i = 0; i < drum.Count; i++)
            drumValues[i] = drum[i];

        Clear();
        drumComposer.SetDrum(drumValues, config.AudioSampleRate / (float)config.AudioDrumRate);
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
        var mem = MemoryPool<float>.Shared.Rent(length);
        var buffer = mem.Memory.Span.Slice(0, length);

        ComposeAudio(buffer);

        var args = new AudioStreamDataEventArgs(mem, length);
        BufferReady?.Invoke(this, args);
    }

    private void SetSampleRate(int value)
    {
        _sampleRate = value;
        _bufferDenominator = config.MasterClockDenominator;
        _bufferNumerator = _sampleRate * config.MasterClockNumerator;
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