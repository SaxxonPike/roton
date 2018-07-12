namespace Roton.Emulation.Data.Impl
{
    public sealed class Sound : ISound
    {
        private readonly int[] _data;

        public Sound(params int[] data)
        {
            _data = data;
        }

        public int this[int index] => _data[index];
        public int Length => _data.Length;
    }
}