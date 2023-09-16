using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentTrackingSystem.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public long StudentNo { get; set; }

        [Required]
        public string StudentName { get; set; }
        [Required]
        public string StudentSurname { get; set; }
    }
}
