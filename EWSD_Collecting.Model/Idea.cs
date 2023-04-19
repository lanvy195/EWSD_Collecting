using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EWSD_Collecting.Models
{
    public class Idea
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Title { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        public string? Path { get; set; }
        public bool isDisplay { get; set; }
        public bool isAgree { get; set; }
        public int Views { get; set; }
        public int Comments { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public DateTime CreateDatetime { get; set; } = DateTime.Now;
        [Required]
        public int CategoryId { get; set; }
        [ValidateNever]
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [Required]
        public int TopicId { get; set; }
        [ValidateNever]
        [ForeignKey("TopicId")]
        public Topic Topic { get; set; }
        [ValidateNever]
        public string ApplicationUserId { get; set; }
        [ValidateNever]
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
    }

}
