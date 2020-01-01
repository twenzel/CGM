using codessentials.CGM.Export;
using System.IO;
using System.Text;

namespace codessentials.CGM
{
    /// <summary>
    /// Represents a CGM file in CleanText mode
    /// </summary>
    public class ClearTextCGMFile : CGMFile
    {
        private string _fileName;

        public ClearTextCGMFile()
        {
            ResetMetaDefinitions();
        }

        public ClearTextCGMFile(BinaryCGMFile binaryfile)
        {
            _fileName = binaryfile.FileName;
            ApplyValues(binaryfile);
        }

        /// <summary>
        /// Writes the CGM commands to the current file name
        /// </summary>
        public void WriteFile()
        {
            WriteFile(_fileName);
        }

        /// <summary>
        /// Writes the CGM commands to the given file name-
        /// </summary>
        /// <param name="fileName">The file name to write the content to.</param>
        public void WriteFile(string fileName)
        {
            using (var stream = File.Create(fileName))
            {
                WriteFile(stream);
            }
        }

        /// <summary>
        /// Writes the CGM commands to the given stream-
        /// </summary>
        /// <param name="stream">The stream to write the content to.</param>
        public void WriteFile(Stream stream)
        {
            ResetMetaDefinitions();
            using (var writer = new DefaultClearTextWriter(stream))
            {
                foreach (var command in _commands)
                    writer.WriteCommand(command);

                _messages.AddRange(writer.Messages);
            }
        }

        /// <summary>
        /// Gets the whole CGM as clear text.
        /// </summary>
        /// <returns></returns>
        public string GetContent()
        {
            using (var stream = new MemoryStream())
            {
                WriteFile(stream);

                return Encoding.Default.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// The original file name.
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
        }
    }
}
