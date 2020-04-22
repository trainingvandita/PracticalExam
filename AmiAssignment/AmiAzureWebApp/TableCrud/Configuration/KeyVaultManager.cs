using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TableCrud.Configuration
{
    public class KeyVaultManager
    {
        private static KeyVaultClient keyVaultClient = null;
        
        //static void Main(string[] args)
        //{
        //    
        //    var kvUri = "https://training1vault.vault.azure.net/secrets/";
        //    Console.WriteLine(GetValueFromAzureVault(kvUri, secretName));
        //    var StroageString = GetValueFromAzureVault(kvUri, secretName);
        //    System.Threading.Thread.Sleep(5000);
        //    Console.WriteLine(" done.");

        //}
        public static string GetValueFromAzureVault(string vaultBaseURL, string secret)
        {
            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            string value = string.Empty;
            try
            {
                keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
                var taskGetSecret = GetSecretAsync(vaultBaseURL, secret);
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