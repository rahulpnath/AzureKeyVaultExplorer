namespace AzureKeyVaultExplorer.Tests
{
    using AzureKeyVaultExplorer.Model;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class KeyVaultConfigurationTests
    {
        [TestMethod]
        public void GetKeyVaultConfigurationHashCodeTest()
        {
            var keyVaultConfiguraiton = GetTestKeyVaultConfiguration();
            var anotherInstanceOfKeyVaultConfiguration = GetTestKeyVaultConfiguration();

            Assert.IsNotNull(keyVaultConfiguraiton.GetHashCode());
            Assert.IsTrue(keyVaultConfiguraiton.GetHashCode().Equals(anotherInstanceOfKeyVaultConfiguration.GetHashCode()));
        }

        [TestMethod]
        public void KeyVaultConfigurationEqualsTest()
        {
            var keyVaultConfiguration = GetTestKeyVaultConfiguration();
            var anotherInstanceOfKeyVaultConfiguration = GetTestKeyVaultConfiguration();

            Assert.IsTrue(keyVaultConfiguration.Equals(anotherInstanceOfKeyVaultConfiguration));
        }

        [TestMethod]
        public void KeyNotEqualsTest()
        {
            var keyVaultConfiguration = GetTestKeyVaultConfiguration();
            var anotherKeyVaultConfiguration = GetAnotherTestKeyVaultConfiguration();
            Assert.IsTrue(!keyVaultConfiguration.Equals(anotherKeyVaultConfiguration));
        }

        [TestMethod]
        public void GetKeyVaultConfigurationToStringTest()
        {
            var keyVaultConfiguration = GetTestKeyVaultConfiguration();

            Assert.IsNotNull(keyVaultConfiguration.ToString());
            Assert.IsTrue(keyVaultConfiguration.ToString().Equals(keyVaultConfiguration.VaultName));
        }

        private static KeyVaultConfiguration GetTestKeyVaultConfiguration()
        {
            return new KeyVaultConfiguration()
                   {
                       ADApplicationClientId = "ClientId",
                       ADApplicationSecret = "ApplicationSecret",
                       AzureKeyVaultUrl = "https://test.vault.azure.net",
                       VaultName = "test"
                   };
        }

        private static KeyVaultConfiguration GetAnotherTestKeyVaultConfiguration()
        {
            return new KeyVaultConfiguration()
            {
                ADApplicationClientId = "AnotherClientId",
                ADApplicationSecret = "AnotherApplicationSecret",
                AzureKeyVaultUrl = "https://anothertest.vault.azure.net",
                VaultName = "anothertest"
            };
        }
    }
}