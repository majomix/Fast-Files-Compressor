using System;
using System.Collections.Generic;

namespace FastFilesCompressor
{
    public class DeltaFastFile : FastFile
    {
        public byte[] InitialMetaData { get; set; }
    }
}
