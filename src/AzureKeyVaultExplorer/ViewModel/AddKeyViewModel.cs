namespace AzureKeyVaultExplorer.ViewModel
{
    using System;
    using AzureKeyVaultExplorer.Interface;
    using AzureKeyVaultExplorer.Model;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    public class AddKeyViewModel : ViewModelBase
    {
        private readonly IKeyRepository keyRepository;

        private readonly string currentKeyVault;

        private string keyVersion;

        private string keyName;

        public AddKeyViewModel(IKeyRepository keyRepository, string currentKeyVault)
        {
            this.keyRepository = keyRepository;
            this.currentKeyVault = currentKeyVault;
            this.AddKeyCommand = new RelayCommand(this.OnAddKeyCommand, this.CanAddKeyCommand);
            this.CancelAddKeyCommand = new RelayCommand(this.OnCancelAddKeyCommand);
        }

        public delegate void OnKeyAdded(object sender, KeyAddedEventArgs args);

        public event EventHandler RequestClose;

        public event OnKeyAdded KeyAdded;

        public string HeaderText { get; set; }

        public string KeyName
        {
            get
            {
                return this.keyName;
            }

            set
            {
                this.keyName = value;
                this.RaisePropertyChanged(() => this.KeyName);
                this.AddKeyCommand.RaiseCanExecuteChanged();
            }
        }

        public string KeyVersion
        {
            get
            {
                return this.keyVersion;
            }

            set
            {
                this.keyVersion = value;
                this.RaisePropertyChanged(() => this.KeyVersion);
                this.AddKeyCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand AddKeyCommand { get; set; }

        public RelayCommand CancelAddKeyCommand { get; set; }

        private bool CanAddKeyCommand()
        {
            return !string.IsNullOrWhiteSpace(this.KeyName);
        }

        private void OnCancelAddKeyCommand()
        {
            this.RaiseRequestClose();
        }

        private void RaiseRequestClose()
        {
            if (this.RequestClose != null)
            {
                this.RequestClose(this, EventArgs.Empty);
            }
        }

        private void RaiseKeyAdded(Key key)
        {
            if (this.KeyAdded != null)
            {
                this.KeyAdded(this, new KeyAddedEventArgs(key));
            }
        }

        private void OnAddKeyCommand()
        {
            var key = new Key(this.currentKeyVault, this.KeyName, this.KeyVersion);
            this.RaiseKeyAdded(key);
            this.RaiseRequestClose();
        }
    }
}