namespace AzureKeyVaultExplorer.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AzureKeyVaultExplorer.Model;

    public interface IKeyRepository
    {
        IEnumerable<Key> All { get; }

        Key Get(string name);

        Task<bool> InsertOrUpdate(Key key);

        bool Delete(Key key);
    }
}