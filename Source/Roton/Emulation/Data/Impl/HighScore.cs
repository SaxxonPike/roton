namespace Roton.Emulation.Data.Impl
{
    public class HighScore : IHighScore
    {
        internal HighScore()
        {
        }
        
        public string Name { get; set; }
        public int Score { get; set; }
    }
}