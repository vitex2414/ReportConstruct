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
    public class BrunchDController : Controller
    {
        private readonly ModelContext _context;

        public BrunchDController(ModelContext context)
        {
            _context = context;
        }

        // GET: BrunchD
        public async Task<IActionResult> Index()
        {
            return View(await _context.Timetb.Where(m =>  m.Groupname == User.Claims.Last().Value.ToString()).ToListAsync());
        }

      
    }
}
