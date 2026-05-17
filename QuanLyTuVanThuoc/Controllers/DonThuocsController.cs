using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyTuVanThuoc.Models;

namespace QuanLyTuVanThuoc.Controllers
{
    public class DonThuocsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonThuocsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DonThuocs
        public async Task<IActionResult> Index()
        {
            return View(await _context.DonThuocs.ToListAsync());
        }

        // GET: DonThuocs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donThuoc = await _context.DonThuocs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (donThuoc == null)
            {
                return NotFound();
            }

            return View(donThuoc);
        }

        // GET: DonThuocs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DonThuocs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenBenhNhan,NgayKe,DaTuVan")] DonThuoc donThuoc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donThuoc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(donThuoc);
        }

        // GET: DonThuocs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donThuoc = await _context.DonThuocs.FindAsync(id);
            if (donThuoc == null)
            {
                return NotFound();
            }
            return View(donThuoc);
        }

        // POST: DonThuocs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenBenhNhan,NgayKe,DaTuVan")] DonThuoc donThuoc)
        {
            if (id != donThuoc.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donThuoc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonThuocExists(donThuoc.Id))
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
            return View(donThuoc);
        }

        // GET: DonThuocs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donThuoc = await _context.DonThuocs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (donThuoc == null)
            {
                return NotFound();
            }

            return View(donThuoc);
        }

        // POST: DonThuocs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donThuoc = await _context.DonThuocs.FindAsync(id);
            if (donThuoc != null)
            {
                _context.DonThuocs.Remove(donThuoc);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonThuocExists(int id)
        {
            return _context.DonThuocs.Any(e => e.Id == id);
        }
    }
}
