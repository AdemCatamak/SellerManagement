using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace SellerManagement.Functions.Triggers;

public static class ReplyPolicyAcceptanceFormHandler
{
    public const string FunctionName = "ReplyPolicyAcceptanceForm";

    [FunctionName(FunctionName)]
    public static async Task<IActionResult> ReplyPolicyAcceptanceFormAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
        ReplyPolicyAcceptanceFormRequest request,
        [DurableClient] IDurableClient durableTaskClient)
    {
        await durableTaskClient.RaiseEventAsync(request.PolicyNumber, AppConstants.PolicyAcceptanceEventName, request.Response);
        return new OkResult();
    }
}

public record ReplyPolicyAcceptanceFormRequest(string PolicyNumber, bool Response);