namespace AzureKeyVaultExplorer.ViewModel
{
    using System;
    using System.Collections.ObjectModel;

    using AzureKeyVaultExplorer.Interface;
    using AzureKeyVaultExplorer.Model;

    using GalaSoft.MvvmLight;

    public class KeyVaultAccountViewModel : ViewModelBase
    {
        private readonly KeyVaultConfiguration keyVaultConfiguration;

        private readonly IKeyRepository keyRepository;

        private Key selectedKey;

        public KeyVaultAccountViewModel(KeyVaultConfiguration keyVaultConfiguration, IKeyRepository keyRepository)
        {
            if (keyVaultConfiguration == null)
            {
                throw new ArgumentNullException("keyVaultConfiguration");
            }

            this.keyVaultConfiguration = keyVaultConfiguration;
            this.keyRepository = keyRepository;
            this.ManageVaultKeysViewModel = new ManageVaultKeysViewModel(keyVaultConfiguration.VaultName, keyRepository, new KeyVaultKeyRepository(keyVaultConfiguration));
            this.ManageLocalKeysViewModel = new ManageLocalKeysViewModel(keyVaultConfiguration.VaultName, keyRepository);
            this.ManageLocalKeysViewModel.KeysModified += this.HandleKeysModified;
            this.ManageVaultKeysViewModel.KeysModified += this.HandleKeysModified;
            this.KeyCryptographicOperationsViewModel = new KeyCryptographicOperationsViewModel(new KeyOperations(keyVaultConfiguration));
        }

        public ObservableCollection<Key> AllKeys
        {
            get
            {
                return new ObservableCollection<Key>(this.keyRepository.GetAll().Result);
            }
        }

        public ManageLocalKeysViewModel ManageLocalKeysViewModel { get; set; }

        public ManageVaultKeysViewModel ManageVaultKeysViewModel { get; set; }

        public KeyCryptographicOperationsViewModel KeyCryptographicOperationsViewModel { get; set; }

        public Key SelectedKey
        {
            get
            {
                return this.selectedKey;
            }

            set
            {
                this.selectedKey = value;
                this.RaisePropertyChanged(() => this.SelectedKey);
            }
        }

        private void HandleKeysModified(object sender, EventArgs e)
        {
            this.RaisePropertyChanged(() => this.AllKeys);
        }
    }
}