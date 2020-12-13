    using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Library.Models;
using Library.ViewModels;
using Library.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Library.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public UsersController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Index() => View(_userManager.Users.ToList().Where(p => p.UserName != "admin"));

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult CreateLibrarian()
        {
            return View("~/Views/Users/Create.cshtml", new ReaderViewModel { Role = "librarian" });

        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult CreateReader() 
        {
            return View("~/Views/Users/Create.cshtml", new ReaderViewModel { Role = "reader" });
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { 
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    SecondName = model.SecondName
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (model.Role.Equals("reader"))
                    {
                        Address address = new Address
                        {
                            City = model.City,
                            Street = model.Street,
                            Hous = model.Hous
                        };
                        Reader reader = new Reader
                        {
                            Id = user.Id,
                            AddressId = address.Id,
                            Phone = model.Phone
                        };

                        await _userManager.AddToRoleAsync(user, "reader");

                        _context.Addresses.Add(address);
                        _context.SaveChanges();
                        reader.AddressId = address.Id;
                        _context.Readers.Add(reader);
                        _context.SaveChanges();
                    }
                    else
                    {
                        Librarian librarian = new Librarian
                        {
                            Id = user.Id
                        };

                        await _userManager.AddToRoleAsync(user, "librarian");

                        _context.Librarians.Add(librarian);
                        _context.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            UserViewModel model;
            User user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (await _userManager.IsInRoleAsync(user, "reader"))
            {
                Reader reader = _context.Readers.Find(id);
                reader.Address = _context.Addresses.Find(reader.AddressId);

                model = new ReaderViewModel {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    SecondName = user.SecondName,
                    Phone = reader.Phone,
                    City = reader.Address.City,
                    Street = reader.Address.Street,
                    Hous = reader.Address.Hous,
                    Role = "reader"
                };
            }
            else
            {
                Librarian librarian = _context.Librarians.Find(id);

                model = new UserViewModel {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    SecondName = user.SecondName,
                    Role = "librarian"
                };
            }

            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                user.UserName = model.UserName;

                if (user != null)
                {
                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        user.FirstName = model.FirstName;
                        user.SecondName = model.SecondName;

                        if (model.Role.Equals("reader"))
                        {
                            Reader reader = _context.Readers.Find(model.Id);
                            reader.Address = _context.Addresses.Find(reader.AddressId);

                            
                            reader.Phone = model.Phone;
                            reader.Address.City = model.City;
                            reader.Address.Street = model.Street;
                            reader.Address.Hous = model.Hous;

                            _context.Readers.Update(reader);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            Librarian librarian = _context.Librarians.Find(model.Id);

                            _context.Librarians.Update(librarian);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }

                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                if (await _userManager.IsInRoleAsync(user, "reader"))
                {
                    _context.Readers.Remove(_context.Readers.Find(id));
                }
                else
                {
                    _context.Librarians.Remove(_context.Librarians.Find(id));
                }

                IdentityResult result = await _userManager.DeleteAsync(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
