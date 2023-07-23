namespace SellerManagement.Functions;

public static class AppConstants
{
    public const string CosmosDbName = "SellerDb";
    public const string SellerContainerName = "Sellers";
    public const string SellerLeasesContainerName = "SellerLeases";
    public const string CosmosDbConnectionStrConfigName = "CosmosDb";
    
    public const string SellerCreated_InformSellerAboutPolicyQueue = "SellerCreated-InformSellerAboutPolicyQueue";
    public const string ServiceBusConnectionStrConfigName = "ServiceBus";
    
    public const string PolicyAcceptanceEventName = "PolicyAcceptance";
}