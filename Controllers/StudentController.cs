
using Microsoft.AspNetCore.Mvc;
using StudentTrackingSystem.Models;
using System.Data;

namespace StudentTrackingSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;

        public StudentController(IStudentRepository context)
        {
            _studentRepository = context;
        }
        public IActionResult Index()
        {
            List<Student> objStudentList = _studentRepository.GetAll().ToList();
            return View(objStudentList);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Student student)
        {
            if (ModelState.IsValid)
            {
                _studentRepository.Add(student);
                _studentRepository.Save();
                TempData["success"] = "Student added successfully!";
                return RedirectToAction("Index", "Student");
            }
            return View();
        }

        public IActionResult Update(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Student? studentDB = _studentRepository.Get(u => u.Id == id);
            if (studentDB == null)
            {
                return NotFound();
            }
            return View(studentDB);
        }

        [HttpPost]
        public IActionResult Update(Student student)
        {
            if (ModelState.IsValid)
            {
                _studentRepository.Update(student);
                _studentRepository.Save();
                TempData["success"] = "Student information updated!";
                return RedirectToAction("Index", "Student");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Student? studentDB = _studentRepository.Get(u => u.Id == id);
            if (studentDB == null)
            {
                return NotFound();
            }
            return View(studentDB);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Student? student = _studentRepository.Get(u => u.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            _studentRepository.Delete(student);
            _studentRepository.Save();
            TempData["success"] = "Student deleted successfully!";
            return RedirectToAction("Index", "Student");
        }
    }
}
