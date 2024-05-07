using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionStages.Models
{
    [Table("Groups")]
    public class Group
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; } = string.Empty;


    }
}
