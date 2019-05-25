using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace CustomVisionGlobalAI.Training
{
    class Program
    {
        private const string endPoint = "https://southcentralus.api.cognitive.microsoft.com";
        private const string trainingKey = "42c6daefe89d42a6b09b8b8235e0d2f2";

        static void Main(string[] args)
        {
            CustomVisionTrainingClient trainingApi = new CustomVisionTrainingClient()
            {
                ApiKey = trainingKey,
                Endpoint = endPoint
            };

            Project project = trainingApi.GetProject(new Guid("80b63914-a25c-4bcb-a483-4c8e261357a0"));

            IDictionary<string, string> dadosImagens = JsonConvert.DeserializeObject<IDictionary<string, string>>(File.ReadAllText("imagens.json"));

            foreach (var chave in dadosImagens.Keys)
            {
                var tag = trainingApi.CreateTag(project.Id, chave);
                var imagens = Directory.GetFiles(dadosImagens[chave]);

                var imageFiles = imagens.Select(img => new ImageFileCreateEntry(Path.GetFileName(img), File.ReadAllBytes(img))).ToList();
                trainingApi.CreateImagesFromFiles(project.Id, new ImageFileCreateBatch(imageFiles, new List<Guid>() { tag.Id }));
            }

            var iteration = trainingApi.TrainProject(project.Id);


            while (iteration.Status == "Training")
            {
                Thread.Sleep(1000);
                iteration = trainingApi.GetIteration(project.Id, iteration.Id);
            }

            var publishedModelName = "treeClassModel";
            var predictionResourceId = "/subscriptions/6539021a-2e2a-4733-b025-cc48a0397c9e/resourceGroups/GlobalAI/providers/Microsoft.CognitiveServices/accounts/GAI_Prediction";
            trainingApi.PublishIteration(project.Id, iteration.Id, publishedModelName, predictionResourceId);
        }
    }
}
