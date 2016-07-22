namespace Roton.Emulation.Execution
{
    internal abstract partial class Core
    {
        internal virtual byte[] PlayMusic(string music)
        {
            return new byte[0];
        }

        public virtual void PlaySound(int priority, byte[] sound)
        {
        }

        internal virtual void StopSound()
        {
        }
    }
}