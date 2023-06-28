using Microsoft.AspNetCore.Http;
using shared.bll.Validators.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace shared.bll.Validators.Implementations
{
    public class FileHeaderValidator : IValidator
    {
        private static readonly Dictionary<string, List<byte[]>> _fileSignature = new Dictionary<string, List<byte[]>>
        {
            { ".png", new List<byte[]> { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } } },
            { ".jpeg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                }
            },
            { ".jpg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
                }
            },
        };

        private readonly IFormFile formFile;

        public FileHeaderValidator(IFormFile formFile)
        {
            this.formFile = formFile;
        }

        public bool Validate()
        {
            if (string.IsNullOrEmpty(formFile.FileName))
            {
                return false;
            }
            var ext = Path.GetExtension(formFile.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext))
            {
                return false;
            }
            using var memoryStream = new MemoryStream();
            formFile.CopyTo(memoryStream);
            memoryStream.Position = 0;
            if (ext == ".svg")
            {
                using var reader = new StreamReader(memoryStream);
                var xmlDocument = new XmlDocument();
                try
                {
                    xmlDocument.LoadXml(reader.ReadToEnd());
                    return true;
                }
                catch (XmlException)
                {
                    return false;
                }
            }

            var signatures = _fileSignature[ext];
            using var binaryReader = new BinaryReader(memoryStream);
            var headerBytes = binaryReader.ReadBytes(signatures.Max(m => m.Length));

            return signatures.Any(signature =>
                headerBytes.Take(signature.Length).SequenceEqual(signature));
        }
    }
}
