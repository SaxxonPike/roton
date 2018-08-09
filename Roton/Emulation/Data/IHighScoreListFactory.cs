namespace Roton.Emulation.Data
{
    public interface IHighScoreListFactory
    {
        IHighScoreList Load();
        void Save(IHighScoreList highScoreList);
    }
}