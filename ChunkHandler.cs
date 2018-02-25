using System;
using System.IO;

namespace FastFilesCompressor
{
    public static class ChunkHandler
    {
        public static Int32 Chunk(BinaryWriter writer)
        {
            return 0;
        }

        public static void Dechunk(FastFileBinaryReader reader, FastFileBinaryWriter writer)
        {
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                Int32 compressedChunkSize = reader.ReadInt32();
                Int32 uncompressedChunkSize = reader.ReadInt32();

                byte[] input = reader.ReadBytes(compressedChunkSize);
                byte[] output = new byte[uncompressedChunkSize];

                int result = LZ4Handler.LZ4_decompress_safe(input, output, input.Length, uncompressedChunkSize);

                if (result != uncompressedChunkSize)
                {
                    throw new ArgumentException();
                }

                writer.Write(output);

                // padding
                if (compressedChunkSize % 4 != 0)
                {
                    reader.ReadBytes(4 - compressedChunkSize % 4);
                }
            }
        }
    }
}
