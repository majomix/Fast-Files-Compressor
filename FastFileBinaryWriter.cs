using System.IO;
using System.Text;
using System.Linq;

namespace FastFilesCompressor
{
    public class FastFileBinaryWriter : BinaryWriter
    {
        public FastFileBinaryWriter(FileStream fileStream)
            : base(fileStream) { }

        public override void Write(string value)
        {
            Write(Encoding.ASCII.GetBytes(value));
        }

        public void Write(FastFileHeader header)
        {
            Write(header.Signature);
            Write(header.Version);
            Write(header.Build);
            Write(header.Depth);
            Write(header.MaximumChunkSize);
            Write(header.Zeros);
        }

        public void Write(FastFile fastFile)
        {
            Write(fastFile.References.Count());

            foreach (byte[] reference in fastFile.References)
            {
                Write(reference);
            }

            Write(fastFile.CompressedFileSize);
            Write(fastFile.CompressedFileSize);
            Write(fastFile.UncompressedFileSize);
            Write(fastFile.MetaData);
            Write(fastFile.UncompressedFileSize);
            Write(fastFile.CompressionType);
        }

        public void Write(DeltaFastFile fastFile)
        {
            Write(fastFile.References.Count());
            Write(fastFile.CompressedFileSize);
            Write(fastFile.CompressedFileSize);
            Write(fastFile.InitialMetaData);
            Write(fastFile.UncompressedFileSize);

            foreach (byte[] reference in fastFile.References)
            {
                Write(reference);
            }

            Write(fastFile.MetaData);
            Write(fastFile.UncompressedFileSize);
            Write(fastFile.CompressionType);
        }
    }
}
