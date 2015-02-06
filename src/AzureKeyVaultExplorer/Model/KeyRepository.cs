namespace AzureKeyVaultExplorer.Model
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using AzureKeyVaultExplorer.Interface;

    using Newtonsoft.Json;

    public class KeyRepository : IKeyRepository
    {
        private const string KeyFileFormat = "{0}.key.json";

        private readonly string keyPath = @"Configurations\{0}";

        private readonly string vaultName;

        private readonly List<Key> allKeys;

        public KeyRepository(string vaultName)
        {
            this.vaultName = vaultName;
            this.keyPath = string.Format(this.keyPath, vaultName);
            if (!Directory.Exists(this.keyPath))
            {
                Directory.CreateDirectory(this.keyPath);
            }

            this.allKeys = new List<Key>();
            this.GetAllKeysFromStorage();
        }

        public IEnumerable<Key> All
        {
            get
            {
                return this.allKeys;
            }
        }

        public Key Get(string keyIdentifier)
        {
            return this.All.FirstOrDefault(k => k.KeyIdentifier.Equals(keyIdentifier));
        }

        [Obsolete("Keys should not be updated but only created or deleted")]
        public async Task<bool> InsertOrUpdate(Key key)
        {
            var writeToFile = JsonConvert.SerializeObject(key);
            var filePath = Path.Combine(
                this.keyPath,
                string.Format(KeyFileFormat, key.Name));

            using (var file = File.Create(filePath))
            {
                using (var streamWriter = new StreamWriter(file))
                {
                    await streamWriter.WriteAsync(writeToFile);
                    await streamWriter.FlushAsync();
                    this.allKeys.Remove(key);
                    this.allKeys.Add(key);
                    return true;
                }
            }
        }

        public bool Delete(Key key)
        {
            var keyFilePath = Path.Combine(this.keyPath, string.Format(KeyFileFormat, key.Name));
            File.Delete(keyFilePath);
            this.allKeys.Remove(key);
            return true;
        }

        private void GetAllKeysFromStorage()
        {
            var pattern = string.Format(KeyFileFormat, "*");
            this.allKeys.Clear();
            var allFiles = Directory.GetFiles(this.keyPath, pattern, SearchOption.AllDirectories);
            foreach (var file in allFiles)
            {
                var fileContents = File.ReadAllText(file);
                var key = JsonConvert.DeserializeObject<Key>(fileContents);
                this.allKeys.Add(key);
            }
        }
    }
}