using System.ComponentModel.DataAnnotations;

namespace LabFormProject.Models
{
    public class ClassInformationModel
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string? ClassName { get; set; }

        [Required]
        public int? StudentCount { get; set; }

        public string? Description { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}