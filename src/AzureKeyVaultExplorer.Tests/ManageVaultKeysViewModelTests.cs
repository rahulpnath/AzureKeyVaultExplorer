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
                           {
                               new Key
                               {
                                   KeyIdentifier =
                                       "https://test.vault.azure.net/keys/TestKey/09d4ghadf45d423e9c294685a8aa562f",
                                   Name = "TestKey"
                               }
                           },
                           {
                               new Key
                               {
                                   KeyIdentifier =
                                       "https://test.vault.azure.net/keys/TestKeyAlternate/09d4ghadf45d423e9c294685a8aa562f",
                                   Name = "TestKeyAlternate"
                               }
                           }
                       };
            }
        }

        [TestMethod]
        public void CreateInstanceTests()
        {
            var repository = new Mock<IKeyRepository>();
            var keyVaultRepository = new Mock<IKeyRepository>();
            var viewModel = new ManageVaultKeysViewModel(VaultName, repository.Object, keyVaultRepository.Object);
            Assert.IsNotNull(viewModel.GetAllCommand);
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
    }
}