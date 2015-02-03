namespace AzureKeyVaultExplorer.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using AzureKeyVaultExplorer.Interface;
    using AzureKeyVaultExplorer.Model;

    using GalaSoft.MvvmLight;

    public class KeyVaultAccountViewModel : ViewModelBase
    {
        private readonly KeyVaultConfiguration keyVaultConfiguration;

        private readonly IKeyRepository keyRepository;

        public KeyVaultAccountViewModel(KeyVaultConfiguration keyVaultConfiguration, IKeyRepository keyRepository)
        {
            if (keyVaultConfiguration == null)
            {
                throw new ArgumentNullException("keyVaultConfiguration");
            }

            this.keyVaultConfiguration = keyVaultConfiguration;
            this.keyRepository = keyRepository;
            this.ManageLocalKeysViewModel = new ManageLocalKeysViewModel(keyVaultConfiguration.VaultName, keyRepository);
            this.ManageLocalKeysViewModel.KeysModified += this.HandleKeysModified;
            this.KeyCryptographicOperationsViewModel = new KeyCryptographicOperationsViewModel(new KeyOperations(keyVaultConfiguration));
        }

        public ObservableCollection<Key> AllKeys
        {
            get
            {
                return new ObservableCollection<Key>(this.keyRepository.All);
            }
        }

        public ManageLocalKeysViewModel ManageLocalKeysViewModel { get; set; }

        public KeyCryptographicOperationsViewModel KeyCryptographicOperationsViewModel { get; set; }

        public Key SelectedKey { get; set; }

        private void HandleKeysModified(object sender, EventArgs e)
        {
            this.RaisePropertyChanged(() => this.AllKeys);
        }
    }
}