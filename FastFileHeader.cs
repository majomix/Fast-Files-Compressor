using System;

namespace FastFilesCompressor
{
    public class FastFileHeader
    {
        public string Signature { get; set; }
        public Int32 Version { get; set; }
        public Int32 Build { get; set; }
        public Int32 Depth { get; set; }
        public Int32 MaximumChunkSize { get; set; }
        public byte[] Zeros { get; set; }
    }
}
