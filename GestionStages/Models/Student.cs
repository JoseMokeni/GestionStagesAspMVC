using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GestionStages.Models
{
    [Table("Students")]
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        [Column(TypeName = "nvarchar(60)")]
        public string Email { get; set; } = string.Empty;

        /*Nullable field*/
        [Display(Name = "CV")]
        [Column(TypeName = "nvarchar(255)")]
        public string? CV { get; set; }

        [Required]
        public int GroupId { get; set; }

        [ForeignKey("GroupId")]

        public Group Group { get; set; }



    }
}
