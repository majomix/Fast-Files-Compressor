using System;
using System.Collections.Generic;

namespace FastFilesCompressor
{
    public class FastFile
    {
        public FastFileHeader Header { get; set; }
        public IEnumerable<byte[]> References { get; set; }
        public UInt64 CompressedFileSize { get; set; }
        public UInt64 CompressedFileSizeConfirmed { get; set; }
        public UInt64 UncompressedFileSize { get; set; }
        public byte[] MetaData { get; set; }
        public UInt64 UncompressedFileSizeConfirmed { get; set; }
        public Int32 CompressionType { get; set; }
    }
}
