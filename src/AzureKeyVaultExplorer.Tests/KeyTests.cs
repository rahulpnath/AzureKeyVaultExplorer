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
            Assert.IsTrue(key.ToString().Equals(key.KeyBundle.Key.Kid));
        }

        private static Key GetTestKey()
        {
            var keyBundle = new KeyBundle();
            keyBundle.Key = new JsonWebKey();
            keyBundle.Key.Kid = "https://test.vault.azure.net/keys/TestKey";
            var key = new Key(keyBundle, true);
            return key;
        }

        private static Key GetAnotherTestKey()
        {
            var keyBundle = new KeyBundle();
            keyBundle.Key = new JsonWebKey();
            keyBundle.Key.Kid = "https://test.vault.azure.net/keys/AnotherTestKey";
            var key = new Key(keyBundle, true);
            return key;
        }
    }
}