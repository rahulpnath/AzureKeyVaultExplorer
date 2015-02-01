namespace AzureKeyVaultExplorer.ViewModel
{
    using AzureKeyVaultExplorer.Model;

    using GalaSoft.MvvmLight;

    public class AzureKeyVaultExplorerViewModel : ViewModelBase
    {
        public AzureKeyVaultExplorerViewModel()
        {
            this.ManageKeyVaultAccountsViewModel = new ManageKeyVaultAccountsViewModel(new KeyVaultConfigurationRepository());
        }

        public ManageKeyVaultAccountsViewModel ManageKeyVaultAccountsViewModel { get; set; }
    }
}