﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using GalileuszSchool.Models;
using System.ComponentModel.DataAnnotations;

namespace GalileuszSchool.Areas.Admin.Controllers
{
    [Area("Admin")]
    
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager )
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
              
        }

        // get /admin/Roles
        public IActionResult Index()
        {
            return View(roleManager.Roles);
        }

        //get /admin/roles/create
        public IActionResult Create()
        {
            return View();
        }

        //post /admin/roles/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([MinLength(2), Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    TempData["Success"] = "The role has been created";
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            ModelState.AddModelError("", "Minimum lenght is 2");
            return View();
        }

        //get /admin/roles/edit{id}
        
        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);

            List<AppUser> members = new List<AppUser>();
            List<AppUser> nonMembers = new List<AppUser>();


            foreach (AppUser user in userManager.Users)
            {
                var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }

            return View(new RoleEdit
            {
                Role = role,
                Memmbers = members,
                NonMemmbers = nonMembers
            });
        }

    }
}