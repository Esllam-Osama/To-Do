using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using To_Do.data;
using To_Do.Models;

namespace To_Do.Controllers
{
    public class ToDoController : Controller
    {
        ApplicationDbContext applicationDbContext = new ApplicationDbContext();
        public IActionResult CreateUser()
        {
            return View();
        }
        public IActionResult SaveCooke(string name)
        {
            CookieOptions cookieOptions = new();
            cookieOptions.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Append("name", name, cookieOptions);
            return RedirectToAction("AllItems");
        }
        public IActionResult AllItems()
        {
            return View(applicationDbContext.missions.ToList());
        }
        public IActionResult CreateNew()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateNew(Mission mission, IFormFile FileUrl)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(FileUrl.FileName);
            if (  FileUrl.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", fileName);
                using (var stream = System.IO.File.Create(path))
                {
                    FileUrl.CopyTo(stream);
                }
                mission.FileUrl = fileName;
                applicationDbContext.missions.Add(mission);
                applicationDbContext.SaveChanges();
            }
            return RedirectToAction(nameof(AllItems));
        }
        [HttpGet]
        public IActionResult Eddit(int id)
        {
            var m = applicationDbContext.missions.FirstOrDefault(e => e.Id == id);
            return View(m);
        }
        [HttpPost]
        public IActionResult Eddit(Mission mission ,IFormFile FileUrl)
        {
            var oldFile = applicationDbContext.missions.AsNoTracking().FirstOrDefault(e => e.Id == mission.Id);
            if (FileUrl != null && FileUrl.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(FileUrl.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", fileName);
                using (var stram = System.IO.File.Create(path))
                {
                    FileUrl.CopyTo(stram);
                }
                mission.FileUrl = fileName;
                // delete old path
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", oldFile.FileUrl);
                if (System.IO.File.Exists(oldPath)) {
                    System.IO.File.Delete(oldPath);
                }
            }
            else
            {
                mission.FileUrl=oldFile.FileUrl ;
            }
            applicationDbContext.missions.Update(mission);
            applicationDbContext.SaveChanges();

            return RedirectToAction(nameof(AllItems));
        }
        public IActionResult Delete(int id)
        {
            Mission mission = applicationDbContext.missions.FirstOrDefault(e => e.Id == id);
            var path = Path.Combine(Directory.GetCurrentDirectory().ToString(), "wwwroot\\files", mission.FileUrl);
            if (System.IO.File.Exists(path)) {
            System.IO .File.Delete(path);
            }
            applicationDbContext.missions.Remove(mission);
            applicationDbContext.SaveChanges();
            return RedirectToAction(nameof(AllItems));
        }
    }
}
