namespace AzureKeyVaultExplorer.ViewModel
{
    using System;
    using System.Windows.Media.TextFormatting;

    using AzureKeyVaultExplorer.Interface;
    using AzureKeyVaultExplorer.Model;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    public class KeyCryptographicOperationsViewModel : ViewModelBase
    {
        private readonly IKeyOperations keyOperations;

        private string inputString;

        private string encryptedString;

        private byte[] inputAsArray;

        public KeyCryptographicOperationsViewModel(IKeyOperations keyOperations)
        {
            this.keyOperations = keyOperations;
            this.SelectedDataConverter = new Base64DataConverter();
            this.EncryptedString = this.SelectedDataConverter.DisplayMessage;
            this.EncryptCommand = new RelayCommand(this.OnEncryptCommand, this.CanExecuteCommand);
            this.DecryptCommand = new RelayCommand(this.OnDecryptCommand, this.CanExecuteCommand);
        }

        public IDataConverter SelectedDataConverter { get; set; }

        public string InputString
        {
            get
            {
                return this.inputString;
            }

            set
            {
                this.inputString = value;
                this.EncryptCommand.RaiseCanExecuteChanged();
                this.DecryptCommand.RaiseCanExecuteChanged();
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
            this.EncryptedString = await this.keyOperations.Encrypt(this.CurrentKey, this.inputString, this.SelectedDataConverter);
        }

        private bool CanExecuteCommand()
        {
            if (!string.IsNullOrWhiteSpace(this.inputString))
            {
                try
                {
                    this.EncryptedString = string.Empty;
                    this.inputAsArray = this.SelectedDataConverter.ConvertToByteArray(this.inputString);
                    return true;
                }
                catch (Exception)
                {
                    this.EncryptedString = this.SelectedDataConverter.DisplayMessage;
                }
            }

            return false;
        }

        private async void OnDecryptCommand()
        {
            this.EncryptedString = null;
            this.EncryptedString = await this.keyOperations.Decrypt(this.CurrentKey, this.inputString, this.SelectedDataConverter);
        }
    }
}