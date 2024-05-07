using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GestionStages.Models
{
    [Table("Companies")]
    public class Company
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
    }
}
