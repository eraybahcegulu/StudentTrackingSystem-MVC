using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        [Range(1, long.MaxValue, ErrorMessage = "Student number cannot be 0.")]
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
		[Range(0, 100, ErrorMessage = "The exam result can be between 0-100.")]
		public int MidtermExam { get; set; }
		[Range(0, 100, ErrorMessage = "The exam result can be between 0-100.")]
		public int FinalExam { get; set; }

		[Range(0, 100, ErrorMessage = "Discontinuity can be between 0-100.")]
		public int Discontinuity { get; set; }

        [MaxLength(100)]
        [DisplayName("Last Message")]
        public string? Message { get; set; }
    }
}