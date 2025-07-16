using System.Data;
using DemoMVC.Data;
using DemoMVC.Models.Entities;
using DemoMVC.Models.Process;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using X.PagedList.Extensions;

namespace DemoMVC.Controllers
{
    public class PersonController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ExcelProcess _excelProcess;
        private readonly AutoGenerateCode _autoGenerateCode;
        public async Task<IActionResult> Index(int? page, int? PageSize)
        {
            ViewBag.PageSize = new List<SelectListItem>
            {
                new SelectListItem { Text = "3", Value = "3" },
                new SelectListItem { Text = "5", Value = "5" },
                new SelectListItem { Text = "10", Value = "10" },
                new SelectListItem { Text = "15", Value = "15" },
                new SelectListItem { Text = "25", Value = "25" },
                new SelectListItem { Text = "50", Value = "50" }
            };
            int pageSize = (PageSize ?? 3);
            ViewBag.psize = pageSize;
            var model = _context.Person.ToList().ToPagedList(page ?? 1, pageSize);
            return View(model);
        }

        public PersonController(ApplicationDbContext context)
        {
            _context = context;
            _excelProcess = new ExcelProcess();
            _autoGenerateCode = new AutoGenerateCode();
        }
        public async Task<IActionResult> Detail(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var person = await _context.Person
            .FirstOrDefaultAsync(m => m.PersonId == id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }
        public IActionResult Create()
        {
            AutoGenerateCode autoGenerateCode = new AutoGenerateCode();
            // Generate a new PersonId
            var Person = _context.Person.OrderByDescending(p => p.PersonId).FirstOrDefault();
            var PersonId = Person == null ? "PS000" : Person.PersonId;
            var newPersonId = autoGenerateCode.GenerateId(PersonId);
            var newPerson = new Person
            {
                PersonId = newPersonId,
                FullName = string.Empty,
                Address = string.Empty,
                Email = string.Empty
            };
            return View(newPerson);
        }
        public IActionResult Uploads()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Uploads(IFormFile file)
        {
            if (file != null)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                if (fileExtension != ".xls" && fileExtension != ".xlsx")
                {
                    ModelState.AddModelError("File", "Please upload a valid Excel file.");
                    // Process the uploaded file
                }
                else
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + fileExtension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory() + "/Uploads/Excels", fileName);
                    var filelocation = new FileInfo(filePath).ToString();
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        var dt = _excelProcess.ExcelToDataTable(filelocation);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            var ps = new Person
                            {
                                PersonId = dt.Rows[i][0].ToString(),
                                FullName = dt.Rows[i][1].ToString(),
                                Address = dt.Rows[i][2].ToString(),
                                Email = dt.Rows[i][3].ToString(),
                            };
                            _context.Add(ps);
                        }
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonId,FullName,Address")] Person person)
        {
            if (ModelState.IsValid)
            {
                // Generate a new PersonId
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }
        public IActionResult Download()
        {
            var fileName = "Your FileName" + ".xlsx";
            using(ExcelPackage excelPackage = new ExcelPackage())
            {
                var workSheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells["A1"].Value = "PersonId";
                workSheet.Cells["B1"].Value = "FullName";
                workSheet.Cells["C1"].Value = "Address";

                var personList = _context.Person.ToList();
                workSheet.Cells["A2"].LoadFromCollection(personList);
                var stream = new MemoryStream(excelPackage.GetAsByteArray());

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("PersonId,FullName,Address")] Person person)
        {
            if (id != person.PersonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.PersonId))
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
            return View(person);
        }
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Person == null)
            {
                return NotFound();
            }
            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var person = await _context.Person.FindAsync(id);
            if (person != null)
            {
                _context.Person.Remove(person);    
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(string id)
        {
            return _context.Person.Any(e => e.PersonId == id);
        }
    }
}
