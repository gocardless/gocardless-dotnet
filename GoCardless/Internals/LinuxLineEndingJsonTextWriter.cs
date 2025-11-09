using System.IO;
using Newtonsoft.Json;

namespace GoCardless.Internals
{
    public class LinuxLineEndingJsonTextWriter : JsonTextWriter
    {
        public LinuxLineEndingJsonTextWriter(TextWriter textWriter)
            : base(textWriter) { }

        protected override void WriteIndent()
        {
            if (Formatting == Formatting.Indented)
            {
                WriteWhitespace("\n");
                int currentIndentCount = Top * Indentation;
                for (int i = 0; i < currentIndentCount; i++)
                    WriteIndentSpace();
            }
        }
    }
}
