using System;
using System.Collections.Generic;

namespace DocumentRepositoryApi.DataAccess.Entities
{
    public class Document : BaseEntity
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public string Version { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
