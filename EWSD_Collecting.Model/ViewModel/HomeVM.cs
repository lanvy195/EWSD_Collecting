using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWSD_Collecting.Models.ViewModel
{
	public class HomeVM
	{
		public IEnumerable<Idea> MostViews { get; set; }
		public IEnumerable<Idea> MostPopular { get; set; }
		public IEnumerable<Idea> LatestIdeas { get; set; }
		public IEnumerable<Comment> LatestComments { get; set; }

	}
}
