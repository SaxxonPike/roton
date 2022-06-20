namespace Roton.Emulation.Core;

public interface IConfigFileService
{
    IConfigFile Load();
    void Save(IConfigFile configFile);
}