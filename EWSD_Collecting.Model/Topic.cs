using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EWSD_Collecting.Models
{
    public class Topic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime CreateDatetime { get; set; } = DateTime.Now;
        public DateTime ClosureDate { get; set; }
        public DateTime FinalClosureDate { get; set; }
        [ValidateNever]
        public ICollection<Idea> Idea { get; set; }
    }
}
