namespace AzureKeyVaultExplorer.Model
{
    public class KeyVaultConfiguration
    {
        public string VaultName { get; set; }

        public string AzureKeyVaultUrl { get; set; }

        public string ADApplicationClientId { get; set; }

        public string ADApplicationSecret { get; set; }
    }
}