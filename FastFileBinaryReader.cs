using System.IO;

namespace FastFilesCompressor
{
    public class FastFileBinaryReader : BinaryReader
    {
        public FastFileBinaryReader(FileStream fileStream)
            : base(fileStream) { }

    }
}
