using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PoojagWebapplication
{
       public class Program
        {
            private static KeyVaultClient keyVaultClient = null;
            static void Main(string[] args)
            {
                string secretName = "StorageConnectionString";
               //var kvUri = "https://Training1vault.vault.azure.net/secrets/";
            var kvUri = "https://poojagkeyvault.vault.azure.net/secrets/Connetionstring/607972307761425c953e5ae5a35a60f3";
            Console.WriteLine(GetValueFromAzureVault(kvUri, secretName));

                System.Threading.Thread.Sleep(5000);
                Console.WriteLine(" done.");

            }
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
