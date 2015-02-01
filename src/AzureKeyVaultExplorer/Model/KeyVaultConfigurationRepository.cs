namespace AzureKeyVaultExplorer.Model
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Documents;

    using AzureKeyVaultExplorer.Interface;

    using Newtonsoft.Json;

    public class KeyVaultConfigurationRepository : IKeyVaultConfigurationRepository
    {
        private const string ConfigurationPath = "Configurations";

        private const string ConfigurationFileFormat = "{0}.configuration.json";

        private readonly List<KeyVaultConfiguration> allConfigurations;

        public KeyVaultConfigurationRepository()
        {
            if (!Directory.Exists(ConfigurationPath))
            {
                Directory.CreateDirectory(ConfigurationPath);
            }

            this.allConfigurations = new List<KeyVaultConfiguration>();
            this.GetAllConfigurationsFromStorage();
        }

        public IEnumerable<KeyVaultConfiguration> All
        {
            get
            {
                return this.allConfigurations; 
            }
        }

        public KeyVaultConfiguration Get(string keyVaultUrl)
        {
            return this.All.FirstOrDefault(c => c.AzureKeyVaultUrl.Equals(keyVaultUrl));
        }

        public async Task<bool> InsertOrUpdate(KeyVaultConfiguration keyVaultConfiguration)
        {
            var writeToFile = JsonConvert.SerializeObject(keyVaultConfiguration);
            var filePath = Path.Combine(
                ConfigurationPath,
                keyVaultConfiguration.VaultName,
                string.Format(ConfigurationFileFormat, keyVaultConfiguration.VaultName));
            
            var directoryPath = Path.Combine(ConfigurationPath, keyVaultConfiguration.VaultName);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (var file = File.Create(filePath))
            {
                using (var streamWriter = new StreamWriter(file))
                {
                    await streamWriter.WriteAsync(writeToFile);
                    await streamWriter.FlushAsync();
                    this.allConfigurations.Remove(keyVaultConfiguration);
                    this.allConfigurations.Add(keyVaultConfiguration);
                    return true;
                }
            }
        }

        public bool Delete(KeyVaultConfiguration keyVaultConfiguration)
        {
            var configurarionDirectory = Path.Combine(ConfigurationPath, keyVaultConfiguration.VaultName);
            Directory.Delete(configurarionDirectory, true);
            this.allConfigurations.Remove(keyVaultConfiguration);
            return true;
        }

        private List<KeyVaultConfiguration> GetAllConfigurationsFromStorage()
        {
            var pattern = string.Format(ConfigurationFileFormat, "*");
            this.allConfigurations.Clear();
            var allFiles = Directory.GetFiles(ConfigurationPath, pattern, SearchOption.AllDirectories);
            foreach (var file in allFiles)
            {
                var fileContents = File.ReadAllText(file);
                var configuration = JsonConvert.DeserializeObject<KeyVaultConfiguration>(fileContents);
                this.allConfigurations.Add(configuration);
            }

            return this.allConfigurations;
        }
    }
}