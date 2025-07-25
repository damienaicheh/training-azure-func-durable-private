Grant the system-assigned identity access to the storage account

https://learn.microsoft.com/en-us/azure/azure-functions/functions-identity-based-connections-tutorial#grant-the-system-assigned-identity-access-to-the-storage-account


Publish the function app

```bash
func azure functionapp publish <function_app_name> --dotnet-isolated
```

https://learn.microsoft.com/en-us/azure/azure-functions/functions-reference?tabs=blob&pivots=programming-language-csharp#connecting-to-host-storage-with-an-identity

https://learn.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-configure-managed-identity