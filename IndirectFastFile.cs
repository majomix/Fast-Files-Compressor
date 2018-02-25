using System;
using System.Collections.Generic;

namespace FastFilesCompressor
{
    public class IndirectFastFile : FastFile
    {
        public byte[] InitialMetaData { get; set; }
    }
}
