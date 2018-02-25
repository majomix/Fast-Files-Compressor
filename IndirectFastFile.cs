namespace FastFilesCompressor
{
    public class IndirectFastFile
    {
        public FastFileHeader Header { get; set; }
        public byte[] Indirection { get; set; }
        public FastFile FastFile { get; set; }
    }
}
