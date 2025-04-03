using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    public class ClassInformationModel
    {
        private static int IDCounter = 1;
        public int ID { get; private set; }
        [Required]
        public string? ClassName { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int StudentCount { get; set; }
        [Required]
        public string? Description { get; set; }

        public void SetID()
        {
            ID=IDCounter++;
        }
        public void DeclareID()
        {
            ID = IDCounter--;
        }
    }
}
