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
		[Required(ErrorMessage = "Student number cannot be left blank.")]
		[DisplayName("Student No")]
		public long? StudentNo { get; set; }
        [MaxLength(25)]
		[DisplayName("Student Name")]
		[RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]*$", ErrorMessage = "Student surname can only contain letters.")]
		[Required(ErrorMessage = "Student name cannot be left blank.")]
        public string StudentName { get; set; }
        [MaxLength(25)]
		[DisplayName("Student Surname")]
		[RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]*$", ErrorMessage = "Student surname can only contain letters.")]
		[Required(ErrorMessage = "Student surname cannot be left blank.")]
        public string StudentSurname { get; set; }
    }
}
