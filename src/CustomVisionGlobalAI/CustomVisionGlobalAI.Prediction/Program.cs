using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CustomVisionGlobalAI.Prediction
{
    class Program
    {
        private const string endPoint = "https://southcentralus.api.cognitive.microsoft.com";
        private const string predictionKey = "d002e41dfc0746e1b0aac9fe7bbd6567";


        static void Main(string[] args)
        {
            CustomVisionPredictionClient endpoint = new CustomVisionPredictionClient()
            {
                ApiKey = predictionKey,
                Endpoint = endPoint
            };

            IDictionary<string, string> dadosImagens = JsonConvert.DeserializeObject<IDictionary<string, string>>(File.ReadAllText("opcoes.json"));

            Console.WriteLine("Selecione a imagem de teste.");
            string opcao = Console.ReadLine();

            var imagemTeste = new MemoryStream(File.ReadAllBytes(dadosImagens[opcao]));

            var result = endpoint.ClassifyImage(new Guid("80b63914-a25c-4bcb-a483-4c8e261357a0"), "treeClassModel", imagemTeste);

            foreach (var c in result.Predictions)
            {
                Console.WriteLine($"\t{c.TagName}: {c.Probability:P1}");
            }
        }
    }
}
