using System;
using System.Security.Cryptography;
using System.Text;
using marvel_api.Config;
using Microsoft.Extensions.Options;

namespace marvel_api.Auth
{
    public class CredentialsService : ICredentialsService
    {
        private static readonly DateTime _epochDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private readonly AuthConfigModel _authConfig;

        public CredentialsService(IOptions<AuthConfigModel> authConfigOptions)
        {
            _authConfig = authConfigOptions.Value;
        }
        
        public CredentialsModel GenerateCredentials()
        {
            var credentials = new CredentialsModel {
                PublicKey =  _authConfig.PublicKey,
                TimeStamp = GenerateTimeStamp()
            };

            credentials.Hash = GenerateHash(credentials.TimeStamp);

            return credentials;
        }

        private string GenerateTimeStamp()
        {
            var epochDiff = DateTime.UtcNow - _epochDate;

            return Math.Ceiling(epochDiff.TotalMilliseconds).ToString();
        }

        private string GenerateHash(string timeStamp)
        {
            var key = timeStamp + _authConfig.PrivateKey + _authConfig.PublicKey;

            var encoder = System.Text.Encoding.ASCII.GetEncoder();

            byte[] keyBytes = new byte[key.Length];
            encoder.GetBytes(key.ToCharArray(), 0, key.Length, keyBytes, 0, true);

            StringBuilder hashBuilder = new StringBuilder();

            using(var md5Genrator = MD5.Create())
            {
                var md5Bytes = md5Genrator.ComputeHash(keyBytes);

                for (int i = 0; i < md5Bytes.Length; i++)
                {
                    hashBuilder.Append(md5Bytes[i].ToString("x2").ToLower());
                }
            }

            return hashBuilder.ToString();
        }
    }
}