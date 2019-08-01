using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentRepositoryApi.Models
{
    public class NotEmptyGuidAttribute : ValidationAttribute
    {
        public NotEmptyGuidAttribute() : base("The property cannot be empty.")
        {
        }

        public override bool IsValid(object value)
        {
            var guid = value as Guid?;

            if (!guid.HasValue || guid.Value == Guid.Empty)
            {
                return false;
            }

            return true;
        }
    }
}
