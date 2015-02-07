namespace AzureKeyVaultExplorer.ViewModel
{
    using System;

    using AzureKeyVaultExplorer.Interface;
    using AzureKeyVaultExplorer.Model;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    public class ManageLocalKeysViewModel : ViewModelBase
    {
        private readonly string vaultName;

        private readonly IKeyRepository keyRepository;

        private AddKeyViewModel addKeyViewModel;

        public ManageLocalKeysViewModel(string vaultName, IKeyRepository keyRepository)
        {
            this.vaultName = vaultName;
            this.keyRepository = keyRepository;
            this.AddKeyCommand = new RelayCommand(this.OnAddKey);
        }

        public event EventHandler KeysModified;

        public AddKeyViewModel AddKeyViewModel
        {
            get
            {
                return this.addKeyViewModel;
            }

            set
            {
                this.addKeyViewModel = value;
                this.RaisePropertyChanged(() => this.AddKeyViewModel);
            }
        }

        public RelayCommand AddKeyCommand { get; set; }

        private void OnAddKey()
        {
            this.AddKeyViewModel = new AddKeyViewModel(this.keyRepository, this.vaultName, new Key());
            this.AddKeyViewModel.RequestClose += this.HandleRequestCloseForAddKeyModel;
        }

        private void HandleRequestCloseForAddKeyModel(object sender, System.EventArgs e)
        {
            this.AddKeyViewModel.RequestClose -= this.HandleRequestCloseForAddKeyModel;
            this.AddKeyViewModel = null;
            this.RaiseKeysModified();
        }

        private void RaiseKeysModified()
        {
            if (this.KeysModified != null)
            {
                this.KeysModified(this, EventArgs.Empty);
            }
        }
    }
}