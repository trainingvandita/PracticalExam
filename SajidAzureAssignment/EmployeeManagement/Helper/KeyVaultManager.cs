using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EmployeeManagement.Helper
{
    public class KeyVaultManager
    {
        //public static async Task<string> GetConnectionStringAsync()
        //{
        //    string vaultUri = ConfigurationManager.AppSettings["VaultUri"];
        //    string secretName = ConfigurationManager.AppSettings["Secret"];

        //    AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
        //    KeyVaultClient keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
        //    var secrateValue = await keyVaultClient.GetSecretAsync(vaultUri + secretName).ConfigureAwait(false);           
        //    return "";
        //}
        private static KeyVaultClient keyVaultClient = null;
        private static string kvUri;
        public KeyVaultManager()
        {
            kvUri = ConfigurationManager.AppSettings["VaultUri"];
        }
        public string GetValueFromAzureVault()
        {
            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            string value = string.Empty;
            string secret = ConfigurationManager.AppSettings["Secret"];
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