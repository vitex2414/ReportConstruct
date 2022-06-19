using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReportConstruct.Models;

namespace ReportConstruct.Controllers
{
    public class TimetbsController : Controller
    {
        private readonly ModelContext _context;

        public TimetbsController(ModelContext context)
        {
            _context = context;

        }

        public Timetb Timetb
        {
            get => default;
            set
            {
            }
        }

        // GET: Timetbs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Timetb.ToListAsync());
        }

       

   

        // GET: Timetbs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timetb = await _context.Timetb.FindAsync(id);
            if (timetb == null)
            {
                return NotFound();
            }
            return View(timetb);
        }

        // POST: Timetbs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Dates,Names,Timecome,Late,Lunchleave,Lunchcome,Timeleave,Groupname")] Timetb timetb)
        {
            if (id != timetb.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timetb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimetbExists(timetb.Id))
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
            return View(timetb);
        }

      
        private bool TimetbExists(string id)
        {
            return _context.Timetb.Any(e => e.Id == id);
        }






        public async Task<IActionResult> VerifyRep(string id, [Bind("Id, Dates, Names, Timecome, Late, Lunchleave, Lunchcome, Timeleave, IsChecked")] Timetb timetb)
        {

            if (id != timetb.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                timetb = await _context.Timetb.FindAsync(id);

                try
                {
                    timetb.Id = id;
                    timetb.IsChecked = true;
                    _context.Update(timetb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimetbExists(timetb.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Timetbs");
            }
            return RedirectToAction("Index", "Timetbs");
        }



       
    }
}
