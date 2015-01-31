namespace AzureKeyVaultExplorer.ViewModel
{
    using GalaSoft.MvvmLight;

    public class AzureKeyVaultExplorerViewModel : ViewModelBase
    {
        public ManageKeyVaultAccountsViewModel ManageKeyVaultAccountsViewModel { get; set; }

        public AzureKeyVaultExplorerViewModel()
        {
            this.ManageKeyVaultAccountsViewModel = new ManageKeyVaultAccountsViewModel();
        }
    }
}