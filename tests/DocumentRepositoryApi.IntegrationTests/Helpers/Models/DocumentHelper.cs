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
                    Name = "doc1",
                    Title = "title1",
                    Version = "1.0"
                };
            }
        }

        public static Document ValidDocumentUpdate
        {
            get
            {
                return new Document()
                {
                    Description = "new file",
                    Name = "new document",
                    Title = "new title",
                    Version = "1.1"
                };
            }
        }
    }
}
