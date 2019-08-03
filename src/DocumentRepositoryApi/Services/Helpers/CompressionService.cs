using Microsoft.AspNetCore.Http;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace DocumentRepositoryApi.Services.Helpers
{
    public interface ICompressionService
    {
        byte[] Decompress(byte[] bytes);
        byte[] Compress(IFormFile fileToCompress);
        byte[] Compress(byte[] src);
    }

    public class CompressionService : ICompressionService
    {
        public byte[] Compress(IFormFile fileToCompress)
        {
            int count = 0;
            int offset = 0;
            byte[] content = new byte[(int)fileToCompress.Length];

            var openStream = fileToCompress.OpenReadStream();
            while ((count = openStream.Read(content, offset, content.Length - offset)) > 0)
            {
                offset += count;
            }

            return CompressContent(content);
        }

        private byte[] CompressContent(byte[] content)
        {
            using (var input = new MemoryStream(content))
            using (var output = new MemoryStream())
            {
                using (var compressor = new GZipStream(output, CompressionMode.Compress))
                {
                    CopyTo(input, compressor, input.Length);
                }

                return output.ToArray();
            }
        }

        public byte[] Compress(byte[] src)
        {
            return CompressContent(src);
        }

        private void CopyTo(Stream src, Stream dest, long length)
        {
            byte[] bytes = new byte[length];
            int cnt;
            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        public byte[] Decompress(byte[] bytes)
        {
            using (var input = new MemoryStream(bytes))
            using (var output = new MemoryStream())
            {
                using (var compressor = new GZipStream(input, CompressionMode.Decompress))
                {
                    CopyTo(compressor, output, bytes.Length);
                }

                return output.ToArray();
            }
        }
    }
}
