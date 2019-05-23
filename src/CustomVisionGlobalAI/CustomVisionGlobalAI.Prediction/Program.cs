using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using System;
using System.IO;

namespace CustomVisionGlobalAI.Prediction
{
    class Program
    {
        private const string endPoint = "https://southcentralus.api.cognitive.microsoft.com";
        private const string trainingKey = "bccc9e4e95be46b3ac373f185e936816";


        static void Main(string[] args)
        {
            CustomVisionPredictionClient endpoint = new CustomVisionPredictionClient()
            {
                ApiKey = trainingKey,
                Endpoint = endPoint
            };

            var imagemTeste = new MemoryStream(File.ReadAllBytes("D:\\Downloads\\Fabricio\\Projetos\\CustomVisionGlobalAI\\fotos\\teste\\Teste01.jpg"));

            var result = endpoint.ClassifyImage(new Guid("cec8fbf3-fb17-4f06-a9db-d1623dec02f4"), "treeClassModel", imagemTeste);

            foreach (var c in result.Predictions)
            {
                Console.WriteLine($"\t{c.TagName}: {c.Probability:P1}");
            }
        }
    }
}
