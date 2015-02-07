namespace AzureKeyVaultExplorer.Tests
{
    using System;

    using AzureKeyVaultExplorer.Interface;
    using AzureKeyVaultExplorer.Model;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Base64DataConverterTests
    {
        private const string Base64String = "YW55IGNhcm5hbCBwbGVhcw==";

        private IDataConverter converter;

        [TestInitialize]
        public void Initialize()
        {
            this.converter = new Base64DataConverter();
        }

        [TestMethod]
        public void ConvertValidBase64StringToArrayTest()
        {
            var data = this.converter.ConvertToByteArray(Base64String);
            Assert.IsNotNull(data);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ConvertInValidBase64StringToArrayTest()
        {
            this.converter.ConvertToByteArray("Invalid");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConvertNullBase64StringToArrayTest()
        {
            this.converter.ConvertToByteArray(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConvertNullArrayToStringTest()
        {
            this.converter.ConvertToString(null);
        }
    }
}