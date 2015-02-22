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
            this.DeleteKeyCommand = new RelayCommand(this.OnDeleteKey, this.CanDeleteKey);
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

        public Key SelectedKey { get; private set; }

        public RelayCommand AddKeyCommand { get; set; }

        public RelayCommand DeleteKeyCommand { get; set; }

        public void SetSelectedKey(Key key)
        {
            this.SelectedKey = key;
            this.DeleteKeyCommand.RaiseCanExecuteChanged();
        }

        private void OnDeleteKey()
        {
            this.keyRepository.Delete(this.SelectedKey);
            this.RaiseKeysModified();
        }

        private bool CanDeleteKey()
        {
            return this.SelectedKey != null;
        }

        private void OnAddKey()
        {
            this.AddKeyViewModel = new AddKeyViewModel(this.keyRepository, this.vaultName);
            this.AddKeyViewModel.RequestClose += this.HandleRequestCloseForAddKeyModel;
            this.AddKeyViewModel.KeyAdded += this.HandleKeyAdded;
        }

        private async void HandleKeyAdded(object sender, KeyAddedEventArgs args)
        {
            await this.keyRepository.Add(args.Key);
            this.RaiseKeysModified();
        }

        private void HandleRequestCloseForAddKeyModel(object sender, System.EventArgs e)
        {
            this.AddKeyViewModel.RequestClose -= this.HandleRequestCloseForAddKeyModel;
            this.AddKeyViewModel.KeyAdded -= this.HandleKeyAdded;
            this.AddKeyViewModel = null;
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