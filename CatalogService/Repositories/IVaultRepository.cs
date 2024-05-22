using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.Repositories
{
    public interface IVaultRepository
    {
        Task<string> GetSecretAsync(string path);

    }
}