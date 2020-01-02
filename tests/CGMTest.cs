using System.IO;
using System.Reflection;

namespace codessentials.CGM.Tests
{
    /// <summary>
    /// Base class for CGM tests
    /// </summary>
    abstract class CgmTest
    {
        protected BinaryCgmFile ReadBinaryFile(string resourceName)
        {
            return ReadBinaryFile(resourceName, this.GetType().Assembly);
        }

        protected BinaryCgmFile ReadBinaryFile(string resourceName, Assembly assembly)
        {
            if (!resourceName.StartsWith("codessentials.CGM.Tests.Files"))
                resourceName = $"codessentials.CGM.Tests.Files.{resourceName}";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            return new BinaryCgmFile(stream, resourceName);
        }

        protected byte[] GetResourceData(string resourceName)
        {
            return GetResourceData(resourceName, this.GetType().Assembly);
        }

        protected byte[] GetResourceData(string resourceName, Assembly assembly)
        {
            if (!resourceName.StartsWith("codessentials.CGM.Tests.Files"))
                resourceName = $"codessentials.CGM.Tests.Files.{resourceName}";

            using var ms = new MemoryStream();
            assembly.GetManifestResourceStream(resourceName).CopyTo(ms);
            return ms.ToArray();
        }

        protected string ConvertToClearText(BinaryCgmFile binaryFile)
        {
            var cleanTextFile = new ClearTextCgmFile(binaryFile);
            return cleanTextFile.GetContent();
        }
    }
}
