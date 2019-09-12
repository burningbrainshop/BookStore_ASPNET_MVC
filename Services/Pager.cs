using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Services
{
    public class Pager
    {
        public int NowPage { get; set; }
        public int MaxPage { get; set; }
        public int PageSize { get { return 5; } }

        public Pager()
        {
            this.NowPage = 1;
        }

        public Pager(int page)
        {
            this.NowPage = page;
        }

        public void ValidatePage()
        {
            if (this.NowPage < 1)
                this.NowPage = 1;

            if (this.NowPage > this.MaxPage)
                this.NowPage = this.MaxPage;

            if (this.MaxPage.Equals(0))
                this.MaxPage = 1;
        }
    }
}