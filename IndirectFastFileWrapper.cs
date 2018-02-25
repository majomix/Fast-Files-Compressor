namespace FastFilesCompressor
{
    public class IndirectFastFileWrapper
    {
        public FastFileHeader Header { get; set; }
        public byte[] Indirection { get; set; }
        public FastFileWrapper FastFileWrapper { get; set; }
    }
}
