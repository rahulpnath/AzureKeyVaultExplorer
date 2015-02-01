namespace AzureKeyVaultExplorer.Model
{
    using System;

    public class KeyVaultConfigurationChangedEventArgs : EventArgs
    {
        public KeyVaultConfigurationChangedEventArgs(KeyVaultConfiguration keyVaultConfiguration)
        {
            this.KeyVaultConfiguration = keyVaultConfiguration;
        }

        public KeyVaultConfiguration KeyVaultConfiguration { get; set; }
    }
}