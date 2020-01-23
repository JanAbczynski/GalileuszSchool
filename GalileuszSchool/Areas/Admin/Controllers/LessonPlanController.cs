﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GalileuszSchool.Infrastructure;
using GalileuszSchool.Models;
using GalileuszSchool.Areas.Admin.ViewModels;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace GalileuszSchool.Areas.Admin.Controllers
{
    //[Authorize(Roles = "admin, editor")]
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
            List<ClassRoom> classRooms = new List<ClassRoom>();
            foreach (var classRoom in _context.ClassRoom)
            {
                classRooms.Add(classRoom);
            }
            ViewBag.ClassRoomsList = classRooms;

            await _context.LessonPlan.Include(x => x.Course).ToListAsync();
            await _context.LessonPlan.Include(x => x.ClassRoom).ToListAsync();
            return View(_context.LessonPlan.OrderBy(a => a.startTime));


            //return View(await context.Teachers.OrderByDescending(x => x.Id).Include(x => x.Course).ToListAsync());
        }

        // GET: Admin/LessonPlans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            List<Course> coursesList = new List<Course>();
            foreach (var classRoom in _context.Courses)
            {
                coursesList.Add(classRoom);
            }
            ViewBag.ClassRoomsList = coursesList;


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
            ViewBag.Test = new SelectList(_context.ClassRoom.OrderBy(x => x.ClassRoomNumber), "Id", "ClassRoomName");
            ViewBag.CourseId = new SelectList(_context.Courses.OrderBy(x => x.Id), "Id", "Name");
            



            return View();
        }

        // POST: Admin/LessonPlans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( LessonPlan lessonPlan)
            //[Bind("classroom,dayId,startTime,stopTime, course")]
        {


            if (ModelState.IsValid)
            {
            lessonPlan.day = (Days)lessonPlan.dayId;

                if (!TimeValidation(lessonPlan))
                {
                    ModelState.AddModelError("", "The course already exists co to tu robi");
                    return View(lessonPlan);
                }
                
                _context.Add(lessonPlan);
                await _context.SaveChangesAsync();
                TempData["Success"] = "The lesson has been added";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "The course already exists");
            return View(lessonPlan);
        }

        // GET: Admin/LessonPlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            LessonPlan lessonPlan = await _context.LessonPlan.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }

            //var lessonPlan = await _context.LessonPlan.FindAsync(id);
            if (lessonPlan == null)
            {
                return NotFound();
            }

            ViewBag.CourseId = new SelectList(_context.Courses.OrderBy(x => x.Sorting), "Id", "Name", lessonPlan.CourseId);         
            return View(lessonPlan);
        }

        // POST: Admin/LessonPlans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  LessonPlan lessonPlan)
            //[Bind("Id,classroom,dayId,startTime,stopTime, course")]
        {
            if (id != lessonPlan.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                lessonPlan.day = (Days)lessonPlan.dayId;
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


        private Boolean TimeValidation(LessonPlan lessonPlan)
        {
            if (lessonPlan.startTime >= lessonPlan.stopTime)
            {
                ModelState.AddModelError("", "Course duration is 0");
                return false;
            }
            foreach (LessonPlan existingLesson in _context.LessonPlan)
            {
                if(!(existingLesson.startTime >= lessonPlan.stopTime || existingLesson.stopTime <= lessonPlan.startTime) && existingLesson.dayId == lessonPlan.dayId && existingLesson.ClassRoomId == lessonPlan.ClassRoomId)
                {

                    return false;
                }
                Console.WriteLine(existingLesson.startTime);
            }
            return true;
        }
    }
}
