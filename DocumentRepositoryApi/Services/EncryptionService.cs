using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentRepositoryApi.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;

        private readonly string _key;
        public EncryptionService(IDataProtectionProvider dataProtectionProvider, IConfiguration config)
        {
            _dataProtectionProvider = dataProtectionProvider;
            _key = config.GetValue<string>("Encryption:Key", "cxz92k13md8f981hu6y7alkc");
        }

        public string Encrypt(string input)
        {
            var protector = _dataProtectionProvider.CreateProtector(_key);
            return protector.Protect(input);
        }

        public byte[] Encrypt(byte[] input)
        {
            var protector = _dataProtectionProvider.CreateProtector(_key);
            return protector.Protect(input);
        }

        public string Decrypt(string cipherText)
        {
            var protector = _dataProtectionProvider.CreateProtector(_key);
            return protector.Unprotect(cipherText);
        }

        public byte[] Decrypt(byte[] cipherText)
        {
            var protector = _dataProtectionProvider.CreateProtector(_key);
            return protector.Unprotect(cipherText);
        }
    }
}
