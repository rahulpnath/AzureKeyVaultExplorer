namespace AzureKeyVaultExplorer.Model
{
    using System;

    using Microsoft.KeyVault.Client;

    public class Key
    {
        public Key(KeyBundle keyBundle, bool isLocal)
        {
            if (keyBundle == null || keyBundle.Key == null || string.IsNullOrWhiteSpace(keyBundle.Key.Kid))
            {
                throw new ArgumentNullException("keyBundle");
            }

            this.KeyBundle = keyBundle;
            this.IsLocal = isLocal;
        }

        public bool IsLocal { get; set; }

        public string Name { get; set; }

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

            return this.KeyBundle.Key.Kid.Equals(objAsKey.KeyBundle.Key.Kid);
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
            return this.KeyBundle.Key.Kid.GetHashCode();
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
            return this.KeyBundle.Key.Kid;
        }

        #endregion

        #endregion

        #endregion
    }
}