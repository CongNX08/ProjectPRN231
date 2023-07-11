using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class Paging
    {
        public int PageIndex { get; set; }
        public int LastPage { get; set; }
        public int TotalRecord { get; set; }
        public int PageSize { get; set; }
        public Paging(int PageSize, int TotalRow, int PageIndex)
        {
            TotalRecord = TotalRow;
            this.PageSize = PageSize;
            if (TotalRow > 0)
            {
                int LastPageCal = TotalRow / PageSize;
                if (TotalRow % PageSize != 0)
                {
                    LastPageCal += 1;
                }
                LastPage = LastPageCal;
            }
            LastPage = 1;
            PageIndex = 1;
        }

    }
}
