namespace Roton.Emulation.Execution
{
    internal abstract partial class CoreBase
    {
        internal virtual byte[] PlayMusic(string music)
        {
            return new byte[0];
        }

        internal virtual void PlaySound(int priority, byte[] sound)
        {
        }

        internal virtual void StopSound()
        {
        }
    }
}