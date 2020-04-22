using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AzureAssignment
{
    public class KeyVaultManager
    {
        private static KeyVaultClient keyVaultClient = null;
        private static string Uri = "https://dhartikeyvault.vault.azure.net/secrets/";

        public string GetValueFromAzureVault(string secret)
        {
            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            string value = string.Empty;
            try
            {
                keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
                var taskGetSecret = GetSecretAsync(Uri, secret);
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
            try
            {
                var vaultSecret = await keyVaultClient.GetSecretAsync(vaultBaseURL + secret).ConfigureAwait(false);
                var secretValue = vaultSecret.Value;
                return secretValue;
            }
            catch(Exception e)
            {
                return "";
            }
        }
    }
}