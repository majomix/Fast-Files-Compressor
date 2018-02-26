namespace FastFilesCompressor
{
    public class DeltaFastFileWrapper
    {
        public FastFileHeader Header { get; set; }
        public byte[] Indirection { get; set; }
        public FastFileWrapper FastFileWrapper { get; set; }
    }
}
