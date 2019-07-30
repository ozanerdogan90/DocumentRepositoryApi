using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentRepositoryApi.Models
{
    public class Document
    {
        [Required(AllowEmptyStrings = false)]
        public string DocumentName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Description { get; set; }
        public string Version { get; set; }
    }
}
