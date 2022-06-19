using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReportConstruct.Models;

namespace ReportConstruct.Controllers
{
    public class EmpTimeController : Controller
    {
        private readonly ModelContext _context;

        public EmpTimeController(ModelContext context)
        {
            _context = context;
        }

        // GET: EmpTime
        public async Task<IActionResult> Index()
        {

            //return View(await _context.Timetb.Where(m =>  m.Groupname == User.Claims.Last().Value.ToString()).ToListAsync());
            return View(await _context.Timetb.Where(m => m.Late != null && m.Names == User.Identity.Name).ToListAsync());
        }

    }
}
