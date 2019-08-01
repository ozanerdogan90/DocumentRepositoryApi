using DocumentRepositoryApi.Models;

namespace DocumentRepositoryApi.IntegrationTests.Helpers.Models
{
    public static class DocumentHelper
    {
        public static Document ValidDocument
        {
            get
            {
                return new Document()
                {
                    Description = "integration test document",
                    DocumentName = "doc1",
                    Title = "title1",
                    Version = "1.0"
                };
            }
        }
    }
}
