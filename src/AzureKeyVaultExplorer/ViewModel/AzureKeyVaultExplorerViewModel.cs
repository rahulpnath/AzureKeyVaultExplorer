namespace AzureKeyVaultExplorer.ViewModel
{
    using AzureKeyVaultExplorer.Model;

    using GalaSoft.MvvmLight;

    public class AzureKeyVaultExplorerViewModel : ViewModelBase
    {
        private KeyVaultAccountViewModel keyVaultAccountViewModel;

        public AzureKeyVaultExplorerViewModel()
        {
            this.ManageKeyVaultAccountsViewModel = new ManageKeyVaultAccountsViewModel(new KeyVaultConfigurationRepository());
            this.ManageKeyVaultAccountsViewModel.ConfigurationChanged += this.HandleConfigurationChanged;
        }

        public ManageKeyVaultAccountsViewModel ManageKeyVaultAccountsViewModel { get; set; }

        public KeyVaultAccountViewModel KeyVaultAccountViewModel
        {
            get
            {
                return this.keyVaultAccountViewModel;
            }

            set
            {
                this.keyVaultAccountViewModel = value;
                this.RaisePropertyChanged(() => this.KeyVaultAccountViewModel);
            }
        }

        private void HandleConfigurationChanged(object sender, KeyVaultConfigurationChangedEventArgs e)
        {
            this.KeyVaultAccountViewModel = new KeyVaultAccountViewModel(e.KeyVaultConfiguration, new KeyRepository(e.KeyVaultConfiguration.VaultName));
        }
    }
}