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
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public bool isDisplay { get; set; }
		[ValidateNever]
		[Required]
		public int IdeaId { get; set; }
		[ValidateNever]
		[ForeignKey("IdeaId")]
		public Idea Idea { get; set; }
		public DateTime CreateDatetime { get; set; } = DateTime.Now;
        [ValidateNever]
        [Required]
        public string ApplicationUserId { get; set; }
        [ValidateNever]
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
