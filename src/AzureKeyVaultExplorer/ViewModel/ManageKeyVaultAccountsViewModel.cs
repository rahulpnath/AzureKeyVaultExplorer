namespace AzureKeyVaultExplorer.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    using AzureKeyVaultExplorer.Model;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    public class ManageKeyVaultAccountsViewModel : ViewModelBase
    {
        private AddKeyVaultAccountViewModel addKeyVaultAccountViewModel;

        public ManageKeyVaultAccountsViewModel()
        {
            this.KeyVaultConfigurations = new ObservableCollection<KeyVaultConfiguration>();
        }

        public AddKeyVaultAccountViewModel AddKeyVaultAccountViewModel
        {
            get
            {
                return this.addKeyVaultAccountViewModel;
            }

            set
            {
                this.addKeyVaultAccountViewModel = value;
                this.RaisePropertyChanged(() => this.AddKeyVaultAccountViewModel);
            }
        }

        public ObservableCollection<KeyVaultConfiguration> KeyVaultConfigurations { get; set; }

        public RelayCommand AddKeyVaultAccountCommand
        {
            get
            {
                return new RelayCommand(this.OpenAddKeyVaultAccount);
            }
        }

        private void OpenAddKeyVaultAccount()
        {
            this.AddKeyVaultAccountViewModel = new AddKeyVaultAccountViewModel(this, new KeyVaultConfiguration());
        }

        public void AddKeyVauleAccount(KeyVaultConfiguration keyVaultConfiguration)
        {
            this.KeyVaultConfigurations.Add(keyVaultConfiguration);
            this.CloseAddKeyVaultAccountModal();
        }

        public void CancelAddKeyVaultAccount()
        {
            this.CloseAddKeyVaultAccountModal();
        }

        private void CloseAddKeyVaultAccountModal()
        {
            this.AddKeyVaultAccountViewModel = null;
        }

        public bool CheckIfVaultUrlAlreadyExists(string keyVaultUrl)
        {
            return this.KeyVaultConfigurations.Any(c => string.Equals(keyVaultUrl, c.AzureKeyVaultUrl, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}