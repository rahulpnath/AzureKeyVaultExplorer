namespace AzureKeyVaultExplorer.Tests
{
    using AzureKeyVaultExplorer.Model;
    using AzureKeyVaultExplorer.ViewModel;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestMethod]
    public class AddKeyVaultAccountViewModelTests
    {
        private AddKeyVaultAccountViewModel addKeyVaultAccountViewModel;

        [TestInitialize]
        public void Initialize()
        {

            this.addKeyVaultAccountViewModel = new AddKeyVaultAccountViewModel(new Mock<ManageKeyVaultAccountsViewModel>().Object, new Mock<KeyVaultConfiguration>().Object);
        }
    }
}