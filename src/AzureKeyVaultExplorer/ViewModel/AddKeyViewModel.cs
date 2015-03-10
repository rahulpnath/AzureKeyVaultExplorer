namespace AzureKeyVaultExplorer.ViewModel
{
    using System;
    using System.Text.RegularExpressions;

    using AzureKeyVaultExplorer.Interface;
    using AzureKeyVaultExplorer.Model;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using MvvmValidation;

    public class AddKeyViewModel : ValidatableViewModelBase
    {
        private const string KeyFormat = "^[a-zA-Z0-9-]{1,63}$";

        private readonly IKeyRepository keyRepository;

        private readonly Regex keyMatcher;

        private readonly string currentKeyVault;

        private string keyVersion;

        private string keyName;

        public AddKeyViewModel(IKeyRepository keyRepository, string currentKeyVault)
        {
            this.keyRepository = keyRepository;
            this.currentKeyVault = currentKeyVault;
            this.AddKeyCommand = new RelayCommand(this.OnAddKeyCommand, this.CanAddKeyCommand);
            this.CancelAddKeyCommand = new RelayCommand(this.OnCancelAddKeyCommand);
            this.keyMatcher = new Regex(KeyFormat, RegexOptions.Compiled);
            this.Initialize();
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
            return this.Validator.ValidateAll().IsValid;
        }

        private void Initialize()
        {
            this.Validator.AddRule(() => this.KeyName, () => RuleResult.Assert(!string.IsNullOrWhiteSpace(this.KeyName), "Key Name is required"));
            this.Validator.AddRule(
                () => this.KeyName,
                () =>
                    RuleResult.Assert(
                        this.keyMatcher.Match(this.KeyName).Success,
                        "Key name should have minimum 1 and a maximum of 63 characters and can contain only alphabhets, numbers and '-'"));
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