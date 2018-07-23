using Roton.Emulation.Core;

namespace Lyon.App
{
    public class SpeakerProxy : ISpeaker
    {
        private readonly IComposerProxy _composerProxy;

        public SpeakerProxy(IComposerProxy composerProxy)
        {
            _composerProxy = composerProxy;
        }

        public void PlayDrum(int drum) =>
            _composerProxy.AudioComposer?.PlayDrum(drum);


        public void PlayNote(int note) =>
            _composerProxy.AudioComposer?.PlayNote(note);

        public void StopNote() =>
            _composerProxy.AudioComposer?.StopNote();

        public void Tick() =>
            _composerProxy.AudioComposer?.Tick();
    }
}