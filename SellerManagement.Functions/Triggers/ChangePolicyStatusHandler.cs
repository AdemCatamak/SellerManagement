using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using SellerManagement.Functions.Models;

namespace SellerManagement.Functions.Triggers;

public static class ChangePolicyStatusHandler
{
    public const string FunctionName = "ChangePolicyStatus";

    [FunctionName(FunctionName)]
    public static async Task ChangePolicyStatusAsync(
        [ActivityTrigger] ChangePolicyStatusRequest request,
        [CosmosDB(Connection = AppConstants.CosmosDbConnectionStrConfigName)]
        CosmosClient cosmosClient,
        CancellationToken cancellationToken)
    {
        var sellerContainer = cosmosClient.GetDatabase(AppConstants.CosmosDbName).GetContainer(AppConstants.SellerContainerName);
        var sellerModelResponse = await sellerContainer.GetItemLinqQueryable<SellerModel>()
            .Where(s => s.Email == request.Email)
            .ToFeedIterator()
            .ReadNextAsync(cancellationToken);
        var sellerModel = sellerModelResponse.Resource.First();

        sellerModel.ChangePolicyStatus(request.Status);
        await sellerContainer.UpsertItemAsync(sellerModel, cancellationToken: cancellationToken);
    }
}

public record ChangePolicyStatusRequest(string Email, PolicyStatus Status);