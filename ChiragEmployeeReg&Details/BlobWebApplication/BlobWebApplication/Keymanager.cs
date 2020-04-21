
using BlobWebApplication;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace BlobWebApplication
{
    public class Keymanager
    {
        private static KeyVaultClient keyVaultClient = null;
        private static string kvUri;
        public Keymanager()
        {
            kvUri = ConfigurationManager.AppSettings[Constants.KeyVaultUri];
        }
        public string GetValueFromAzureVault(string secret)
        {
            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            string value = string.Empty;
            try
            {
                keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
                var taskGetSecret = GetSecretAsync(kvUri, secret);
                value = taskGetSecret.Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return value;
        }
        private static async Task<string> GetSecretAsync(string vaultBaseURL, string secret)
        {
            var vaultSecret = await keyVaultClient.GetSecretAsync(vaultBaseURL + secret).ConfigureAwait(false);
            var secretValue = vaultSecret.Value;
            return secretValue;
        }
    }
}