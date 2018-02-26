using System;
using System.IO;

namespace FastFilesCompressor
{
    public class FastFileCompressor
    {
        public void Decompress(string inputPath, string outputPath)
        {
            using (FastFileBinaryReader reader = new FastFileBinaryReader(File.Open(inputPath, FileMode.Open)))
            {
                using (FastFileBinaryWriter writer = new FastFileBinaryWriter(File.Open(outputPath, FileMode.Create)))
                {
                    FastFileWrapper fastFile = new FastFileWrapper();

                    fastFile.Header = reader.ReadFastFileHeader();
                    fastFile.FastFile = reader.ReadFastFileBody();

                    ChunkHandler.Dechunk(reader, writer);
                }
            }
        }

        public void DecompressFd(string inputPath, string outputPath)
        {
            using (FastFileBinaryReader reader = new FastFileBinaryReader(File.Open(inputPath, FileMode.Open)))
            {
                using (FastFileBinaryWriter writer = new FastFileBinaryWriter(File.Open(outputPath, FileMode.Create)))
                {
                    IndirectFastFileWrapper indirectFastFile = reader.ReadIndirectFastFile();

                    indirectFastFile.FastFileWrapper = new FastFileWrapper();
                    indirectFastFile.FastFileWrapper.Header = reader.ReadFastFileHeader();
                    indirectFastFile.FastFileWrapper.FastFile = reader.ReadIndirectFastFileBody();

                    ChunkHandler.Dechunk(reader, writer);
                }
            }
        }

        public void Compress(string inputPath, string outputPath)
        {
            FastFileWrapper fastFile = new FastFileWrapper();

            using (FastFileBinaryReader reader = new FastFileBinaryReader(File.Open(outputPath, FileMode.Open)))
            {
                fastFile.Header = reader.ReadFastFileHeader();
                fastFile.FastFile = reader.ReadFastFileBody();
            }

            using (FastFileBinaryReader reader = new FastFileBinaryReader(File.Open(inputPath, FileMode.Open)))
            {
                using (FastFileBinaryWriter writer = new FastFileBinaryWriter(File.Open(outputPath + "_tmp", FileMode.Create)))
                {
                    writer.Write(fastFile.Header);
                    writer.Write(fastFile.FastFile);

                    ChunkHandler.Chunk(reader, writer);

                    fastFile.FastFile.UncompressedFileSize = reader.BaseStream.Position;
                    fastFile.FastFile.CompressedFileSize = writer.BaseStream.Position;

                    writer.BaseStream.Seek(36, SeekOrigin.Begin);
                    writer.Write(fastFile.FastFile);
                }
            }
        }
    }
}
