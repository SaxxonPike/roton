using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Roton.Emulation
{
    internal class DatArchive : IFileSystem
    {
        private const int DatArchiveEntries = 24;
        private const int DatArchiveNameLength = 50;

        public DatArchive(byte[] data)
        {
            FileData = new Dictionary<string, byte[]>();
            Data = data;
        }

        public void ChangeDirectory(string relativeDirectory)
        {
            throw new NotSupportedException(@"Can't change directory in a DAT file.");
        }

        protected byte[] Data
        {
            get { return DataBytes; }
            set
            {
                FileData.Clear();
                DataBytes = value;
                using (var mem = new MemoryStream(DataBytes))
                {
                    var reader = new BinaryReader(mem);
                    int count = reader.ReadInt16();
                    var names = new string[DatArchiveEntries];
                    var offsets = new int[DatArchiveEntries];
                    var data = new byte[DatArchiveEntries][];

                    // get virtual file names
                    for (var i = 0; i < DatArchiveEntries; i++)
                    {
                        if (i < count)
                        {
                            int nameLength = reader.ReadByte();
                            var nameData = reader.ReadBytes(nameLength);

                            // read garbage bytes
                            reader.ReadBytes(DatArchiveNameLength - nameLength);

                            names[i] = nameData.ToStringValue();
                        }
                        else
                        {
                            reader.ReadBytes(51);
                        }
                    }

                    // get offsets within the data
                    for (var i = 0; i < DatArchiveEntries; i++)
                    {
                        offsets[i] = reader.ReadInt32();
                    }

                    // read from each offset
                    for (var i = 0; i < count; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(names[i]))
                        {
                            mem.Position = offsets[i];
                            var lineBuilder = new StringBuilder();
                            while (true)
                            {
                                int lineLength = reader.ReadByte();
                                var line = reader.ReadBytes(lineLength).ToStringValue();
                                if (line != @"@")
                                {
                                    if (lineBuilder.Length > 0)
                                    {
                                        lineBuilder.Append('\x000D');
                                    }
                                    lineBuilder.Append(line);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            FileData[names[i]] = lineBuilder.ToString().ToBytes();
                        }
                    }
                }
            }
        }

        protected byte[] DataBytes { get; set; }

        protected Dictionary<string, byte[]> FileData { get; set; }

        protected List<string> Files { get; set; }

        public IList<string> GetDirectories()
        {
            return new List<string>();
        }

        public IList<string> GetFiles()
        {
            return new List<string>(Files);
        }

        public byte[] ReadFile(string filename)
        {
            return FileData[filename.ToLowerInvariant()];
        }

        public void WriteFile(string filename, byte[] data)
        {
            throw new NotSupportedException(@"Can't write to a DAT file.");
        }
    }
}