using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWSD_Collecting.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        public int? DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        [ValidateNever]
        public Department Department { get; set; }
        public string? Path { get; set; }
        [ValidateNever]
        public string FullName { get; set; }
        public bool isDelete { get; set; }
        public bool isQA { get; set; }
    }
}
