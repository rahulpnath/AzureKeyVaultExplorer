namespace AzureKeyVaultExplorer.Model
{
    using System;

    public class KeyAddedEventArgs : EventArgs
    {
        public KeyAddedEventArgs(Key key)
        {
            this.Key = key;
        }

        public Key Key { get; set; }
    }
}