namespace Roton.Zip
{
    internal enum ZipCompressionMethod
    {
        Stored = 0,
        Shrunk = 1,
        Reduced1 = 2,
        Reduced2 = 3,
        Reduced3 = 4,
        Reduced4 = 5,
        Imploded = 6,
        Tokenized = 7,
        Deflated = 8,
        EnhancedDeflate = 9,
        IbmTerseOld = 10,
        Reserved11 = 11,
        BZip2 = 12,
        Reserved13 = 13,
        Lzma = 14,
        Reserved15 = 15,
        Reserved16 = 16,
        Reserved17 = 17,
        IbmTerseNew = 18,
        Ibmlz77 = 19,
        WavPack = 97,
        PpMd = 98
    }
}