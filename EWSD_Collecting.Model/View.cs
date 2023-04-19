using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EWSD_Collecting.Models
{
    public class View
    {
        [Key]
        public int Id { get; set; }
        public DateTime LastVisit { get; set; } = DateTime.Now;
        [Required]
        public int IdeaId { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        public int React { get; set; }
    }
}
