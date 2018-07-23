namespace Roton.Emulation.Core
{
    public interface ISpeaker
    {
        void PlayDrum(int drum);
        void PlayNote(int note);
        void Tick();
        void StopNote();
    }
}