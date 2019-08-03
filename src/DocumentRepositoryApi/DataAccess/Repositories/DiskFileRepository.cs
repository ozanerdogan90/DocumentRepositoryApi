using DocumentRepositoryApi.DataAccess.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentRepositoryApi.DataAccess.Repositories
{
    public class DiskFileRepository : IDocumentContentRepository
    {
        private readonly string _repositoryFolder;
        private readonly int _parentRoothDepth = 3;

        public DiskFileRepository(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            var rootPath = GetRouteParentPath(hostingEnvironment.ContentRootPath);

            var defaultFileDirectory = Path.Combine(rootPath, "DocumentContents");
            _repositoryFolder = configuration.GetValue("DocumentApi:ContentPath", defaultFileDirectory);
            if (!Directory.Exists(_repositoryFolder))
            {
                Directory.CreateDirectory(_repositoryFolder);
            }
        }

        public async Task<DocumentContent> Fetch(Guid id)
        {
            var filePath = GetFilePath(id);

            if (System.IO.File.Exists(filePath))
            {
                return JsonConvert.DeserializeObject<DocumentContent>(await System.IO.File.ReadAllTextAsync(filePath));
            }
            else
            {
                throw new FileNotFoundException("A content with the specified identifier was not found in the repository.", filePath);
            }
        }

        public async Task RemoveAsync(Guid id)
        {
            System.IO.File.Delete(GetFilePath(id));
        }

        public async Task<bool> Store(DocumentContent content)
        {
            await System.IO.File.WriteAllTextAsync(GetFilePath(content.DocumentId), JsonConvert.SerializeObject(content));
            return true;
        }

        private string GetFilePath(Guid identifier)
        {
            return System.IO.Path.Combine(_repositoryFolder, identifier.ToString());
        }

        private string GetRouteParentPath(string path, int depth = 0)
        {
            if (depth >= _parentRoothDepth)
                return path;

            depth++;
            var parent = Directory.GetParent(path);
            if (parent != null)
                return GetRouteParentPath(parent.FullName, depth++);

            return path;
        }
    }
}
