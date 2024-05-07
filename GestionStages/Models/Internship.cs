using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionStages.Models
{
    [Table("Internships")]
    public class Internship
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        [Column(TypeName = "nvarchar(100)")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Description")]
        [Column(TypeName = "text")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Start Date")]
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Status")]
        [Column(TypeName = "nvarchar(50)")]
        public string Status { get; set; } = string.Empty;

        [Required]
        public int CompanyId { get; set; }
        [Required]
        public int StudentId { get; set; }

        [Required]
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }

        [Required]
        [ForeignKey("StudentId")]
        public Student Student { get; set; }



    }
}
