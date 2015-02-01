namespace AzureKeyVaultExplorer.Model
{
    using Microsoft.KeyVault.Client;

    public class Key
    {
        public Key(KeyBundle keyBundle, bool isLocal)
        {
            this.KeyBundle = keyBundle;
            this.IsLocal = isLocal;
        }

        public bool IsLocal { get; set; }

        public KeyBundle KeyBundle { get; set; }
    }
}