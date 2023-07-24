# Seller Management

This project was created as a sample project to gain experience on the Azure Functions tool. It contains examples of different trigger scenarios such as HttpTrigger, CosmosDbTrigger, ServiceBusTrigger and IDurableContextTrigger.

An example is created through a scenario about the demand to be a seller on a platform. 

1- The user creates a record. (HttpTrigger)

2- The message that a seller is created in the system is raised. (CosmosDbTrigger -> ServiceBus). (CosmosDbTrigger -> ServiceBus)

3- An e-mail is sent to the seller about the rules and its approval is awaited. (ServiceBusTrigger -> DurableClient)

4- The seller approves or rejects. (HttpTrigger -> DurableClient)

5- Depending on the response of the person, the status of the seller object changes. (OrchestrationTrigger)

## __RUN__

You can easily create the required components in your own azure account by using the bicep file located under the infra file. If you have [Azure Account](https://marketplace.visualstudio.com/items?itemName=ms-vscode.azure-account) and [Bicep](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-bicep) extensions on VsCode, you can easily right click on the file and say publish and create all components.

After this process, you can get a local settings file by getting rid of the template extension at the end of the local.settings.json.template file. When you place the connection strings for the storage-account, cosmos-db and service-bus you created into this file, the application will become executable in the local environment.
