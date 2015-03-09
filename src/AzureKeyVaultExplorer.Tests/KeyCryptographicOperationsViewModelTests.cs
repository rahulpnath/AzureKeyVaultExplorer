namespace AzureKeyVaultExplorer.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using AzureKeyVaultExplorer.Interface;
    using AzureKeyVaultExplorer.Model;
    using AzureKeyVaultExplorer.ViewModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class KeyCryptographicOperationsViewModelTests
    {
        [TestMethod]
        public void VerifyCopyCommandCopiesToClipboard()
        {
            var keyoperationsMock = new Mock<IKeyOperations>();
            var viewModel = new KeyCryptographicOperationsViewModel(keyoperationsMock.Object);
            Assert.IsNotNull(viewModel.CopyToClipboardCommand);
            viewModel.EncryptedString = "test";
            viewModel.CopyToClipboardCommand.Execute(null);
            var data = Clipboard.GetText(TextDataFormat.Text) as string;
            Assert.AreEqual<string>(data, viewModel.EncryptedString);
        }
    }
}
