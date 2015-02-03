using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    internal class DatArchive : IFileSystem
    {
        private const int DAT_ARCHIVE_ENTRIES = 24;
        private const int DAT_ARCHIVE_NAME_LENGTH = 50;

        public DatArchive(byte[] data)
        {
            this.FileData = new Dictionary<string, byte[]>();
            this.Data = data;
        }

        public void ChangeDirectory(string relativeDirectory)
        {
            throw new NotSupportedException(@"Can't change directory in a DAT file.");
        }

        protected byte[] Data
        {
            get
            {
                return DataBytes;
            }
            set
            {
                FileData.Clear();
                DataBytes = value;
                using (MemoryStream mem = new MemoryStream(DataBytes))
                {
                    BinaryReader reader = new BinaryReader(mem);
                    int count = reader.ReadInt16();
                    string[] names = new string[DAT_ARCHIVE_ENTRIES];
                    int[] offsets = new int[DAT_ARCHIVE_ENTRIES];
                    byte[][] data = new byte[DAT_ARCHIVE_ENTRIES][];

                    // get virtual file names
                    for (int i = 0; i < DAT_ARCHIVE_ENTRIES; i++)
                    {
                        if (i < count)
                        {
                            int nameLength = reader.ReadByte();
                            byte[] nameData = reader.ReadBytes(nameLength);

                            // read garbage bytes
                            reader.ReadBytes(DAT_ARCHIVE_NAME_LENGTH - nameLength);

                            names[i] = nameData.ToStringValue();
                        }
                        else
                        {
                            reader.ReadBytes(51);
                        }
                    }

                    // get offsets within the data
                    for (int i = 0; i < DAT_ARCHIVE_ENTRIES; i++)
                    {
                        offsets[i] = reader.ReadInt32();
                    }

                    // read from each offset
                    for (int i = 0; i < count; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(names[i]))
                        {
                            mem.Position = offsets[i];
                            StringBuilder lineBuilder = new StringBuilder();
                            while (true)
                            {
                                int lineLength = reader.ReadByte();
                                string line = reader.ReadBytes(lineLength).ToStringValue();
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

        protected byte[] DataBytes
        {
            get;
            set;
        }

        protected Dictionary<string, byte[]> FileData
        {
            get;
            set;
        }

        protected List<string> Files
        {
            get;
            set;
        }

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
