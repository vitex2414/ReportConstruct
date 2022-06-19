using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReportConstruct.Models;

namespace ReportConstruct.Controllers
{
    [Authorize]
    public class QueriesController : Controller
    {
        private readonly ModelContext _context;

        private readonly IConfiguration _configuration;

        public QueriesController(ModelContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        public ReportFields ReportFields
        {
            get => default;
            set
            {
            }
        }

        // GET: Queries
        public async Task<IActionResult> Index()
        {
            return View(await _context.Queries.ToListAsync());
        }

        // GET: Queries/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var queries = await _context.Queries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (queries == null)
            {
                return NotFound();
            }

            return View(queries);
        }

        // GET: Queries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Queries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> SendQuery([Bind("Query,Id,Name")] Queries queries, Params[] parameters, ReportFields[] reportFields)
        {
            // Queries query = await _context.Queries.FirstOrDefaultAsync(i => i.Id == id);

            queries.ReportFields = reportFields.ToList();
            queries.Params = parameters.ToList();
            queries.ConnectionString = _configuration["SecureConnection"];
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "report.xlsx";

            return File(queries.GetExcel(), contentType, fileName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Query,Id,Name")] Queries queries, Params[] parameters, ReportFields[] reportFields)
        {

            if (ModelState.IsValid)
            {
                _context.Add(queries);
                await _context.SaveChangesAsync();
                queries = _context.Queries.FirstOrDefault(f => f.Name == queries.Name);
                for (int i = 0; i < parameters.Length; i++)
                {
                    parameters[i].QueryId = queries.Id;
                    _context.Add(parameters[i]);
                }
                for (int i = 0; i < reportFields.Length; i++)
                {
                    reportFields[i].QueryId = queries.Id;
                    _context.Add(reportFields[i]);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(queries);
        }

        // GET: Queries/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var queries = await _context.Queries.FindAsync(id);

            if (queries == null)
            {
                return NotFound();
            }
            ViewBag.Params = _context.Params.Where(p => p.QueryId == id).ToList();
            ViewBag.Fields = _context.ReportFields.Where(f => f.QueryId == id).ToList();
            return View(queries);
        }

        // POST: Queries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Query,Id,Name")] Queries queries, Params[] parameters, ReportFields[] reportFields)
        {
            if (id != queries.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(queries);
                    await _context.SaveChangesAsync();
                    _context.Params.RemoveRange(_context.Params.Where(p => p.QueryId == id).ToList());
                    _context.ReportFields.RemoveRange(_context.ReportFields.Where(p => p.QueryId == id).ToList());

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(parameters[i].ParamName))
                        {
                            parameters[i].QueryId = id;
                            _context.Add(parameters[i]);
                        }
                    }
                    for (int i = 0; i < reportFields.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(reportFields[i].QueryField))
                        {
                            reportFields[i].QueryId = id;
                            _context.Add(reportFields[i]);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QueriesExists(queries.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Queries");
            }
            return View(queries);
        }

        // GET: Queries/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var queries = await _context.Queries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (queries == null)
            {
                return NotFound();
            }

            return View(queries);
        }

        // POST: Queries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var queries = await _context.Queries.FindAsync(id);
            _context.Queries.Remove(queries);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QueriesExists(string id)
        {
            return _context.Queries.Any(e => e.Id == id);
        }
    }
}
