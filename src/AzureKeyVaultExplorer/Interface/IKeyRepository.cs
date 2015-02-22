namespace AzureKeyVaultExplorer.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AzureKeyVaultExplorer.Model;

    public interface IKeyRepository
    {
        Task<IEnumerable<Key>> GetAll();

        Task<Key> Get(string name);

        Task<bool> Add(Key key);

        Task<bool> Delete(Key key);
    }
}