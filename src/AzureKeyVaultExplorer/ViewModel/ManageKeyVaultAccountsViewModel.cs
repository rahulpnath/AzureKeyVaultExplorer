﻿namespace AzureKeyVaultExplorer.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    using AzureKeyVaultExplorer.Interface;
    using AzureKeyVaultExplorer.Model;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    public class ManageKeyVaultAccountsViewModel : ViewModelBase
    {
        private readonly IKeyVaultConfigurationRepository keyVaultConfigurationRepository;

        private AddKeyVaultAccountViewModel addKeyVaultAccountViewModel;

        private KeyVaultConfiguration selectedKeyVaultConfiguration;

        public ManageKeyVaultAccountsViewModel(IKeyVaultConfigurationRepository keyVaultConfigurationRepository)
        {
            this.keyVaultConfigurationRepository = keyVaultConfigurationRepository;
            this.DeleteKeyVaultAccountCommand = new RelayCommand(this.OnDeleteKeyVaultConfiguration, this.CanDeleteKeyVaultConfiguration);
        }

        public delegate void ConfigurationChangedEventHadler(object sender, KeyVaultConfigurationChangedEventArgs e);

        public event ConfigurationChangedEventHadler ConfigurationChanged;

        public AddKeyVaultAccountViewModel AddKeyVaultAccountViewModel
        {
            get
            {
                return this.addKeyVaultAccountViewModel;
            }

            set
            {
                this.addKeyVaultAccountViewModel = value;
                this.RaisePropertyChanged(() => this.AddKeyVaultAccountViewModel);
            }
        }

        public ObservableCollection<KeyVaultConfiguration> KeyVaultConfigurations
        {
            get
            {
                return new ObservableCollection<KeyVaultConfiguration>(this.keyVaultConfigurationRepository.All);
            }
        }

        public KeyVaultConfiguration SelectedKeyVaultConfiguration
        {
            get
            {
                return this.selectedKeyVaultConfiguration;
            }

            set
            {
                this.selectedKeyVaultConfiguration = value;
                this.RaiseConfigurationChangedEvent();
                this.DeleteKeyVaultAccountCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand AddKeyVaultAccountCommand
        {
            get
            {
                return new RelayCommand(this.OpenAddKeyVaultAccount);
            }
        }

        public RelayCommand DeleteKeyVaultAccountCommand { get; private set; }

        public bool CheckIfVaultUrlAlreadyExists(string keyVaultUrl)
        {
            return this.KeyVaultConfigurations.Any(c => string.Equals(keyVaultUrl, c.AzureKeyVaultUrl, StringComparison.CurrentCultureIgnoreCase));
        }

        private bool CanDeleteKeyVaultConfiguration()
        {
            return this.SelectedKeyVaultConfiguration != null;
        }

        private void OnDeleteKeyVaultConfiguration()
        {
            this.keyVaultConfigurationRepository.Delete(this.selectedKeyVaultConfiguration);
            this.RaisePropertyChanged(() => this.KeyVaultConfigurations);
            this.SelectedKeyVaultConfiguration = null;
            this.RaiseConfigurationChangedEvent();
        }

        private void OpenAddKeyVaultAccount()
        {
            this.AddKeyVaultAccountViewModel = new AddKeyVaultAccountViewModel(this.keyVaultConfigurationRepository, new KeyVaultConfiguration());
            this.AddKeyVaultAccountViewModel.RequestClose += this.HandleAddKeyVaultAccountViewModelRequestClose;
        }

        private void RaiseConfigurationChangedEvent()
        {
            if (this.ConfigurationChanged != null)
            {
                this.ConfigurationChanged(this, new KeyVaultConfigurationChangedEventArgs(this.SelectedKeyVaultConfiguration));
            }
        }

        private void HandleAddKeyVaultAccountViewModelRequestClose(object sender, EventArgs e)
        {
            this.AddKeyVaultAccountViewModel.RequestClose -= this.HandleAddKeyVaultAccountViewModelRequestClose;
            this.AddKeyVaultAccountViewModel = null;
            this.RaisePropertyChanged(() => this.KeyVaultConfigurations);
        }
    }
}