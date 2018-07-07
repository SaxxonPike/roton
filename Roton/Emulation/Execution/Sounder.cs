using Roton.Core;

namespace Roton.Emulation.Execution
{
    public class Sounder : ISounder
    {
        private readonly IState _state;

        public Sounder(IState state)
        {
            _state = state;
        }
        
        public void Clear()
        {
            _state.SoundPlaying = false;
            Stop();
        }

        public ISound Encode(string music)
        {
            return new Sound();
        }        
        
        public void Play(int priority, ISound sound, int offset, int length)
        {
        }
        
        public void Stop()
        {
        }

    }

    public interface ISounder
    {
        void Clear();
        ISound Encode(string music);
        void Play(int priority, ISound sound, int offset, int length);
        void Stop();
    }
}