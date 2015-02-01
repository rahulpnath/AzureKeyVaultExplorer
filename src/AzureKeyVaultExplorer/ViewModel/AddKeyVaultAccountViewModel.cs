namespace AzureKeyVaultExplorer.ViewModel
{
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.RegularExpressions;
    using System.Windows.Input;
    using System.Windows.Media.TextFormatting;

    using AzureKeyVaultExplorer.Interface;
    using AzureKeyVaultExplorer.Model;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    public class AddKeyVaultAccountViewModel : ViewModelBase
    {
        private const string KeyVaultUrlFormat = @"https://([www]?)(?<vaultname>.*).vault.azure.net(/)?";

        private readonly IKeyVaultConfigurationRepository keyVaultConfigurationRepository;

        private readonly Regex vaultUrlMatcher;

        private KeyVaultConfiguration keyVaultConfiguration;

        private string keyVaultUrl;

        private string adApplicationId;

        private string adApplicationSecret;

        public AddKeyVaultAccountViewModel(IKeyVaultConfigurationRepository keyVaultConfigurationRepository, KeyVaultConfiguration keyVaultConfiguration)
        {
            this.keyVaultConfigurationRepository = keyVaultConfigurationRepository;
            this.vaultUrlMatcher = new Regex(KeyVaultUrlFormat, RegexOptions.Compiled);
            this.AddKeyVaultAccountCommand = new RelayCommand(this.AddKeyVaultAccount, this.CanAddKeyVaultCommand);
            this.Init(keyVaultConfiguration);
        }

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
        
        private void Init(KeyVaultConfiguration keyVaultConfiguration)
        {
            this.keyVaultConfiguration = keyVaultConfiguration;
            this.KeyVaultUrl = keyVaultConfiguration.AzureKeyVaultUrl;
            this.ADApplicationId = keyVaultConfiguration.ADApplicationClientId;
            this.ADApplicationSecret = keyVaultConfiguration.ADApplicationSecret;
        }

        private void CancelAddKeyVaultAccount()
        {
        }

        private bool CanAddKeyVaultCommand()
        {
            var hasAnythingEmpty = string.IsNullOrWhiteSpace(this.KeyVaultUrl)
                                  || string.IsNullOrWhiteSpace(this.ADApplicationId)
                                  || string.IsNullOrWhiteSpace(this.ADApplicationSecret);
            var isValidVaultUrl = this.CheckIfVaultUrlMatchesFormat();

            var vaultUrlAlreadyExists = this.keyVaultConfigurationRepository.Get(this.KeyVaultUrl) != null;

            return !hasAnythingEmpty && !vaultUrlAlreadyExists && isValidVaultUrl;
        }

        private bool CheckIfVaultUrlMatchesFormat()
        {
           return this.keyVaultUrl != null && this.vaultUrlMatcher.Match(this.KeyVaultUrl).Success;
        }

        private void AddKeyVaultAccount()
        {
            this.keyVaultConfiguration.AzureKeyVaultUrl = this.KeyVaultUrl;
            this.keyVaultConfiguration.VaultName = this.GetVaultNameFromUrl();
            this.keyVaultConfiguration.ADApplicationSecret = this.ADApplicationSecret;
            this.keyVaultConfiguration.ADApplicationClientId = this.ADApplicationId;
            this.keyVaultConfigurationRepository.Insert(this.keyVaultConfiguration);
        }

        private string GetVaultNameFromUrl()
        {
            var match = this.vaultUrlMatcher.Match(this.KeyVaultUrl);
            return match.Groups["vaultname"].Value;
        }
    }
}