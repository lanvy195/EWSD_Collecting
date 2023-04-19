using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWSD_Collecting.Models.ViewModel
{
	public class TopicVM
	{
		public Topic topic { get; set; }
		public IEnumerable<Idea> idea { get; set; }
	}
}
