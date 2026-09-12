using AwesomeAssertions;
using Moq;
using NUnit.Framework;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Gameplay;

public class SoundPlayerTests(Context context) : AllContextTestFixture(context)
{
    [Test]
    public void PlaySound_ShouldQueueMusic_WhenNotPlaying()
    {
        // Set up two notes of music.
        byte[] music1 = [0x30, 0x01, 0x32, 0x01];

        // Queue up some music.
        SoundPlayer.PlaySound(-1, music1);

        // Assert.
        ((bool)State.SoundPlaying).Should().BeTrue();
        ((int)State.SoundPriority).Should().Be(-1);
        State.SoundBuffer.Count.Should().Be(4);
    }

    [Test]
    public void PlaySound_ShouldAppendMusic_WhenMusicAlreadyPlaying()
    {
        // Set up two passages of music.
        byte[] music1 = [0x30, 0x01];
        byte[] music2 = [0x32, 0x01];

        // Queue them both up.
        SoundPlayer.PlaySound(-1, music1);
        SoundPlayer.PlaySound(-1, music2);

        // Assert.
        ((bool)State.SoundPlaying).Should().BeTrue();
        ((int)State.SoundPriority).Should().Be(-1);
        State.SoundBuffer.Count.Should().Be(4);
    }

    [Test]
    public void PlaySound_ShouldRejectSoundEffects_WhenMusicIsPlaying()
    {
        // Set up music and a sound effect.
        byte[] music = [0x30, 0x01];
        byte[] sfx = [0x20, 0x01];

        // Queue them both up, music first.
        SoundPlayer.PlaySound(-1, music);
        SoundPlayer.PlaySound(2, sfx);

        // Assert.
        ((int)State.SoundPriority).Should().Be(-1);
        State.SoundBuffer.Count.Should().Be(2);
    }

    [Test]
    public void PlaySound_ShouldAppendMusic_WhenSoundEffectIsPlaying()
    {
        // Set up music and a sound effect.
        byte[] sfx = [0x20, 0x01];
        byte[] music = [0x30, 0x01];

        // Queue them both up, sound effect first.
        SoundPlayer.PlaySound(1, sfx);
        SoundPlayer.PlaySound(-1, music);

        // Assert.
        ((int)State.SoundPriority).Should().Be(1);
        State.SoundBuffer.Count.Should().Be(4);
    }

    [Test]
    public void PlaySound_ShouldInterruptSound_WhenHigherPrioritySoundPlays()
    {
        // Set up two sound effects.
        byte[] sfx1 = [0x20, 0x05];
        byte[] sfx2 = [0x40, 0x01];

        // Queue and start one sound effect.
        SoundPlayer.PlaySound(1, sfx1);
        SoundPlayer.UpdateSound();
        ((int)State.SoundTicks).Should().Be(0x13);

        // Queue a second sound effect with a higher priority value.
        SoundPlayer.PlaySound(2, sfx2);
        ((int)State.SoundTicks).Should().Be(0);
        ((int)State.SoundPriority).Should().Be(2);
        
        // Assert.
        State.SoundBuffer.Count.Should().Be(2);
    }

    [Test]
    public void ClearSound_ShouldStopSoundAndClearBuffer()
    {
        // Set up music.
        byte[] music = [0x30, 0x01, 0x32, 0x01];
        
        // Play it, then stop it.
        SoundPlayer.PlaySound(-1, music);
        SoundPlayer.ClearSound();

        // Assert.
        ((bool)State.SoundPlaying).Should().BeFalse();
        State.SoundBuffer.Count.Should().Be(0);
        SpeakerMock.Verify(s => s.StopNote(), Times.AtLeastOnce);
    }

    [Test]
    public void PlaySound_ShouldIgnoreEmptySound()
    {
        // Start playing an empty sound sequence.
        SoundPlayer.PlaySound(-1, []);

        // Assert.
        ((bool)State.SoundPlaying).Should().BeFalse();
        State.SoundBuffer.Count.Should().Be(0);
    }
}