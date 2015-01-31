namespace AzureKeyVaultExplorer.Model
{
    public class KeyVaultConfiguration
    {
        public string AzureKeyVaultUrl { get; set; }

        public string ADApplicationClientId { get; set; }

        public string ADApplicationSecret { get; set; }
    }
}