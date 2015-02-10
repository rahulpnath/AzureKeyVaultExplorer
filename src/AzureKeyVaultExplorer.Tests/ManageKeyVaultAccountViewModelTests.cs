namespace AzureKeyVaultExplorer.Tests
{
    using AzureKeyVaultExplorer.Interface;
    using AzureKeyVaultExplorer.Model;
    using AzureKeyVaultExplorer.ViewModel;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestClass]
    public class ManageKeyVaultAccountViewModelTests
    {
        [TestMethod]
        public void CreateInstanceTest()
        {
            var viewModel = new ManageKeyVaultAccountsViewModel(new Mock<IKeyVaultConfigurationRepository>().Object);
            Assert.IsTrue(viewModel.AddKeyVaultAccountCommand.CanExecute(null));
            Assert.IsNull(viewModel.SelectedKeyVaultConfiguration);
            Assert.IsFalse(viewModel.DeleteKeyVaultAccountCommand.CanExecute(null));
        }

        [TestMethod]
        public void CanDeleteKeyVaultConfigurationTest()
        {
            var repository = new Mock<IKeyVaultConfigurationRepository>();
            var viewModel = new ManageKeyVaultAccountsViewModel(repository.Object);
            viewModel.SelectedKeyVaultConfiguration = new Mock<KeyVaultConfiguration>().Object;
            Assert.IsTrue(viewModel.DeleteKeyVaultAccountCommand.CanExecute(null));
        }

        [TestMethod]
        public void DeleteKeyVaultConfigurationTest()
        {
            var repository = new Mock<IKeyVaultConfigurationRepository>();
            var configuration = new Mock<KeyVaultConfiguration>().Object;
            var viewModel = new ManageKeyVaultAccountsViewModel(repository.Object)
                            {
                                SelectedKeyVaultConfiguration = configuration
                            };
            Assert.IsTrue(viewModel.DeleteKeyVaultAccountCommand.CanExecute(null));
            var isEventRaised = false;
            viewModel.ConfigurationChanged += (sender, args) =>
            {
                isEventRaised = true;
                Assert.IsNull(args.KeyVaultConfiguration);
            };
            viewModel.DeleteKeyVaultAccountCommand.Execute(null);
            repository.Verify(a => a.Delete(configuration), Times.Once);
            Assert.IsTrue(isEventRaised);
        }
    }
}