namespace AzureKeyVaultExplorer.ViewModel
{
    using System.Collections.ObjectModel;
    using System.Text.RegularExpressions;
    using System.Windows.Input;
    using System.Windows.Media.TextFormatting;

    using AzureKeyVaultExplorer.Model;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    public class AddKeyVaultAccountViewModel : ViewModelBase
    {
        private readonly ManageKeyVaultAccountsViewModel _manageKeyVaultAccountsViewModel;

        private const string KeyVaultUrlFormat = @"https://.*.vault.azure.net/";

        private KeyVaultConfiguration _keyVaultConfiguration;

        private string keyVaultUrl;

        private string adApplicationId;

        private string adApplicationSecret;

        private Regex vaultUrlMatcher;

        public AddKeyVaultAccountViewModel(ManageKeyVaultAccountsViewModel manageKeyVaultAccountsViewModel, KeyVaultConfiguration keyVaultConfiguration)
        {
            this._manageKeyVaultAccountsViewModel = manageKeyVaultAccountsViewModel;
            this.vaultUrlMatcher = new Regex(KeyVaultUrlFormat, RegexOptions.Compiled);
            this.AddKeyVaultAccountCommand = new RelayCommand(this.AddKeyVaultAccount, this.CanAddKeyVaultCommand);
            this.Init(keyVaultConfiguration);
        }

        private void Init(KeyVaultConfiguration keyVaultConfiguration)
        {
            this._keyVaultConfiguration = keyVaultConfiguration;
            this.KeyVaultUrl = keyVaultConfiguration.AzureKeyVaultUrl ?? string.Empty;
            this.ADApplicationId = keyVaultConfiguration.ADApplicationClientId ?? string.Empty;
            this.ADApplicationSecret = keyVaultConfiguration.ADApplicationSecret ?? string.Empty;
        }

        public RelayCommand AddKeyVaultAccountCommand { get; set; }

        public RelayCommand CancelAddKeyVaultAccountCommand
        {
            get
            {
                return new RelayCommand(this.CancelAddKeyVaultAccount);
            }
        }

        private void CancelAddKeyVaultAccount()
        {
            this._manageKeyVaultAccountsViewModel.CancelAddKeyVaultAccount();
        }

        private bool CanAddKeyVaultCommand()
        {
            var hasAnythingEmpty = string.IsNullOrWhiteSpace(this.KeyVaultUrl)
                                  || string.IsNullOrWhiteSpace(this.ADApplicationId)
                                  || string.IsNullOrWhiteSpace(this.ADApplicationSecret);
            var isValidVaultUrl = this.CheckIfVaultUrlMatchesFormat();

            var vaultUrlAlreadyExists = this._manageKeyVaultAccountsViewModel.CheckIfVaultUrlAlreadyExists(this.KeyVaultUrl);

            return !hasAnythingEmpty && !vaultUrlAlreadyExists && isValidVaultUrl;
        }

        private bool CheckIfVaultUrlMatchesFormat()
        {
           return this.vaultUrlMatcher.Match(this.KeyVaultUrl).Success;
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

        private void AddKeyVaultAccount()
        {
            this._keyVaultConfiguration.AzureKeyVaultUrl = this.KeyVaultUrl;
            this._keyVaultConfiguration.ADApplicationSecret = this.ADApplicationSecret;
            this._keyVaultConfiguration.ADApplicationClientId = this.ADApplicationId;
            this._manageKeyVaultAccountsViewModel.AddKeyVauleAccount(this._keyVaultConfiguration);
        }
    }
}