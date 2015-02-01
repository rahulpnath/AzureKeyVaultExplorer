namespace AzureKeyVaultExplorer.Model
{
    public class KeyVaultConfiguration
    {
        public string VaultName { get; set; }

        public string AzureKeyVaultUrl { get; set; }

        public string ADApplicationClientId { get; set; }

        public string ADApplicationSecret { get; set; }

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
            var objAsConfiguration = obj as KeyVaultConfiguration;
            if (objAsConfiguration == null)
            {
                return false;
            }

            return this.AzureKeyVaultUrl.Equals(objAsConfiguration.AzureKeyVaultUrl);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            return this.AzureKeyVaultUrl.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return this.VaultName;
        }

        #endregion
    }
}