using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DefenceSimulator3.Data;
using DefenceSimulator3.Models;

namespace DefenceSimulator3.Controllers
{
    public class WeaponDefencesController : Controller
    {
        private readonly DefenceSimulator3Context _context;

        public WeaponDefencesController(DefenceSimulator3Context context)
        {
            _context = context;
        }

        // GET: WeaponDefences
        public async Task<IActionResult> Index()
        {
            return View(await _context.WeaponDefence.ToListAsync());
        }
        // GET: WeaponDefences
        public async Task<IActionResult> Defence()
        {
            return View(await _context.WeaponDefence.ToListAsync());
        }

        // GET: WeaponDefences/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weaponDefence = await _context.WeaponDefence
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weaponDefence == null)
            {
                return NotFound();
            }

            return View(weaponDefence);
        }

        // GET: WeaponDefences/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WeaponDefences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WeaponId,Name,Speed,Amount")] WeaponDefence weaponDefence)
        {
            if (ModelState.IsValid)
            {
                _context.Add(weaponDefence);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(weaponDefence);
        }

        // GET: WeaponDefences/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weaponDefence = await _context.WeaponDefence.FindAsync(id);
            if (weaponDefence == null)
            {
                return NotFound();
            }
            return View(weaponDefence);
        }

        // POST: WeaponDefences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Speed,Amount")] WeaponDefence weaponDefence)
        {
            if (id != weaponDefence.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(weaponDefence);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WeaponDefenceExists(weaponDefence.Id))
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
            return View(weaponDefence);
        }

        // GET: WeaponDefences/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weaponDefence = await _context.WeaponDefence
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weaponDefence == null)
            {
                return NotFound();
            }

            return View(weaponDefence);
        }

        // POST: WeaponDefences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var weaponDefence = await _context.WeaponDefence.FindAsync(id);
            if (weaponDefence != null)
            {
                _context.WeaponDefence.Remove(weaponDefence);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WeaponDefenceExists(int id)
        {
            return _context.WeaponDefence.Any(e => e.Id == id);
        }
    }
}
