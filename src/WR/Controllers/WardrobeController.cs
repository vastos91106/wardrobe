using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WR.Data;
using WR.Model;

namespace WR.Controllers
{
    public class WardrobeController : Controller
    {
        private readonly UserManager<User> _userManager;
        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();

        public WardrobeController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ActionResult> Index()
        {
            var result = _dbContext.WarDrobes.Where(x => x.IsPrivate == false);
            if (User.Identity.IsAuthenticated)
            {
                var user = await GetCurrentUserAsync();

                result = result.Union(
                       _dbContext.InvitedUsers.Where(x => x.UserID == user.Id)
                           .Select(v => v.WarDrobe)
                           .Include(v => v.InvitedUsers)
                           .Include(c => c.Articles));

                result = result.Union(_dbContext.WarDrobes.Where(x => x.UserID == user.Id));
            }

            result = result.Include(v => v.InvitedUsers).Include(c => c.Articles);
            return View(result.ToList());
        }

        public async Task<ActionResult> Detail(Guid ID)
        {
            var result = _dbContext.WarDrobes.Include(v => v.InvitedUsers).Include(c => c.Articles).FirstOrDefault(x => x.ID == ID);
            if (result != null)
            {
                if (result.IsPrivate == true)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        var user = await GetCurrentUserAsync();
                        if (result.UserID == ID || result.InvitedUsers.Any(z => z.UserID == user.Id))
                        {
                            return View(result);
                        }
                    }
                }
                else
                {
                    return View(result);
                }
            }
            return NotFound();
        }

        public IActionResult Create(WarDrobe model)
        {
            if (ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                try
                {
                    _dbContext.Set<WarDrobe>().Add(model);
                    _dbContext.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
                return Ok(model);
            }
        }


        public async Task<IActionResult> InviteUser(Guid userID, Guid wardrobeID)
        {
            var result = _dbContext.WarDrobes.FirstOrDefault(x => x.ID == wardrobeID);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return Unauthorized();
                }

                if (result.UserID != user.Id)
                {
                    ModelState.AddModelError(nameof(userID), "Только хозяин может приглашать людей");
                    return BadRequest(ModelState);
                }
                else
                {
                    if (_dbContext.InvitedUsers.Any(x => x.UserID == userID && x.WarDrobeID == wardrobeID))
                    {
                        var entity = _dbContext.InvitedUsers.First(x => x.UserID == userID && x.WarDrobeID == wardrobeID);
                        _dbContext.Remove<InvitedUser>(entity);
                        _dbContext.SaveChanges();
                        return Ok("Убрано");
                    }

                    _dbContext.InvitedUsers.Add(new InvitedUser()
                    {
                        UserID = userID,
                        WarDrobeID = wardrobeID
                    });
                    _dbContext.SaveChanges();
                    return Ok("Добавлено");
                }
            }
        }
    }
}