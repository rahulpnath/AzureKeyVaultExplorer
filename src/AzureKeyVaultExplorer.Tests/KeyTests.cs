namespace AzureKeyVaultExplorer.Tests
{
    using AzureKeyVaultExplorer.Model;

    using Microsoft.KeyVault.Client;
    using Microsoft.KeyVault.WebKey;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class KeyTests
    {
        [TestMethod]
        public void GetKeyHashCodeTest()
        {
            var key = GetTestKey();
            var anotherInstanceOfKey = GetTestKey();

            Assert.IsNotNull(key.GetHashCode());
            Assert.IsTrue(key.GetHashCode().Equals(anotherInstanceOfKey.GetHashCode()));
        }

        [TestMethod]
        public void KeyEqualsTest()
        {
            var key = GetTestKey();
            var anotherInstanceOfKey = GetTestKey();

            Assert.IsTrue(key.Equals(anotherInstanceOfKey));
        }

        [TestMethod]
        public void KeyNotEqualsTest()
        {
            var key = GetTestKey();
            var anotherNew = GetAnotherTestKey();
            Assert.IsTrue(!key.Equals(anotherNew));
        }

        [TestMethod]
        public void GetKeyToStringTest()
        {
            var key = GetTestKey();

            Assert.IsNotNull(key.ToString());
            Assert.IsTrue(key.ToString().Equals(key.KeyIdentifier));
        }

        [TestMethod]
        public void CanAddKeyCommandUrlWithNoTrailingSlashAndNoVersionNumberTest()
        {
            var key = new Key("https://test.vault.azure.net/keys/TestKey");
            Assert.AreEqual(key.Name, "TestKey");
            Assert.IsNull(key.Version);
            Assert.AreEqual(key.VaultName, "test");
        }

        [TestMethod]
        public void CanAddKeyCommandUrlWithTrailingSlashAndNoVersionNumberTest()
        {
            var key = new Key("https://test.vault.azure.net/keys/TestKey/");
            Assert.AreEqual(key.Name, "TestKey");
            Assert.IsNull(key.Version);
            Assert.AreEqual(key.VaultName, "test");
        }

        [TestMethod]
        public void CanAddKeyCommandUrlWithNoTrailingSlashAndVersionNumberTest()
        {
            var key = new Key("https://test.vault.azure.net/keys/TestKey/0f653b06c1d94159bc7090596bbf7784");
            Assert.AreEqual(key.Name, "TestKey");
            Assert.AreEqual(key.Version, "0f653b06c1d94159bc7090596bbf7784");
            Assert.AreEqual(key.VaultName, "test");
        }

        [TestMethod]
        public void CanAddKeyCommandUrlWithTrailingSlashAndVersionNumberTest()
        {
            var key = new Key("https://test.vault.azure.net/keys/TestKey/0f653b06c1d94159bc7090596bbf7784/");
            Assert.AreEqual(key.Name, "TestKey");
            Assert.AreEqual(key.Version, "0f653b06c1d94159bc7090596bbf7784");
            Assert.AreEqual(key.VaultName, "test");
        }

        private static Key GetTestKey()
        {
            return new Key("https://test.vault.azure.net/keys/TestKey");
        }

        private static Key GetAnotherTestKey()
        {
            return new Key("https://test.vault.azure.net/keys/AnotherTestKey");
        }
    }
}