using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;

namespace Core.Excel
{
    public abstract class BaseGenerateSheet
    {
        public string SheetName { set; get; }

        public IWorkbook Workbook { get; set; }

        public virtual void GenSheet(ISheet sheet)
        {

        }
    }
}
