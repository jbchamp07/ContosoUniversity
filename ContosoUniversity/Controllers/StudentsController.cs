using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Entities;
using ContosoUniversity.Seedwork;
using Microsoft.Extensions.Hosting.Internal;
using ContosoUniversity.Models;

namespace ContosoUniversity.Controllers
{
    public class StudentsController : Controller
    {
        private IStudentsRepository Repository { get; }
        public StudentsController(IStudentsRepository repository)
        {
            Repository = repository;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
              return await Repository.ListAll() != null ? 
                          View(await Repository.ListAll()) :
                          Problem("Entity set 'SchoolContext.Students'  is null.");
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await Repository.ListAll() == null)
            {
                return NotFound();
            }

            var student = await Repository.GetById(id.Value);
            if (student == null)
            {
                return NotFound();
            }
            StudentViewModel studentViewModel = new StudentViewModel()
            {
                EnrollmentDate = student.EnrollmentDate,
                FirstMidName = student.FirstMidName,
                LastName = student.LastName,
                Photo = await ConverByteToFile(student.Photo)

            };
            return View(studentViewModel);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentViewModel studentViewModel)
        {
            Student student = new Student()
            {
                EnrollmentDate = studentViewModel.EnrollmentDate,
                FirstMidName = studentViewModel.FirstMidName,
                LastName = studentViewModel.LastName,
                Photo = await ConverFileToByte(studentViewModel.Photo),
            };
            if (ModelState.IsValid)
            {
                await Repository.Insert(student);
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await Repository.ListAll() == null)
            {
                return NotFound();
            }

            var student = await Repository.GetById(id.Value);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LastName,FirstMidName,EnrollmentDate,Id")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await Repository.Update(student);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await Repository.ListAll() == null)
            {
                return NotFound();
            }

            var student = await Repository.GetById(id.Value);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await Repository.ListAll() == null)
            {
                return Problem("Entity set 'SchoolContext.Students'  is null.");
            }
            var student = await Repository.GetById(id);
            if (student != null)
            {
                await Repository.Delete(student);
            }
            
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> StudentExists(int id)
        {
          return await Repository.DoesExist(id);
        }


        private async Task<byte[]> ConverFileToByte(IFormFile file)
        {
            MemoryStream memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
        private async Task<IFormFile> ConverByteToFile(byte[] tab)
        {
            MemoryStream memoryStream = new MemoryStream(tab);
            IFormFile formFile = new FormFile(memoryStream, 0, tab.Length, "test1", "test2")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/octet-stream"
            };
            return formFile;
        }


    }
}
