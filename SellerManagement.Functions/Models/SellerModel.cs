using System;

namespace SellerManagement.Functions.Models;

public class SellerModel
{
    public string id { get; private set; }
    public string Email { get; private set; }
    public PolicyStatus PolicyStatus { get; private set; }
    public string? PolicyNumber { get; private set; }
    public DateTime? UpdatedOn { get; private set; }

    public SellerModel(string id, string email, PolicyStatus policyStatus, string? policyNumber, DateTime? updatedOn)
    {
        this.id = id;
        Email = email;
        PolicyStatus = policyStatus;
        PolicyNumber = policyNumber;
        UpdatedOn = updatedOn;
    }


    public void ChangePolicyStatus(PolicyStatus policyStatus)
    {
        PolicyStatus = policyStatus;
        UpdatedOn = DateTime.UtcNow;
    }

    public void SetPolicyNumber(string policyNumber)
    {
        PolicyStatus = PolicyStatus.Created;
        PolicyNumber = policyNumber;
        UpdatedOn = DateTime.UtcNow;
    }

    public static SellerModel Create(string email)
    {
        return new SellerModel(email, email, PolicyStatus.Noop, null, null);
    }
}