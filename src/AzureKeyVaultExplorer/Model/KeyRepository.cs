namespace AzureKeyVaultExplorer.Model
{
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

        public async Task<bool> Add(Key key)
        {
            var writeToFile = JsonConvert.SerializeObject(key);
            var filePath = Path.Combine(this.keyPath, string.Format(KeyFileFormat, key.Name));
            await WriteKeyToFile(filePath, writeToFile);
            this.UpdateKeyInLocalList(key);
            return true;
        }

        public bool Delete(Key key)
        {
            var keyFilePath = Path.Combine(this.keyPath, string.Format(KeyFileFormat, key.Name));
            File.Delete(keyFilePath);
            this.allKeys.Remove(key);
            return true;
        }

        private static async Task<bool> WriteKeyToFile(string filePath, string writeToFile)
        {
            using (var file = File.Create(filePath))
            {
                using (var streamWriter = new StreamWriter(file))
                {
                    await streamWriter.WriteAsync(writeToFile);
                    await streamWriter.FlushAsync();
                    return true;
                }
            }
        }

        private static Key GetKeyFromFileContents(string file)
        {
            var fileContents = File.ReadAllText(file);
            var key = JsonConvert.DeserializeObject<Key>(fileContents);
            return key;
        }

        private void UpdateKeyInLocalList(Key key)
        {
            this.allKeys.Remove(key);
            this.allKeys.Add(key);
        }

        private void GetAllKeysFromStorage()
        {
            this.allKeys.Clear();
            var allFiles = this.GetAllFilesMatchingKeyFileFormat();
            this.PopulateKeysFromAllFiles(allFiles);
        }

        private void PopulateKeysFromAllFiles(IEnumerable<string> allFiles)
        {
            foreach (var file in allFiles)
            {
                var key = GetKeyFromFileContents(file);
                this.allKeys.Add(key);
            }
        }

        private IEnumerable<string> GetAllFilesMatchingKeyFileFormat()
        {
            var pattern = string.Format(KeyFileFormat, "*");
            var allFiles = Directory.GetFiles(this.keyPath, pattern, SearchOption.AllDirectories);
            return allFiles;
        }
    }
}