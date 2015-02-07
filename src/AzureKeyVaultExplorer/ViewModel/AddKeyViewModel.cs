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
        private const string KeyIdentifierFormat = @"https://([www]?)(?<vaultname>.*).vault.azure.net/keys/(?<keyname>.*)/(?<keyversion>.*)";

        private readonly IKeyRepository keyRepository;

        private readonly string currentKeyVault;

        private readonly Regex keyMatcher;

        private readonly Key key;

        private string keyIdentifier;

        private string keyName;

        private string keyVersion;

        private string keyVault;

        public AddKeyViewModel(IKeyRepository keyRepository, string currentKeyVault, Key key)
        {
            this.keyRepository = keyRepository;
            this.currentKeyVault = currentKeyVault;
            this.AddKeyCommand = new RelayCommand(this.OnAddKeyCommand, this.CanAddKeyCommand);
            this.CancelAddKeyCommand = new RelayCommand(this.OnCancelAddKeyCommand);
            this.keyMatcher = new Regex(KeyIdentifierFormat, RegexOptions.Compiled);
            this.key = key;
            this.KeyIdentifier = key.KeyIdentifier;
        }

        public event EventHandler RequestClose;

        public string KeyIdentifier
        {
            get
            {
                return this.keyIdentifier;
            }

            set
            {
                this.keyIdentifier = value;
                this.RaisePropertyChanged(() => this.KeyIdentifier);
                this.AddKeyCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand AddKeyCommand { get; set; }

        public RelayCommand CancelAddKeyCommand { get; set; }

        private bool CanAddKeyCommand()
        {
            var isEmpty = string.IsNullOrWhiteSpace(this.KeyIdentifier);
            var isValidFormat = this.CheckIfKeyIdntifierMatchesFormat();
            var isForCurrentVault = this.currentKeyVault.Equals(this.keyVault, StringComparison.CurrentCultureIgnoreCase);

            return !isEmpty && isValidFormat && isForCurrentVault;
        }

        private bool CheckIfKeyIdntifierMatchesFormat()
        {
            if (string.IsNullOrWhiteSpace(this.keyIdentifier))
            {
                return false;
            }

            var matches = this.keyMatcher.Match(this.KeyIdentifier);
            if (matches.Success)
            {
                this.keyName = matches.Groups["keyname"].Value;
                this.keyVersion = matches.Groups["keyversion"].Value;
                this.keyVault = matches.Groups["vaultname"].Value;
            }

            return matches.Success;
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
            this.key.IsLocal = true;
            this.key.Name = this.keyName;
            this.key.KeyIdentifier = this.keyIdentifier;
            await this.keyRepository.InsertOrUpdate(this.key);
            this.RaiseRequestClose();
        }
    }
}