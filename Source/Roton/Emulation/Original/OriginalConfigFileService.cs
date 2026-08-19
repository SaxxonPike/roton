using System;
using System.Linq;
using System.Text;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public sealed class OriginalConfigFileService(IFileSystem fileSystem) : IConfigFileService
{
    private const string ConfigFileName = "ZZT.CFG";

    private IFileSystem FileSystem => fileSystem;

    private static ConfigFile Decode(byte[] data)
    {
        var buffer = Enumerable.Range(0, 8).Select(_ => string.Empty).ToArray();
        var lines = data
            .ToStringValue()
            .Replace("\r\n", "\r")
            .Split('\r');

        Array.Copy(buffer, lines, Math.Min(buffer.Length, lines.Length));

        if (lines.Length >= 4 && int.TryParse(buffer[0], out var value0))
        {
            int.TryParse(buffer[1], out var value1);
            int.TryParse(buffer[2], out var value2);
            int.TryParse(buffer[3], out var value3);

            return new ConfigFile
            {
                Format = ConfigFileFormat.Original30,
                Value0 = value0,
                Value1 = value1,
                Value2 = value2,
                Value3 = value3,
                WorldName = buffer[4],
                RegistrationType = buffer[5],
                RegistrationName = buffer[6]
            };
        }

        return new ConfigFile
        {
            Format = ConfigFileFormat.Original32,
            WorldName = buffer[0],
            RegistrationName = buffer[1]
        };
    }

    private static byte[] Encode(IConfigFile configFile)
    {
        var output = new StringBuilder();

        switch (configFile.Format)
        {
            case ConfigFileFormat.Original30:
            {
                output.AppendLine(configFile.Value0.ToString());
                output.AppendLine(configFile.Value1.ToString());
                output.AppendLine(configFile.Value2.ToString());
                output.AppendLine(configFile.Value3.ToString());
                output.AppendLine(configFile.WorldName);
                output.AppendLine(configFile.RegistrationType);
                output.AppendLine(configFile.RegistrationName);
                break;
            }
            case ConfigFileFormat.Original32:
            {
                output.AppendLine(configFile.WorldName);
                output.AppendLine(configFile.RegistrationName);
                break;
            }
        }

        return output.ToString().ToBytes();
    }

    public IConfigFile? Load()
    {
        var file = FileSystem.GetFile(ConfigFileName);
        return file != null ? Decode(file) : null;
    }

    public void Save(IConfigFile configFile)
    {
        var file = Encode(configFile);
        FileSystem.PutFile(ConfigFileName, file);
    }
}