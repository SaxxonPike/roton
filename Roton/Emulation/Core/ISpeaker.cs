namespace Roton.Emulation.Core
{
    public interface ISpeaker
    {
        void PlayDrum(int drum);
        void PlayNote(int note);
        void PlayStep();
        void Tick();
        void StopNote();
    }
}