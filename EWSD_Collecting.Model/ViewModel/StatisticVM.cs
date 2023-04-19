using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWSD_Collecting.Models.ViewModel
{
    public class StatisticVM
    {
        public int Total_Ideas_without_comment { get; set; }
        public IEnumerable<Idea> List_Ideas_without_comment { get; set; }
        public int Total_Ideas_ANONYMOUS { get; set; }
        public IEnumerable<Idea> List_Ideas_ANONYMOUS { get; set; }
        public int Total_COMMENT_ANONYMOUS { get; set; }
        public IEnumerable<Comment> List_COMMENT_ANONYMOUS { get; set; }
    }
}
