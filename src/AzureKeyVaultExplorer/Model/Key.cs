namespace AzureKeyVaultExplorer.Model
{
    using System;
    using System.Text.RegularExpressions;
    using Microsoft.KeyVault.Client;
    using Newtonsoft.Json;

    public class Key
    {
        public Key(string vaultName, string keyName, string version = null, string keyType = "RSA", bool isLocal = false)
        {
            const string KeyIdentifierUrlFormat = @"https://{0}.vault.azure.net/keys/{1}/{2}";
            this.VaultName = vaultName;
            this.Name = keyName;
            this.Version = version;
            this.KeyType = keyType;
            this.IsLocal = isLocal;
            this.KeyIdentifier = string.Format(KeyIdentifierUrlFormat, vaultName, keyName, version);
        }

        [JsonConstructor]
        public Key(string keyIdentifier, string keyType = "RSA", bool isLocal = false)
        {
            const string KeyIdentifierFormat = @"https://([www]?)(?<vaultname>[a-zA-Z0-9-]{3,24}).vault.azure.net/keys/(?<keyname>[a-zA-Z0-9-]{1,63})[/]?(?<keyversion>[^/]*)[/]?";
            var keyMatcher = new Regex(KeyIdentifierFormat);
            var matches = keyMatcher.Match(keyIdentifier);

            if (!matches.Success)
            {
                throw new FormatException("Key Identifier does not match the expected format.");
            }

            this.KeyType = keyType;
            this.IsLocal = isLocal;
            this.Name = matches.Groups["keyname"].Value;
            var keyVersion = matches.Groups["keyversion"].Value;
            if (!string.IsNullOrWhiteSpace(keyVersion))
            {
                this.Version = keyVersion;
            }

            this.VaultName = matches.Groups["vaultname"].Value;
            this.KeyIdentifier = keyIdentifier;
        }

        public bool IsLocal { get; private set; }

        public string Name { get; private set; }

        public string KeyType { get; set; }

        public string VaultName { get; private set; }

        public string Version { get; private set; }

        public string KeyIdentifier { get; private set; }

        [JsonIgnore]
        public KeyBundle KeyBundle { get; set; }

        #region Overrides of Object

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            var objAsKey = obj as Key;
            if (objAsKey == null)
            {
                return false;
            }

            return this.KeyIdentifier.Equals(objAsKey.KeyIdentifier);
        }

        #region Overrides of Object

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            return this.KeyIdentifier.GetHashCode();
        }

        #region Overrides of Object

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return this.KeyIdentifier;
        }

        #endregion

        #endregion

        #endregion
    }
}