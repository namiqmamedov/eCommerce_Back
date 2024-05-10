using Wolmart.MVC.DAL;
using Wolmart.MVC.Models;
using Wolmart.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Wolmart.MVC.Areas.admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FeedbackController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public FeedbackController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page)
        {
            IQueryable<Feedback> feedbacks = _context.Feedbacks.Include(x=>x.FeedbackImages).OrderByDescending(x => x.ID);

            return View(PagiNationList<Feedback>.Create(feedbacks, page, 5));
        }

        public async Task<IActionResult> Delete(int? ID, int page)
        {
            IQueryable<Feedback> feedbacks = _context.Feedbacks.OrderBy(x => x.ID == ID);

            if (ID == null) return BadRequest();

            Feedback feedback = await _context.Feedbacks.FirstOrDefaultAsync(x => x.ID == ID);

            if (feedback == null) return NotFound();

            if (feedbacks.Count() == 1) return BadRequest();
            
            _context.Feedbacks.Remove(feedback);

            await _context.SaveChangesAsync();

            return PartialView("_FeedbackIndexPartial", PagiNationList<Feedback>.Create(feedbacks, page, 5));
        }

        [HttpGet]
        public async Task<IActionResult> Read(int? ID)
        {
            if (ID == null) return BadRequest();

            Feedback feedback = await _context.Feedbacks.Include(x=>x.FeedbackImages).FirstOrDefaultAsync(x => x.ID == ID);

            if (feedback == null) return NotFound();

            return View(feedback);
        }
    }
}
