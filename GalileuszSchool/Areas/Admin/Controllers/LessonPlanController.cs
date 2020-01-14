using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GalileuszSchool.Infrastructure;
using GalileuszSchool.Models;
using GalileuszSchool.Areas.Admin.ViewModels;

namespace GalileuszSchool.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LessonPlanController : Controller
    {
        private readonly GalileuszSchoolContext _context;

        public LessonPlanController(GalileuszSchoolContext context)
        {
            _context = context;
        }

        // GET: Admin/LessonPlans
        public async Task<IActionResult> Index()
        {
            return View(await _context.LessonPlan.ToListAsync());
        }

        // GET: Admin/LessonPlans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lessonPlan = await _context.LessonPlan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lessonPlan == null)
            {
                return NotFound();
            }

            return View(lessonPlan);
        }

        // GET: Admin/LessonPlans/Create
        public IActionResult Create()
        {


            return View();
        }

        // POST: Admin/LessonPlans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,classroom,day,startTime,stopTime, Course")] LessonPlan lessonPlan)
        {
            var v1 = lessonPlan.classroom;
            var v2 = lessonPlan.course;
            var v3 = lessonPlan.day;
            var v4 = lessonPlan.Id;
            var v5 = lessonPlan.startTime;
            var v6 = lessonPlan.stopTime;


            if (ModelState.IsValid)
            {
                _context.Add(lessonPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lessonPlan);
        }

        // GET: Admin/LessonPlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lessonPlan = await _context.LessonPlan.FindAsync(id);
            if (lessonPlan == null)
            {
                return NotFound();
            }
            return View(lessonPlan);
        }

        // POST: Admin/LessonPlans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,classroom,day,startTime,stopTime")] LessonPlan lessonPlan)
        {
            if (id != lessonPlan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lessonPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LessonPlanExists(lessonPlan.Id))
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
            return View(lessonPlan);
        }

        // GET: Admin/LessonPlans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lessonPlan = await _context.LessonPlan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lessonPlan == null)
            {
                return NotFound();
            }

            return View(lessonPlan);
        }

        // POST: Admin/LessonPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lessonPlan = await _context.LessonPlan.FindAsync(id);
            _context.LessonPlan.Remove(lessonPlan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LessonPlanExists(int id)
        {
            return _context.LessonPlan.Any(e => e.Id == id);
        }
    }
}
