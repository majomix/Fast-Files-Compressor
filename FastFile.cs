using System;
using System.Collections.Generic;

namespace FastFilesCompressor
{
    public class FastFile
    {
        public IEnumerable<byte[]> References { get; set; }
        public Int64 CompressedFileSize { get; set; }
        public Int64 UncompressedFileSize { get; set; }
        public byte[] MetaData { get; set; }
        public Int64 UncompressedFileSizeConfirmed { get; set; }
        public Int32 CompressionType { get; set; }
    }
}
