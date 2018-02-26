using System;
using System.IO;

namespace FastFilesCompressor
{
    public static class ChunkHandler
    {
        public static void Chunk(FastFileBinaryReader reader, FastFileBinaryWriter writer)
        {
            int maximumChunkSize = 65536;

            for (int currentPosition = 0; currentPosition < reader.BaseStream.Length; currentPosition += maximumChunkSize)
            {
                int uncompressedChunkSize = (int)reader.BaseStream.Length - currentPosition;
                if (uncompressedChunkSize > maximumChunkSize)
                {
                    uncompressedChunkSize = maximumChunkSize;
                }

                byte[] inputBuffer = reader.ReadBytes((int)maximumChunkSize);
                byte[] outputBuffer = new byte[maximumChunkSize * 2];

                int compressedChunkSize = LZ4Handler.LZ4_compress(inputBuffer, outputBuffer, inputBuffer.Length);
                writer.Write(compressedChunkSize);
                writer.Write((int)uncompressedChunkSize);
                writer.Write(outputBuffer, 0, compressedChunkSize);

                if (compressedChunkSize % 4 != 0)
                {
                    writer.Write(new byte[4 - compressedChunkSize % 4]);
                }
            }
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
