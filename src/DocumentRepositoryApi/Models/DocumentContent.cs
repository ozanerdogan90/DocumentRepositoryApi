using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentRepositoryApi.Models
{
    public class DocumentContent
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }
}
