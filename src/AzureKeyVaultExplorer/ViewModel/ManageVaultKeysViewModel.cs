namespace AzureKeyVaultExplorer.ViewModel
{
    using System;

    using AzureKeyVaultExplorer.Interface;
    using AzureKeyVaultExplorer.Model;

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
            this.DeleteKeyCommand = new RelayCommand(this.OnDeleteCommand, this.CanExecuteDeleteCommand);
        }

        public event EventHandler KeysModified;

        public RelayCommand GetAllCommand { get; set; }

        public RelayCommand DeleteKeyCommand { get; set; }

        public Key SelectedKey { get; private set; }

        public void SetSelectedKey(Key key)
        {
            this.SelectedKey = key;
            this.DeleteKeyCommand.RaiseCanExecuteChanged();
        }

        private async void OnDeleteCommand()
        {
            await this.keyVaultRepository.Delete(this.SelectedKey);
            await this.keyRepository.Delete(this.SelectedKey);
            this.OnKeysModified();
        }

        private bool CanExecuteDeleteCommand()
        {
            return this.SelectedKey != null;
        }

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