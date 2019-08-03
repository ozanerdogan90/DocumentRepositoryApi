using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using System;

namespace DocumentRepositoryApi.Services.Helpers
{
    public interface IEncryptionService
    {
        string Encrypt(string input);
        byte[] Encrypt(byte[] input);
        string Decrypt(string cipherText);
        byte[] Decrypt(byte[] cipherText);
    }

    public class EncryptionService : IEncryptionService
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;

        private readonly string _key;
        public EncryptionService(IDataProtectionProvider dataProtectionProvider, IConfiguration config)
        {
            _dataProtectionProvider = dataProtectionProvider;
            _key = config["Encryption:Key"] ?? throw new ArgumentNullException("EncryptionKey");
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
