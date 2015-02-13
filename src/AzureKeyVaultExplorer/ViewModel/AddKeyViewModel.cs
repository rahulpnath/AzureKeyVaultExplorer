namespace AzureKeyVaultExplorer.ViewModel
{
    using System;
    using System.Text.RegularExpressions;

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

        public event EventHandler RequestClose;

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

        private async void OnAddKeyCommand()
        {
            var key = new Key(this.currentKeyVault, this.KeyName, this.KeyVersion);
            await this.keyRepository.Add(key);
            this.RaiseRequestClose();
        }
    }
}