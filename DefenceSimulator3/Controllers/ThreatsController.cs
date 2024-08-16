using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DefenceSimulator3.Data;
using DefenceSimulator3.Models;
using static DefenceSimulator3.Models.Enums;
using System.Collections;
using DefenceSimulator3.Service;


namespace DefenceSimulator3.Controllers
{
    public class ThreatsController : Controller
    {
        private readonly DefenceSimulator3Context _context;
        private readonly AttackHandlerService _attackHandlerService;


        public ThreatsController(DefenceSimulator3Context context, AttackHandlerService attackHandlerService)
        {
            _context = context;
            _attackHandlerService = attackHandlerService;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Route("StartAttack/{threatId}")]
        public async Task<IActionResult> StartAttack(int threatId)
        {
            bool isAttackLaunched = await _attackHandlerService.RegisterAndRunAttackTask(threatId);

            return isAttackLaunched ? RedirectToAction(nameof(Index)) : RedirectToAction(nameof(Index), new { Error = "Attack not found" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EndAttack(int threatId)
        {
            Threat? attack = _context.Threat.Find(threatId);

            bool result = await _attackHandlerService.RemoveAttackForIntercepted(attack.ThreatId);

            return result ? RedirectToAction(nameof(Index)) : RedirectToAction(nameof(Index), new { Error = "Attack not found" });
        }


        // GET: Threats
        public async Task<IActionResult> Index()
        {
            var defenceSimulator3Context = _context.Threat.Include(t => t.Origin).Include(t => t.Weapon);
            return View(await defenceSimulator3Context.ToListAsync());
        }

        // GET: Threats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var threat = await _context.Threat
                .Include(t => t.Origin)
                .Include(t => t.Weapon)
                .FirstOrDefaultAsync(m => m.ThreatId == id);
            if (threat == null)
            {
                return NotFound();
            }

            return View(threat);
        }

        // GET: Threats/Create
        public IActionResult Create()
        {
            List<Enums.ThreatStatus> ThreatStatus = new List<Enums.ThreatStatus> { Enums.ThreatStatus.בהמתנה };
            ViewData["OriginId"] = new SelectList(_context.Origin, "Id", "Name");
            ViewData["WeaponId"] = new SelectList(_context.Weapon, "WeaponId", "Name");
            ViewData["EnumStatus"] = new SelectList(ThreatStatus);
            return View();
        }

        // POST: Threats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OriginId,Amount,Status,WeaponId")] Threat threat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(threat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            List<Enums.ThreatStatus> ThreatStatus = new List<Enums.ThreatStatus> { Enums.ThreatStatus.בהמתנה };
            ViewData["OriginId"] = new SelectList(_context.Origin, "Id", "Id", threat.OriginId);
            ViewData["WeaponId"] = new SelectList(_context.Weapon, "WeaponId", "WeaponId", threat.WeaponId);
            ViewData["EnumStatus"] = new SelectList(ThreatStatus);

            return View(threat);
        }

        // GET: Threats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var threat = await _context.Threat.FindAsync(id);
            if (threat == null)
            {
                return NotFound();
            }
            ViewData["OriginId"] = new SelectList(_context.Origin, "Id", "Id", threat.OriginId);
            ViewData["WeaponId"] = new SelectList(_context.Weapon, "WeaponId", "WeaponId", threat.WeaponId);
            return View(threat);
        }

        // POST: Threats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ThreatId,OriginId,LaunchTime,Amount,Success,Fail,Status,WeaponId")] Threat threat)
        {
            if (id != threat.ThreatId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(threat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThreatExists(threat.ThreatId))
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
            ViewData["OriginId"] = new SelectList(_context.Origin, "Id", "Id", threat.OriginId);
            ViewData["WeaponId"] = new SelectList(_context.Weapon, "WeaponId", "WeaponId", threat.WeaponId);
            return View(threat);
        }

        // GET: Threats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var threat = await _context.Threat
                .Include(t => t.Origin)
                .Include(t => t.Weapon)
                .FirstOrDefaultAsync(m => m.ThreatId == id);
            if (threat == null)
            {
                return NotFound();
            }

            return View(threat);
        }

        // POST: Threats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var threat = await _context.Threat.FindAsync(id);
            if (threat != null)
            {
                _context.Threat.Remove(threat);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThreatExists(int id)
        {
            return _context.Threat.Any(e => e.ThreatId == id);
        }
    }
}
