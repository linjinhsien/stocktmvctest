using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication2.Models;
using WebApplication2.partials;
using X.PagedList;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApplication2.Controllers
{
    public class StockjsonsController : Controller
    {
        private readonly StockContext _context;

        public StockjsonsController(StockContext context)
        {
            _context = context;
        }
        
    public async Task<IActionResult> ImportJson()
    {
        string jsonUrl = "https://openapi.twse.com.tw/v1/exchangeReport/BWIBBU_ALL";
        List<Stockjson> stocks = new List<Stockjson>();

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(jsonUrl);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                    try
                    {
                        stocks = JsonConvert.DeserializeObject<List<Stockjson>>(json);
                    }
                    catch (JsonSerializationException ex)
                    {
                        // 記錄錯誤
                        Console.WriteLine($"反序列化錯誤: {ex.Message}");
                        // 可能的話，跳過錯誤的項目或使用部分成功的數據
                    }
                   
            }
        }

            // 清空資料庫
            _context.Stockjsons.RemoveRange(_context.Stockjsons);
            await _context.SaveChangesAsync();

            // 插入新資料
            _context.Stockjsons.AddRange(stocks);
            await _context.SaveChangesAsync();

            return Json(new { message = "資料已成功匯入", redirectUrl = Url.Action("Index", "Stockjsons") });

        }
        // GET: Stockjsons
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 20; // 每頁顯示的項目數
            int pageNumber = (page ?? 1); // 如果沒有指定頁碼，默認為第1
            return View(_context.Stockjsons.ToPagedList(pageNumber, pageSize));
            
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
