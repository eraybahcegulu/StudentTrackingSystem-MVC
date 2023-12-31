﻿
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using StudentTrackingSystem.Models;
using StudentTrackingSystem.Utility;
using System.Data;
using System.Drawing;


namespace StudentTrackingSystem.Controllers
{
     
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;

        private readonly IFileProvider _fileProvider;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StudentController(IStudentRepository context, IFileProvider fileProvider, IWebHostEnvironment webHostEnvironment)
        {
            _studentRepository = context;
            _fileProvider = fileProvider;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize(Roles = UserRoles.Role_Teacher)]
        public IActionResult Index()
        {
            List<Student> objStudentList = _studentRepository.GetAll().ToList();
            return View(objStudentList);
        }

        [Authorize(Roles = UserRoles.Role_Teacher)]
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

        [Authorize(Roles = UserRoles.Role_Teacher)]
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
                    var existingStudent = _studentRepository.Get(u => u.Id == student.Id);

                    if (existingStudent == null)
                    {
                        return NotFound();
                    }

                    existingStudent.StudentNo = student.StudentNo;
                    existingStudent.StudentName = student.StudentName;
                    existingStudent.StudentSurname = student.StudentSurname;
                    existingStudent.MidtermExam = student.MidtermExam;
                    existingStudent.FinalExam = student.FinalExam;
                    existingStudent.Discontinuity = student.Discontinuity;
                    existingStudent.EMail = student.EMail;

                    _studentRepository.Update(existingStudent);
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


        [Authorize(Roles = UserRoles.Role_Teacher)]
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

        [Authorize(Roles = UserRoles.Role_Teacher)]
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

        [Authorize(Roles = UserRoles.Role_Teacher)]
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


        [Authorize(Roles = UserRoles.Role_Teacher)]

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





        [Authorize(Roles = UserRoles.Role_Student)]
        public IActionResult DownloadFile(string? fileName )
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                Student student = _studentRepository.Get(u => u.FileName == fileName);
                if (student == null)
                {
                    return NotFound();
                }

                student.FileName = null;
                _studentRepository.Update(student);
                _studentRepository.Save();

                TempData["danger"] = "The homework file you tried to download has been removed from the system.";
                TempData["danger"] = "The homework file you tried to download has been removed from the system.";
                return RedirectToAction("Index", "StudentHome");
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", fileName);
        }



        [Authorize(Roles = UserRoles.Role_Teacher)]
        [HttpGet]
        public IActionResult Upload(int id)
        {
   
            Student student = _studentRepository.Get(u => u.Id == id);
            if (student == null)
            {
                return NotFound();
            }


            var model = new FileUploadModel
            {
                Student = student
            };
            return View(model);
        }

        [Authorize(Roles = UserRoles.Role_Teacher)]
        [HttpPost, ActionName("Upload")]
        public async Task<IActionResult> UploadPOST(int? id, FileUploadModel model)
        {
            Student student = _studentRepository.Get(u => u.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            if (model.RarFile != null && model.RarFile.Length > 0)
            {

                long maxFileSizeBytes = 20000000;

                if (model.RarFile.Length > maxFileSizeBytes)
                {
                    TempData["danger"] = "File size is too large. You can upload a file up to 20 MB in size.";
                    return RedirectToAction("Index", "Student");
                }

                if (model.RarFile.Length == 0)
                {
                    TempData["danger"] = "Empty file cannot be uploaded.";
                    return RedirectToAction("Index", "Student");
                }

                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.RarFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.RarFile.CopyToAsync(stream);
                }

                student.FileName = uniqueFileName;

                _studentRepository.Update(student);
                _studentRepository.Save();

                TempData["success"] = "The file has been uploaded successfully!";
                return RedirectToAction("Index", "Student");
            }

            TempData["danger"] = "Empty file cannot be uploaded.";
            return RedirectToAction("Index", "Student");
        }

        [Authorize(Roles = UserRoles.Role_Teacher)]
        [HttpPost]
        public IActionResult DeleteHomework(int studentId)
        {
            Student student = _studentRepository.Get(u => u.Id == studentId);
            if (student == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(student.FileName))
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/rar", student.FileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            student.FileName = null;

            _studentRepository.Update(student);
            _studentRepository.Save();

            TempData["success"] = "The previously uploaded homework has been successfully removed."!;
            return RedirectToAction("Index", "Student");
        }




    }
}
