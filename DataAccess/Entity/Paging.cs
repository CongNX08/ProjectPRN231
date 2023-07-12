using System;
using System.Collections.Generic;
using System.Drawing;
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
        public int PageSize { get; set; } = 10;
        public Paging(int PageSize, int TotalRow, int PageIndex)
        {
            this.TotalRecord = TotalRow;
            this.PageSize = PageSize;
            this.PageIndex = PageIndex;
            if (TotalRow > 0)
            {
                int LastPageCal = (TotalRow / PageSize) > 0 ? (TotalRow / PageSize) : 0;
                if (TotalRow % PageSize != 0)
                {
                    LastPageCal += 1;
                }
                this.LastPage = LastPageCal;
            }
        }

        public Paging(int TotalRow, int PageIndex)
        {
            this.TotalRecord = TotalRow;
            this.PageIndex = PageIndex;
            if (TotalRow > 0)
            {
                int LastPageCal = (TotalRow / PageSize) > 0 ? (TotalRow / PageSize) : 0;
                if (TotalRow % PageSize != 0)
                {
                    LastPageCal += 1;
                }
                this.LastPage = LastPageCal;
            }
        }

    }
}
