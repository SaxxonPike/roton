using System;

namespace Roton.Zip
{
    internal class ZipArchiveFile
    {
        public int CompressedSize { get; set; }

        public bool Compression1
        {
            get { return GetFlagBit(1); }
            set { SetFlagBit(1, value); }
        }

        public bool Compression2
        {
            get { return GetFlagBit(2); }
            set { SetFlagBit(2, value); }
        }

        public bool CompressedPatch
        {
            get { return GetFlagBit(5); }
            set { SetFlagBit(5, value); }
        }

        public ZipCompressionMethod CompressionMethod { get; set; }

        public int Crc32 { get; set; }

        public byte[] Data { get; set; }

        public bool Encrypted
        {
            get { return GetFlagBit(0); }
            set { SetFlagBit(0, value); }
        }

        public bool EncryptedDirectory
        {
            get { return GetFlagBit(13); }
            set { SetFlagBit(13, value); }
        }

        public bool EnhancedCompression
        {
            get { return GetFlagBit(12); }
            set { SetFlagBit(12, value); }
        }

        public bool EnhancedDeflate
        {
            get { return GetFlagBit(4); }
            set { SetFlagBit(4, value); }
        }

        public byte[] Extra { get; set; }

        public string FileName { get; set; }

        public int Flags { get; set; }

        bool GetFlagBit(int index)
        {
            return (Flags & (0x1 << index)) != 0;
        }

        public bool HasDataDescriptor
        {
            get { return GetFlagBit(3); }
            set { SetFlagBit(3, value); }
        }

        public DateTime LastModified { get; set; }

        void SetFlagBit(int index, bool value)
        {
            var bit = (value ? 1 : 0) << index;
            Flags &= ~bit;
            Flags |= bit;
        }

        public bool StrongEncryption
        {
            get { return GetFlagBit(6); }
            set { SetFlagBit(6, value); }
        }

        public int UncompressedSize { get; set; }

        public bool Utf8
        {
            get { return GetFlagBit(11); }
            set { SetFlagBit(11, value); }
        }

        public int Version { get; set; }
    }
}