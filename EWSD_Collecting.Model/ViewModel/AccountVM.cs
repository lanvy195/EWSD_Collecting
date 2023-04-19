using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EWSD_Collecting.Models.ViewModel
{
    public class AccountVM
    {
        public ApplicationUser account { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        [ValidateNever]
        public string Password { get; set; }
    }
}
