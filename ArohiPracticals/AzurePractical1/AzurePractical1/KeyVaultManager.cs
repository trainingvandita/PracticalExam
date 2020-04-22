using AzurePractical1.Common;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Constants = AzurePractical1.Common.Constants;

namespace AzurePractical1
{
    public class KeyVaultManager
    {
        private static KeyVaultClient keyVaultClient = null;
        private static string kvUri = "https://askeyvault28.vault.azure.net/secrets/";
        //public KeyVaultManager()
        //{
        //    kvUri = ConfigurationManager.AppSettings[Constants.KeyVaultUri];
        //}
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