﻿using System.Runtime.InteropServices;

namespace FastFilesCompressor
{
    public static class LZ4Handler
    {
        [DllImport("lz4.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int LZ4_compress(byte[] source, byte[] dest, int sourceSize);

        [DllImport("lz4.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int LZ4_decompress_safe(byte[] source, byte[] dest, int compressedSize, int maxDecompressedSize);
    }
}
