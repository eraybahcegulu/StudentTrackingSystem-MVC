
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentTrackingSystem.Models;
using System.Data;
using System.Drawing;

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

		public IActionResult AddUpdate(int? id)
		{
			if (id == null || id == 0)
			{
				return View();
			}
			else
			{
				Student? studentDb = _studentRepository.Get(u => u.Id == id);
				if (studentDb == null)
				{
					return NotFound();
				}
				return View(studentDb);
			}


		}

		[HttpPost]
        public IActionResult AddUpdate(Student student)
        {
            if (ModelState.IsValid)
            {
                if (student.Id == 0)
                {
                    bool isStudentIdExists = _studentRepository.GetAll().Any(s => s.StudentNo == student.StudentNo);

                    if (isStudentIdExists)
                    {
                        TempData["danger"] = "This student number is already registered in the system!";
                        return RedirectToAction("Index", "Student");
                    }

                    _studentRepository.Add(student);
                    TempData["success"] = "Student added successfully!";
                }
                else
                {

                    _studentRepository.Update(student);
                    TempData["success"] = "Student information updated!";
                }

                _studentRepository.Save();
                return RedirectToAction("Index", "Student");
            }
            else
            {
                return View();
            }
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


        public IActionResult Message(int? id)
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

        [HttpPost, ActionName("Message")]
        public IActionResult MessagePOST(int? id, string message)
        {

            Student? student = _studentRepository.Get(u => u.Id == id);
            if (student == null)
            {
                return NotFound(); 
            }

            student.Message = message;
            _studentRepository.Update(student);
            _studentRepository.Save();
            TempData["success"] = "Message sent successfully!";
            return RedirectToAction("Index", "Student");
        }
    }
}
