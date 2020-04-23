using Azure.AI.TextAnalytics;
using System;
using System.IO;

namespace TextAnalytics
{
    class Program
    {
        private static readonly TextAnalyticsApiKeyCredential credentials = new TextAnalyticsApiKeyCredential("42881d0f375141d785ec98fda4a6a633");
        private static readonly Uri endpoint = new Uri("https://eastus.api.cognitive.microsoft.com/");
        static void Main(string[] args)
        {
            var client = new TextAnalyticsClient(endpoint, credentials);
            Console.WriteLine("This aplication will do text analysis of all text files inside the provided folder path");
            Console.Write("Enter Folder path(i.e. D:\\Files): ");
            string path = Console.ReadLine();
            DirectoryInfo d = new DirectoryInfo(@path);
            FileInfo[] Files = d.GetFiles("*.txt");
            foreach (FileInfo file in Files)
            {
                using (StreamReader streamReader = File.OpenText(file.FullName))
                {
                    string text = streamReader.ReadToEnd();
                    Console.WriteLine("File Name: " + file.Name + "\n");
                    LanguageDetectionExample(client, text);
                    KeyPhraseExtractionExample(client, text);
                }
            }
            Console.Write("Press any key to exit.");
            Console.ReadKey();
        }
        static void LanguageDetectionExample(TextAnalyticsClient client, string text)
        {
            DetectedLanguage detectedLanguage = client.DetectLanguage(text);
            Console.WriteLine("Language:");
            Console.WriteLine($"\t{detectedLanguage.Name},\tISO-6391: {detectedLanguage.Iso6391Name}\n");
        }
        static void KeyPhraseExtractionExample(TextAnalyticsClient client, string text)
        {
            var response = client.ExtractKeyPhrases(text);
            Console.WriteLine("Key phrases:");

            foreach (string keyphrase in response.Value)
            {
                Console.WriteLine($"\t{keyphrase}\n");
            }
        }
    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Globalization;
//using System.IO;
//using Azure.AI.TextAnalytics;


//namespace TextAnalytics
//{
//    class Program
//    {
//        private static readonly AzureKeyCredential credentials = new AzureKeyCredential("<replace-with-your-text-analytics-key-here>");
//        private static readonly Uri endpoint = new Uri("<replace-with-your-text-analytics-endpoint-here>");

//            static void Main(string[] args)
//            {
//                var client = new TextAnalyticsClient(endpoint, credentials);

//                LanguageDetectionExample(client);
//                KeyPhraseExtractionExample(client);

//                Console.Write("Press any key to exit.");
//                Console.ReadKey();
//            }

//    }
//  static void LanguageDetectionExample(TextAnalyticsClient client)
//    {
//        DetectedLanguage detectedLanguage = client.DetectLanguage("Ce document est rédigé en Français.");
//        Console.WriteLine("Language:");
//        Console.WriteLine($"\t{detectedLanguage.Name},\tISO-6391: {detectedLanguage.Iso6391Name}\n");
//    }
//    static void KeyPhraseExtractionExample(TextAnalyticsClient client)
//    {
//        var response = client.ExtractKeyPhrases("My cat might need to see a veterinarian.");

//        // Printing key phrases
//        Console.WriteLine("Key phrases:");

//        foreach (string keyphrase in response.Value)
//        {
//            Console.WriteLine($"\t{keyphrase}");
//        }
//    }
//}
