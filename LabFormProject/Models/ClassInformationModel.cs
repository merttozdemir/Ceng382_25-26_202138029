using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    /*I took lots of the parts of this class from the chat gpt except DeclareID*/
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
        public void DeclareID(int id)
        {   
            if (id+1 == IDCounter)
            {
                IDCounter--;
            }
        }
    }
}
