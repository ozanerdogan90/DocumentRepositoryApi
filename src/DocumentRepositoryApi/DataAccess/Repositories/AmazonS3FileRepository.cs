using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using DocumentRepositoryApi.DataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DocumentRepositoryApi.DataAccess.Repositories
{
    public class AmazonS3FileRepository : IDocumentContentRepository
    {
        private readonly IAmazonS3 _client;
        private readonly string _bucketName;

        public AmazonS3FileRepository(IAmazonS3 client, IConfiguration config)
        {
            _client = client;
            _bucketName = config.GetValue<string>("AWS:BucketName");
        }

        public async Task<DocumentContent> Fetch(Guid id)
        {
            var request = new GetObjectRequest()
            {
                BucketName = _bucketName,
                Key = id.ToString()
            };

            using (var response = await _client.GetObjectAsync(request))
            {
                using (StreamReader reader = new StreamReader(response.ResponseStream))
                {
                    return JsonConvert.DeserializeObject<DocumentContent>(reader.ReadToEnd());
                }
            }
        }

        public async Task<bool> Store(DocumentContent content)
        {
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = content.DocumentId.ToString(),
                ContentBody = JsonConvert.SerializeObject(content),
            };

            request.Metadata.Add("x-amz-meta-title", content.Name);
            await _client.PutObjectAsync(request);
            return true;
        }


        public async Task RemoveAsync(Guid id)
        {
            var request = new DeleteObjectRequest()
            {
                BucketName = _bucketName,
                Key = id.ToString()
            };
            await _client.DeleteObjectAsync(request);
        }
    }
}
