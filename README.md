# Durable Functions in a Private environment

## Useful links

Grant the system-assigned identity access to the storage account

https://learn.microsoft.com/en-us/azure/azure-functions/functions-identity-based-connections-tutorial#grant-the-system-assigned-identity-access-to-the-storage-account

https://learn.microsoft.com/en-us/azure/azure-functions/functions-reference?tabs=blob&pivots=programming-language-csharp#connecting-to-host-storage-with-an-identity

https://learn.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-configure-managed-identity

## Disclaimer

This sample scripts are not supported under any Microsoft standard support program or service. The sample script is provided AS IS without warranty of any kind. Microsoft further disclaims all implied warranties including, without limitation, any implied warranties of merchantability or of fitness for a particular purpose. The entire risk arising out of the use or performance of the sample scripts and documentation remains with you. In no event shall Microsoft, its authors, or anyone else involved in the creation, production, or delivery of the scripts be liable for any damages whatsoever (including, without limitation, damages for loss of business profits, business interruption, loss of business information, or other pecuniary loss) arising out of the use of or inability to use the sample scripts or documentation, even if Microsoft has been advised of the possibility of such damages.

## Deploy the infrastructure

```bash
az login --use-device-code
```

```bash
cd terraform && terraform init
```

```bash
export ARM_SUBSCRIPTION_ID=$(az account show --query id -o tsv)
```

```bash
terraform plan -out plan.out
```

Then apply the changes:

```bash
terraform apply plan.out
```

## Publish the function app

From the VM using the bastion, you can clone the repo and publish the function app:

```bash
func azure functionapp publish <function_app_name> --dotnet-isolated
```