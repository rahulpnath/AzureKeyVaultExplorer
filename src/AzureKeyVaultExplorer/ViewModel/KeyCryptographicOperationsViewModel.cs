namespace AzureKeyVaultExplorer.ViewModel
{
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using AzureKeyVaultExplorer.Interface;
    using AzureKeyVaultExplorer.Model;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    public class KeyCryptographicOperationsViewModel : ViewModelBase
    {
        private readonly IKeyOperations keyOperations;

        private string inputString;

        private string encryptedString;

        public KeyCryptographicOperationsViewModel(IKeyOperations keyOperations)
        {
            this.keyOperations = keyOperations;
            this.EncryptCommand = new RelayCommand(this.OnEncryptCommand);
            this.DecryptCommand = new RelayCommand(this.OnDecryptCommand);
        }

        public string InputString
        {
            get
            {
                return this.inputString;
            }
            set
            {
                this.inputString = value;
            }
        }

        public string EncryptedString
        {
            get
            {
                return this.encryptedString;
            }
            set
            {
                this.encryptedString = value;
                this.RaisePropertyChanged(() => this.EncryptedString);
            }
        }

        public Key CurrentKey { get; set; }

        public RelayCommand EncryptCommand { get; set; }

        public RelayCommand DecryptCommand { get; set; }

        private async void OnEncryptCommand()
        {
            this.EncryptedString = null;
            this.EncryptedString = await this.keyOperations.Encrypt(this.CurrentKey, this.inputString);
        }

        private async void OnDecryptCommand()
        {
            this.EncryptedString = null;
            this.EncryptedString = await this.keyOperations.Decrypt(this.CurrentKey, this.inputString);
        }
    }
}