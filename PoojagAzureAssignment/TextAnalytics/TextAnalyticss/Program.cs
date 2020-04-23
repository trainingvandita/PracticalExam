using System;
using Azure;
using System.Globalization;
using Azure.AI.TextAnalytics;
using System.IO;

namespace TextAnalyticss
{
    class Program
    {
        
        private static readonly AzureKeyCredential credentials = new AzureKeyCredential("5cd7dadfe569456985301d28d3140dfc");
        private static readonly Uri endpoint = new Uri("https://poojagtextanalytics.cognitiveservices.azure.com/");
        static void Main(string[] args)
        {
            var client = new TextAnalyticsClient(endpoint, credentials);
            Console.WriteLine("enter Path");
            var path = Console.ReadLine();
            StreamReader reader = File.OpenText(path);
            string text = reader.ReadToEnd();

           // You will implement these methods later in the quickstart.

            LanguageDetectionExample(client,text);

            KeyPhraseExtractionExample(client,text);

            Console.Write("Press any key to exit.");
            Console.ReadKey();

        }
        static void LanguageDetectionExample(TextAnalyticsClient client,string Text)
        {
            DetectedLanguage detectedLanguage = client.DetectLanguage(Text);
            Console.WriteLine("Language:");
            Console.WriteLine($"\t{detectedLanguage.Name},\tISO-6391: {detectedLanguage.Iso6391Name}\n");




        }
        static void KeyPhraseExtractionExample(TextAnalyticsClient client,string Text)
        {
            var response = client.ExtractKeyPhrases(Text);

            // Printing key phrases
            Console.WriteLine("Key phrases:");

            foreach (string keyphrase in response.Value)
            {
                Console.WriteLine($"\t{keyphrase}");
            }
        }




    }
}
