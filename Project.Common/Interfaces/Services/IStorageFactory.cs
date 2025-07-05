using Project.Common.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Interfaces.Services
{
    public interface IStorageFactory
    {
        IEnumerable<IDataStorage> GetRetrievalOrder();

    }
}
