namespace AzureKeyVaultExplorer.ViewModel
{
    using System;
    using System.Text.RegularExpressions;
    using AzureKeyVaultExplorer.Interface;
    using AzureKeyVaultExplorer.Model;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using MvvmValidation;

    public class AddKeyVaultAccountViewModel : ValidatableViewModelBase
    {
        private const string KeyVaultUrlFormat = @"https://((www.)?)(?<vaultname>[a-zA-Z0-9-]{3,24}).vault.azure.net(/)?";

        private readonly IKeyVaultConfigurationRepository keyVaultConfigurationRepository;

        private readonly Regex vaultUrlMatcher;

        private KeyVaultConfiguration keyVaultConfiguration;

        private string keyVaultUrl;

        private string vaultName;

        private string adApplicationId;

        private string adApplicationSecret;

        public AddKeyVaultAccountViewModel(IKeyVaultConfigurationRepository keyVaultConfigurationRepository, KeyVaultConfiguration keyVaultConfiguration)
        {
            this.keyVaultConfigurationRepository = keyVaultConfigurationRepository;
            this.vaultUrlMatcher = new Regex(KeyVaultUrlFormat, RegexOptions.Compiled);
            this.AddKeyVaultAccountCommand = new RelayCommand(this.AddKeyVaultAccount, this.CanAddKeyVaultCommand);
            this.Initialize(keyVaultConfiguration);
        }

        public event EventHandler RequestClose;

        public string KeyVaultUrl
        {
            get
            {
                return this.keyVaultUrl;
            }

            set
            {
                this.keyVaultUrl = value;
                this.RaisePropertyChanged(() => this.KeyVaultUrl);
                this.AddKeyVaultAccountCommand.RaiseCanExecuteChanged();
            }
        }

        public string ADApplicationId
        {
            get
            {
                return this.adApplicationId;
            }

            set
            {
                this.adApplicationId = value;
                this.RaisePropertyChanged(() => this.ADApplicationId);
                this.AddKeyVaultAccountCommand.RaiseCanExecuteChanged();
            }
        }

        public string ADApplicationSecret
        {
            get
            {
                return this.adApplicationSecret;
            }

            set
            {
                this.adApplicationSecret = value;
                this.RaisePropertyChanged(() => this.ADApplicationSecret);
                this.AddKeyVaultAccountCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand AddKeyVaultAccountCommand { get; set; }

        public RelayCommand CancelAddKeyVaultAccountCommand
        {
            get
            {
                return new RelayCommand(this.CancelAddKeyVaultAccount);
            }
        }

        private void Initialize(KeyVaultConfiguration keyVaultConfiguration)
        {
            this.keyVaultConfiguration = keyVaultConfiguration;
            this.KeyVaultUrl = keyVaultConfiguration.AzureKeyVaultUrl;
            this.ADApplicationId = keyVaultConfiguration.ADApplicationClientId;
            this.ADApplicationSecret = keyVaultConfiguration.ADApplicationSecret;
            this.SetupValidationRules();
        }

        private void SetupValidationRules()
        {
            this.Validator.AddRequiredRule(() => this.KeyVaultUrl, "Azure KeyVaultUrl is Required");
            this.Validator.AddRequiredRule(() => this.ADApplicationId, "AD ApplicationId is Required");
            this.Validator.AddRequiredRule(() => this.ADApplicationSecret, "AD ApplicationSecret is Required");

            this.Validator.AddRule(() => this.KeyVaultUrl, () => RuleResult.Assert(this.CheckIfVaultUrlMatchesFormat(), @"Key Vault Url does not match format eg. https://www.<vaultname>.vault.azure.net/ "));
            this.Validator.AddRule(() => this.KeyVaultUrl, () => RuleResult.Assert(this.keyVaultConfigurationRepository.Get(this.vaultName) == null, @"Key Vault Url configuration already exists"));
        }

        private void CancelAddKeyVaultAccount()
        {
            this.RaiseRequestClose();
        }

        private bool CanAddKeyVaultCommand()
        {
            var result = this.Validator.ValidateAll();
            return result.IsValid;
        }

        private bool CheckIfVaultUrlMatchesFormat()
        {
            if (this.keyVaultUrl == null)
            {
                return false;
            }

            var match = this.vaultUrlMatcher.Match(this.KeyVaultUrl);
            if (match.Success)
            {
                this.vaultName = match.Groups["vaultname"].Value;
            }

            return match.Success;
        }

        // http://stackoverflow.com/questions/16848562/async-await-in-mvvm-without-void-methods
        private async void AddKeyVaultAccount()
        {
            this.keyVaultConfiguration.AzureKeyVaultUrl = this.KeyVaultUrl;
            this.keyVaultConfiguration.VaultName = this.vaultName;
            this.keyVaultConfiguration.ADApplicationSecret = this.ADApplicationSecret;
            this.keyVaultConfiguration.ADApplicationClientId = this.ADApplicationId;
            await this.keyVaultConfigurationRepository.InsertOrUpdate(this.keyVaultConfiguration);
            this.RaiseRequestClose();
        }

        private void RaiseRequestClose()
        {
            if (this.RequestClose != null)
            {
                this.RequestClose(this, EventArgs.Empty);
            }
        }
    }
}