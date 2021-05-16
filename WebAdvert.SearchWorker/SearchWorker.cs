using Amazon.Lambda.Core;
using System;
using Nest;

[assembly:LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace WebAdvert.SearchWorker
{
    public class SearchWorker
    {
        public void Process(Amazon.Lambda.SNSEvents.SNSEvent snsEvent, ILambdaContext lambdaContext) 
        {
            foreach (var snsEventRecord in snsEvent.Records)
            {
                lambdaContext.Logger.LogLine(snsEventRecord.Sns.Message);
            }
        }
    }
}
