using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using SellerManagement.Functions.Models;

namespace SellerManagement.Functions.Triggers;

public static class PolicyWorkFlowHandler
{
    public const string FunctionName = "PolicyWorkFlow";

    [FunctionName(FunctionName)]
    public static async Task RunPolicyWorkFlowAsync(
        [OrchestrationTrigger] IDurableOrchestrationContext context)
    {
        var policyWorkFlowRequest = context.GetInput<PolicyWorkFlowRequest>();

        var request = new SendPolicyInformationMailRequest(policyWorkFlowRequest.Email, policyWorkFlowRequest.PolicyNumber);
        await context.CallActivityAsync(SendPolicyInformationMailHandler.FunctionName, request);
        await context.CallActivityAsync(ChangePolicyStatusHandler.FunctionName, new ChangePolicyStatusRequest(policyWorkFlowRequest.Email, PolicyStatus.SellerInformed));

        var accepted = await context.WaitForExternalEvent<bool>(AppConstants.PolicyAcceptanceEventName, TimeSpan.FromHours(1));
        var policyStatus = accepted ? PolicyStatus.Accepted : PolicyStatus.Rejected;
        var changePolicyStatusRequest = new ChangePolicyStatusRequest(policyWorkFlowRequest.Email, policyStatus);
        await context.CallActivityAsync(ChangePolicyStatusHandler.FunctionName, changePolicyStatusRequest);
    }
}

public record PolicyWorkFlowRequest(string Email, string PolicyNumber);