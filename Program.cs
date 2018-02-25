using NDesk.Options;
using System;
using System.IO;

namespace FastFilesCompressor
{
    public class Program
    {
        static void Main(string[] args)
        {
            bool export = true;
            string directory = Directory.GetCurrentDirectory();

            OptionSet options = new OptionSet()
            .Add("import", value => export = false)
            .Add("dir=", value => directory = value);

            options.Parse(Environment.GetCommandLineArgs());

            FastFileCompressor compressor = new FastFileCompressor();
            compressor.Decompress(@"C:\eng_code_post_gfx.ff", @"C:\eng_code_post_gfx.dump");
        }
    }
}
