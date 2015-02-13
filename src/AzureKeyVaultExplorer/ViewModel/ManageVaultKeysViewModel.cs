namespace AzureKeyVaultExplorer.ViewModel
{
    using System;

    using AzureKeyVaultExplorer.Interface;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    public class ManageVaultKeysViewModel : ViewModelBase
    {
        private readonly string vaultName;
        
        private readonly IKeyRepository keyRepository;

        private readonly IKeyRepository keyVaultRepository;

        public ManageVaultKeysViewModel(string vaultName, IKeyRepository keyRepository, IKeyRepository keyVaultRepository)
        {
            this.vaultName = vaultName;
            this.keyRepository = keyRepository;
            this.keyVaultRepository = keyVaultRepository;
            this.GetAllCommand = new RelayCommand(this.OnGetAllCommand);
        }

        public event EventHandler KeysModified;

        public RelayCommand GetAllCommand { get; set; }

        private async void OnGetAllCommand()
        {
            var keysFromVault = await this.keyVaultRepository.GetAll();
            foreach (var key in keysFromVault)
            {
                await this.keyRepository.Add(key);
            }

            this.OnKeysModified();
        }
        
        private void OnKeysModified()
        {
            EventHandler handler = this.KeysModified;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}