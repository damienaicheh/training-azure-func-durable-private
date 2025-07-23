resource "azurerm_storage_account" "storage" {
  name                             = format("st%s", local.resource_suffix_lowercase)
  resource_group_name              = local.resource_group_name
  location                         = local.resource_group_location
  account_tier                     = "Standard"
  account_replication_type         = "LRS"
  tags                             = var.tags
  cross_tenant_replication_enabled = false
  allow_nested_items_to_be_public  = false
  default_to_oauth_authentication  = true
  public_network_access_enabled    = true
  network_rules {
    bypass                     = ["AzureServices"]
    default_action             = "Deny"
    ip_rules                   = [chomp(data.http.my_ip.response_body)]
    virtual_network_subnet_ids = []
  }
}

resource "azurerm_storage_queue" "hello_queue" {
  name                 = "hello-queue"
  storage_account_name = azurerm_storage_account.storage.name
  depends_on = [
    azurerm_role_assignment.user_storage_queue_data_owner,
    azurerm_role_assignment.user_reader,
  ]
}

data "http" "my_ip" {
  url = "https://ipv4.icanhazip.com"
}

output "public_ip" {
  value = chomp(data.http.my_ip.response_body)
}


resource "azurerm_storage_account" "this" {
  name                             = format("stfunc%s", local.resource_suffix_lowercase)
  resource_group_name              = local.resource_group_name
  location                         = local.resource_group_location
  account_tier                     = "Standard"
  account_replication_type         = "LRS"
  tags                             = var.tags
  cross_tenant_replication_enabled = false
  allow_nested_items_to_be_public  = false
  default_to_oauth_authentication  = true
  public_network_access_enabled    = true
  # network_rules {
  #   bypass                     = ["AzureServices"]
  #   default_action             = "Deny"
  #   ip_rules                   = [chomp(data.http.my_ip.response_body)]
  #   virtual_network_subnet_ids = []
  # }
}
