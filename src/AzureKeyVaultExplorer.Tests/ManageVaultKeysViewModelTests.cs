namespace AzureKeyVaultExplorer.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AzureKeyVaultExplorer.Interface;
    using AzureKeyVaultExplorer.Model;
    using AzureKeyVaultExplorer.ViewModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ManageVaultKeysViewModelTests
    {
        private const string VaultName = "testvault";

        private static IEnumerable<Key> AllKeys
        {
            get
            {
                return new List<Key>()
                       {
                           { new Key("https://test.vault.azure.net/keys/TestKey/09d4ghadf45d423e9c294685a8aa562f") },
                           { new Key("https://test.vault.azure.net/keys/TestKeyAlternate/09d4ghadf45d423e9c294685a8aa562f") }
                       };
            }
        }

        private static Key Key
        {
            get
            {
                return new Key("https://test.vault.azure.net/keys/TestKey/09d4ghadf45d423e9c294685a8aa562f");
            }
        }

        [TestMethod]
        public void CreateInstanceTests()
        {
            var repository = new Mock<IKeyRepository>();
            var keyVaultRepository = new Mock<IKeyRepository>();
            var viewModel = new ManageVaultKeysViewModel(VaultName, repository.Object, keyVaultRepository.Object);
            Assert.IsNotNull(viewModel.GetAllCommand);
            Assert.IsNotNull(viewModel.DeleteKeyCommand);
            Assert.IsNull(viewModel.SelectedKey);
        }

        [TestMethod]
        public void GetAllCommandTest()
        {
            var keyVaultRepository = new Mock<IKeyRepository>();
            var allKeys = AllKeys;
            keyVaultRepository.Setup(a => a.GetAll()).Returns(Task.FromResult(allKeys));
            var repository = new Mock<IKeyRepository>();
            var viewModel = new ManageVaultKeysViewModel(VaultName, repository.Object, keyVaultRepository.Object);
            viewModel.KeysModified += (sender, args) => Assert.IsNotNull(sender);
            viewModel.GetAllCommand.Execute(null);
            repository.Verify(a => a.Add(It.IsAny<Key>()), Times.Exactly(allKeys.Count()));
        }

        [TestMethod]
        public void CanDeleteCommandTest()
        {
            var keyVaultRepository = new Mock<IKeyRepository>();
            var repository = new Mock<IKeyRepository>();
            var viewModel = new ManageVaultKeysViewModel(VaultName, repository.Object, keyVaultRepository.Object);
            Assert.IsFalse(viewModel.DeleteKeyCommand.CanExecute(null));
            viewModel.SetSelectedKey(Key);
            Assert.IsTrue(viewModel.DeleteKeyCommand.CanExecute(null));
        }

        [TestMethod]
        public void DeleteCommandTest()
        {
            var keyVaultRepository = new Mock<IKeyRepository>();
            var repository = new Mock<IKeyRepository>();
            var viewModel = new ManageVaultKeysViewModel(VaultName, repository.Object, keyVaultRepository.Object);
            viewModel.KeysModified += (sender, args) => Assert.IsNotNull(sender);
            var key = Key;
            viewModel.SetSelectedKey(key);
            viewModel.DeleteKeyCommand.Execute(null);
            keyVaultRepository.Verify(a => a.Delete(key));
            repository.Verify(a => a.Delete(key));
        }
    }
}