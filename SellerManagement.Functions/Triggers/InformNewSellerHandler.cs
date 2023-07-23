using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using SellerManagement.Functions.Models;

namespace SellerManagement.Functions.Triggers;

public static class InformNewSellerHandler
{
    public const string FunctionName = "InformNewSeller";

    [FunctionName(FunctionName)]
    public static async Task InformNewSellerAsync(
        [ServiceBusTrigger(queueName: AppConstants.SellerCreated_InformSellerAboutPolicyQueue, Connection = AppConstants.ServiceBusConnectionStrConfigName)]
        SellerCreatedEvent sellerCreatedEvent,
        [DurableClient] IDurableClient durableTaskClient,
        [CosmosDB(Connection = AppConstants.CosmosDbConnectionStrConfigName)]
        CosmosClient cosmosClient,
        CancellationToken cancellationToken
    )
    {
        var policyNumber = Guid.NewGuid().ToString();

        var sellerContainer = cosmosClient.GetDatabase(AppConstants.CosmosDbName).GetContainer(AppConstants.SellerContainerName);
        var sellerModelResponse = await sellerContainer.GetItemLinqQueryable<SellerModel>()
            .Where(s => s.Email == sellerCreatedEvent.Email)
            .ToFeedIterator()
            .ReadNextAsync(cancellationToken);
        var sellerModel = sellerModelResponse.Resource.First();

        if (sellerModel.PolicyNumber is not null)
        {
            return;
        }

        // policy was created
        sellerModel.SetPolicyNumber(policyNumber);
        await sellerContainer.UpsertItemAsync(sellerModel, cancellationToken: cancellationToken);

        await durableTaskClient.StartNewAsync(PolicyWorkFlowHandler.FunctionName, policyNumber, new PolicyWorkFlowRequest(sellerModel.Email, sellerModel.PolicyNumber ?? throw new ArgumentNullException(nameof(SellerModel.PolicyNumber))));
    }
}