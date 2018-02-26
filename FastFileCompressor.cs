using System;
using System.IO;

namespace FastFilesCompressor
{
    public class FastFileCompressor
    {
        public void DecompressFastFile(string inputPath, string outputPath)
        {
            using (FastFileBinaryReader reader = new FastFileBinaryReader(File.Open(inputPath, FileMode.Open)))
            {
                using (FastFileBinaryWriter writer = new FastFileBinaryWriter(File.Open(outputPath, FileMode.Create)))
                {
                    FastFileWrapper fastFile = new FastFileWrapper();

                    fastFile.Header = reader.ReadFastFileHeader();
                    fastFile.FastFile = reader.ReadFastFile();

                    ChunkHandler.Dechunk(reader, writer);
                }
            }
        }

        public void DecompressDeltaFastFile(string inputPath, string outputPath)
        {
            using (FastFileBinaryReader reader = new FastFileBinaryReader(File.Open(inputPath, FileMode.Open)))
            {
                using (FastFileBinaryWriter writer = new FastFileBinaryWriter(File.Open(outputPath, FileMode.Create)))
                {
                    ReadDeltaFastFileWrapper(reader);
                    ChunkHandler.Dechunk(reader, writer);
                }
            }
        }

        public void CompressFastFile(string inputPath, string outputPath)
        {
            FastFileWrapper fastFile = new FastFileWrapper();

            using (FastFileBinaryReader reader = new FastFileBinaryReader(File.Open(outputPath, FileMode.Open)))
            {
                fastFile.Header = reader.ReadFastFileHeader();
                fastFile.FastFile = reader.ReadFastFile();
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

        public void CompressDeltaFastFile(string inputPath, string outputPath)
        {
            DeltaFastFileWrapper deltaFastFile;

            using (FastFileBinaryReader reader = new FastFileBinaryReader(File.Open(outputPath, FileMode.Open)))
            {
                deltaFastFile = ReadDeltaFastFileWrapper(reader);
            }

            using (FastFileBinaryReader reader = new FastFileBinaryReader(File.Open(inputPath, FileMode.Open)))
            {
                using (FastFileBinaryWriter writer = new FastFileBinaryWriter(File.Open(outputPath, FileMode.Create)))
                {
                    deltaFastFile.FastFileWrapper.FastFile.CompressedFileSize = new FileInfo(Path.ChangeExtension(inputPath, "ff")).Length;
                    deltaFastFile.FastFileWrapper.FastFile.UncompressedFileSize = reader.BaseStream.Length;

                    writer.Write(deltaFastFile.Header);
                    writer.Write(deltaFastFile.Indirection);
                    writer.Write(deltaFastFile.FastFileWrapper.Header);
                    writer.Write((dynamic)deltaFastFile.FastFileWrapper.FastFile);

                    ChunkHandler.Chunk(reader, writer);
                }
            }
        }

        private DeltaFastFileWrapper ReadDeltaFastFileWrapper(FastFileBinaryReader reader)
        {
            DeltaFastFileWrapper deltaFastFile = reader.ReadDeltaFastFileWrapper();

            deltaFastFile.FastFileWrapper = new FastFileWrapper();
            deltaFastFile.FastFileWrapper.Header = reader.ReadFastFileHeader();
            deltaFastFile.FastFileWrapper.FastFile = reader.ReadDeltaFastFile();

            return deltaFastFile;
        }
    }
}
