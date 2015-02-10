namespace AzureKeyVaultExplorer.Tests
{
    using AzureKeyVaultExplorer.Interface;
    using AzureKeyVaultExplorer.Model;
    using AzureKeyVaultExplorer.ViewModel;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestClass]
    public class ManageLocalKeysViewModelTests
    {
        private const string VaultName = "TestVault";

        [TestMethod]
        public void CreateInstanceTest()
        {
            var viewModel = new ManageLocalKeysViewModel(VaultName, new Mock<IKeyRepository>().Object);
            Assert.IsNotNull(viewModel.AddKeyCommand);
            Assert.IsNull(viewModel.AddKeyViewModel);
            Assert.IsNotNull(viewModel.DeleteKeyCommand);
            Assert.IsTrue(viewModel.AddKeyCommand.CanExecute(null));
            Assert.IsFalse(viewModel.DeleteKeyCommand.CanExecute(null));
        }

        [TestMethod]
        public void CanDeleteKeyCommandTest()
        {
            var viewModel = new ManageLocalKeysViewModel(VaultName, new Mock<IKeyRepository>().Object);
            viewModel.SetSelectedKey(new Mock<Key>().Object);
            Assert.IsTrue(viewModel.DeleteKeyCommand.CanExecute(null));
        }

        [TestMethod]
        public void DeleteKeyCommandTest()
        {
            var keyRepository = new Mock<IKeyRepository>();
            var key = new Mock<Key>().Object;
            var viewModel = new ManageLocalKeysViewModel(VaultName, keyRepository.Object);
            viewModel.KeysModified += (sender, args) => Assert.IsInstanceOfType(sender, typeof(ManageLocalKeysViewModel));
            viewModel.SetSelectedKey(key);
            viewModel.DeleteKeyCommand.Execute(null);
            keyRepository.Verify(a => a.Delete(key), Times.Once);
        }
    }
}