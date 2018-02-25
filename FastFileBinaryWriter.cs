using System.IO;

namespace FastFilesCompressor
{
    public class FastFileBinaryWriter : BinaryWriter
    {
        public FastFileBinaryWriter(FileStream fileStream)
            : base(fileStream) { }

    }
}
