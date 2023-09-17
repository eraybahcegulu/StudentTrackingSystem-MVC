using Microsoft.AspNetCore.Http;
using System.ComponentModel;

namespace StudentTrackingSystem.Models
{
    public class FileUploadModel
    {
        [DisplayName("Homework File")]
        public IFormFile? RarFile { get; set; }

        public Student Student { get; set; } = new Student();
    }
}