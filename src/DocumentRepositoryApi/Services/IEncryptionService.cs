using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentRepositoryApi.Services
{
    public interface IEncryptionService
    {
        string Encrypt(string input);
        byte[] Encrypt(byte[] input);
        string Decrypt(string cipherText);
        byte[] Decrypt(byte[] cipherText);
    }
}
