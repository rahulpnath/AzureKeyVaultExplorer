namespace AzureKeyVaultExplorer.ViewModel
{
    using GalaSoft.MvvmLight;

    public class ManageVaultKeysViewModel : ViewModelBase
    {
        private readonly string vaultName;

        public ManageVaultKeysViewModel(string vaultName)
        {
            this.vaultName = vaultName;
        }
    }
}