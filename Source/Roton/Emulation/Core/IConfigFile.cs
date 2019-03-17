using Roton.Emulation.Core.Impl;

namespace Roton.Emulation.Core
{
    public interface IConfigFile
    {
        ConfigFileFormat Format { get; }
        int Value0 { get; }
        int Value1 { get; }
        int Value2 { get; }
        int Value3 { get; }
        string WorldName { get; }
        string RegistrationType { get; }
        string RegistrationName { get; }
    }
}