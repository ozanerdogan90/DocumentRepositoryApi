using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace DocumentRepositoryApi.IntegrationTests.Helpers.Models
{
    public static class FormDataHelper
    {
        public static MultipartFormDataContent FormData
        {
            get
            {
                var file = File.OpenRead(@"Helpers\Contents\logo.png");
                var content = new StreamContent(file);
                var formData = new MultipartFormDataContent();
                formData.Add(content, "file", "logo.png");
                return formData;
            }
        }
    }
}
