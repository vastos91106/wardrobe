using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WR.Model;
using Microsoft.EntityFrameworkCore;
using WR.Data;

namespace WR.Controllers
{
    public class ArticleController : Controller
    {
        private readonly UserManager<User> _userManager;
        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        private ApplicationDbContext _dbContext = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>());

        public ArticleController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Detail(Guid ID)
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Create(Article model)
        {
            if (ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                try
                {
                    var user = await GetCurrentUserAsync();
                    model.UserID = user.Id;

                    _dbContext.Set<Article>().Add(model);
                    _dbContext.SaveChanges();
                    return Ok(model);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}