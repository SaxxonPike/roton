namespace Roton.Emulation.Core.Impl
{
    public sealed class ConfigFile : IConfigFile
    {
        public ConfigFileFormat Format { get; set; }
        public int Value0 { get; set; }
        public int Value1 { get; set; }
        public int Value2 { get; set; }
        public int Value3 { get; set; }
        public string WorldName { get; set; }
        public string RegistrationType { get; set; }
        public string RegistrationName { get; set; }
    }
}