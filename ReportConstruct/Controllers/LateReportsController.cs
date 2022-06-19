using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReportConstruct.Models;

namespace ReportConstruct.Controllers
{
    public class LateReportsController : Controller
    {
        private readonly ModelContext _context;
        private IHostingEnvironment Environment;
        public LateReportsController(ModelContext context, IHostingEnvironment _environment)
        {
            _context = context;
            Environment = _environment;
        }

        public ErrorViewModel ErrorViewModel
        {
            get => default;
            set
            {
            }
        }

        // GET: LateReports
        public async Task<IActionResult> Index(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lateReports = _context.LateReports
                .Where(s => s.Id_late_day == id).ToList();

            if (lateReports == null)
            {
                return NotFound();
            }
            ViewData["id"] = id;

            return View(lateReports);
        }


        [HttpPost]
        public IActionResult Upload(List<IFormFile> postedFiles, LateReports late, string id_late, string comments)
        {
            
            if (postedFiles.Count<=0)
            {
                TempData["Message"] = "Error, File is not chosen";
                return RedirectToAction("Index", "LateReports", new { id = late.Id });
            }
            else
            {
                string path = Path.Combine(this.Environment.WebRootPath, "Uploaded");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                List<string> uploadedFiles = new List<string>();
                foreach (IFormFile postedFile in postedFiles)
                {
                    string fileName = Path.GetFileName(postedFile.FileName);
                    using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                        uploadedFiles.Add(fileName);
                        //ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                    }


                    late.Id_late_day = late.Id;
                    late.Id = "";
                    late.Names = fileName;
                    late.Path = "/Uploaded/" + fileName;
                    late.Comments = comments;

                    _context.Add(late);
                    _context.SaveChangesAsync();
                }

                return RedirectToAction("Index", "LateReports", new { id = late.Id_late_day });
            }
        }

        private bool LateReportsExists(string id)
        {
            return _context.LateReports.Any(e => e.Id == id);
        }






    }
}
