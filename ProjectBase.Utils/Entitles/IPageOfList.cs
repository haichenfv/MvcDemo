using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBase.Utils.Entitles
{
    public interface IPageOfList
    {
        int PageIndex { get; set; }

        int PageSize { get; set; }

        int PageTotal { get; }

        long RecordTotal { get; set; }
    }

    public interface IPageOfList<T> : IPageOfList, IList<T>
    {
    }
}
