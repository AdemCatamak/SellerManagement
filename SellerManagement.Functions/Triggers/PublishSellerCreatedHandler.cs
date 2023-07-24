using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using SellerManagement.Functions.Models;

namespace SellerManagement.Functions.Triggers;

public static class PublishSellerCreatedHandler
{
    public const string FunctionName = "PublishSellerCreated";

    [FunctionName(FunctionName)]
    public static async Task PublishSellerCreatedAsync(
        [CosmosDBTrigger(
            databaseName: AppConstants.CosmosDbName,
            containerName: AppConstants.SellerContainerName,
            Connection = AppConstants.CosmosDbConnectionStrConfigName,
            LeaseContainerName = AppConstants.SellerLeasesContainerName,
            MaxItemsPerInvocation = 20,
            CreateLeaseContainerIfNotExists = true)]
        IReadOnlyList<SellerModel> sellers,
        [ServiceBus(queueOrTopicName: AppConstants.SellerCreated_InformSellerAboutPolicyQueue,
            Connection = AppConstants.ServiceBusConnectionStrConfigName)]
        ServiceBusSender serviceBusSender)
    {
        var sellerCreatedMessageList = sellers.Where(s => !s.UpdatedOn.HasValue)
            .Select(s => new SellerCreatedEvent { Email = s.Email })
            .Select(e => new ServiceBusMessage(JsonConvert.SerializeObject(e)) { MessageId = e.Email })
            .ToList();

        if (sellerCreatedMessageList.Any())
        {
            await serviceBusSender.SendMessagesAsync(sellerCreatedMessageList);
        }
    }
}