using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P190012_Creasoft.Creasoft
{
    interface IDataBase
    {
        string ConnectionString { set; get; }

        void Open();
        void ExpedovatZbozi(List<Zbozi> Goods);
    }
}
