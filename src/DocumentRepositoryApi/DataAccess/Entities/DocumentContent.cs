using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentRepositoryApi.DataAccess.Entities
{
    public class DocumentContent
    {
        public Guid DocumentId { get; set; }
        public string Name { get; set; }
        public int Length { get; set; }
        public string Type { get; set; }
        public byte[] Content { get; set; }
    }
}
