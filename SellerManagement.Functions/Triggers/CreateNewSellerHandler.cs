using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using SellerManagement.Functions.Models;

namespace SellerManagement.Functions.Triggers;

public static class CreateNewSellerHandler
{
    public const string FunctionName = "CreateNewSeller";

    [FunctionName(FunctionName)]
    public static async Task<IActionResult> CreateNewSellerAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
        CreateSellerRequest request,
        [CosmosDB(
            databaseName: AppConstants.CosmosDbName,
            containerName: AppConstants.SellerContainerName,
            Connection = AppConstants.CosmosDbConnectionStrConfigName)]
        IAsyncCollector<SellerModel> sellers,
        CancellationToken cancellationToken)
    {
        var sellerModel = SellerModel.Create(request.Email);
        await sellers.AddAsync(sellerModel, cancellationToken);

        return new StatusCodeResult(StatusCodes.Status201Created);
    }
}

public record CreateSellerRequest(string Email);