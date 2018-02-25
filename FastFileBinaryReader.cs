using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FastFilesCompressor
{
    public class FastFileBinaryReader : BinaryReader
    {
        public FastFileBinaryReader(FileStream fileStream)
            : base(fileStream) { }

        public string ReadFixedSizeString(int size)
        {
            return Encoding.ASCII.GetString(ReadBytes(size));
        }

        public FastFileHeader ReadFastFileHeader()
        {
            FastFileHeader header = new FastFileHeader();

            header.Signature = ReadFixedSizeString(8);
            header.Version = ReadInt32();
            header.Build = ReadInt32();
            header.Depth = ReadInt32();
            header.MaximumChunkSize = ReadInt32();
            header.Zeros = ReadBytes(12);

            return header;
        }

        public IndirectFastFileWrapper ReadIndirectFastFile()
        {
            IndirectFastFileWrapper indirectFastFile = new IndirectFastFileWrapper();

            indirectFastFile.Header = ReadFastFileHeader();
            indirectFastFile.Indirection = ReadBytes(20);

            return indirectFastFile;
        }

        public FastFile ReadFastFileBody()
        {
            FastFile fastFile = new FastFile();

            long numberOfEntries = ReadInt32();

            List<byte[]> references = new List<byte[]>();
            fastFile.References = references;

            for(int i = 0; i < numberOfEntries; i++)
            {
                references.Add(ReadBytes(16));
            }

            fastFile.CompressedFileSize = ReadInt64();
            ConfirmEquality(fastFile.CompressedFileSize, ReadInt64());

            fastFile.UncompressedFileSize = ReadInt64();
            fastFile.MetaData = ReadBytes(72);
            ConfirmEquality(fastFile.UncompressedFileSize, ReadInt64());
            fastFile.CompressionType = ReadInt32();

            return fastFile;
        }

        public IndirectFastFile ReadIndirectFastFileBody()
        {
            IndirectFastFile fastFile = new IndirectFastFile();

            long numberOfEntries = ReadInt32();
            fastFile.CompressedFileSize = ReadInt64();
            ConfirmEquality(fastFile.CompressedFileSize, ReadInt64());
            fastFile.InitialMetaData = ReadBytes(40);
            fastFile.UncompressedFileSize = ReadInt64();

            List<byte[]> references = new List<byte[]>();
            fastFile.References = references;

            for (int i = 0; i < numberOfEntries; i++)
            {
                references.Add(ReadBytes(16));
            }

            fastFile.MetaData = ReadBytes(80);
            ConfirmEquality(fastFile.UncompressedFileSize, ReadInt64());
            fastFile.CompressionType = ReadInt32();

            return fastFile;
        }

        private void ConfirmEquality(Int64 value, Int64 compared)
        {
            if (value != compared)
            {
                throw new InvalidDataException();
            }
        }
    }
}
