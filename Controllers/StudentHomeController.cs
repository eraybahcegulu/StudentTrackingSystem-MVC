using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentTrackingSystem.Models;
using StudentTrackingSystem.Utility;
using System.Security.Claims;

namespace StudentTrackingSystem.Controllers
{

    //[Authorize(Roles = UserRoles.Role_Student)]
    public class StudentHomeController : Controller
    {
        private readonly IStudentRepository _studentRepository;

        public StudentHomeController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public IActionResult Index()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Name);

            var student = _studentRepository.Get(s => s.EMail == userEmail);

            if (student != null)
            {
                return View(student);
            }
            else
            {
                ViewBag.UserEmail = userEmail;
                TempData["danger"] = "Your information has not been entered into the system yet.";
                return View("NotRegistered");
            }
        }
    }
}
