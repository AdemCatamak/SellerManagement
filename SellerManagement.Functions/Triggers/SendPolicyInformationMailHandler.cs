using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace SellerManagement.Functions.Triggers;

public static class SendPolicyInformationMailHandler
{
    public const string FunctionName = "SendPolicyInformationMail";

    [FunctionName(FunctionName)]
    public static Task SendPolicyInformationMailAsync([ActivityTrigger] SendPolicyInformationMailRequest request)
    {
        Console.WriteLine("#{0} Policy mail sent to {1}", request.PolicyNumber, request.Email);

        return Task.CompletedTask;
    }
}

public record SendPolicyInformationMailRequest(string Email, string PolicyNumber);