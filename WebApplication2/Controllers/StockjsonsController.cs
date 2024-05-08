using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.partials;

namespace WebApplication2.Controllers
{
    public class StockjsonsController : Controller
    {
        private readonly StockContext _context;

        public StockjsonsController(StockContext context)
        {
            _context = context;
        }

        // GET: Stockjsons
        public async Task<IActionResult> Index()
        {
            return View(await _context.Stockjsons.ToListAsync());
        }

        // GET: Stockjsons/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockjson = await _context.Stockjsons
                .FirstOrDefaultAsync(m => m.Code == id);
            if (stockjson == null)
            {
                return NotFound();
            }

            return View(stockjson);
        }

        // GET: Stockjsons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stockjsons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Code,Name,Peratio,DividendYield,Pbratio")] Stockjson stockjson)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stockjson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stockjson);
        }

        // GET: Stockjsons/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockjson = await _context.Stockjsons.FindAsync(id);
            if (stockjson == null)
            {
                return NotFound();
            }
            return View(stockjson);
        }

        // POST: Stockjsons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("Code,Name,Peratio,DividendYield,Pbratio")] Stockjson stockjson)
        {
            if (id != stockjson.Code)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stockjson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockjsonExists(stockjson.Code))
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
            return View(stockjson);
        }

        // GET: Stockjsons/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockjson = await _context.Stockjsons
                .FirstOrDefaultAsync(m => m.Code == id);
            if (stockjson == null)
            {
                return NotFound();
            }

            return View(stockjson);
        }

        // POST: Stockjsons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var stockjson = await _context.Stockjsons.FindAsync(id);
            if (stockjson != null)
            {
                _context.Stockjsons.Remove(stockjson);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockjsonExists(short id)
        {
            return _context.Stockjsons.Any(e => e.Code == id);
        }
    }
}
