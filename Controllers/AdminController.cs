using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tournament.Data.Context;

namespace Tournament.Controllers
{
	public class AdminController : Controller
	{
		private readonly ApplicationContext _applicationContext;

		public AdminController(ApplicationContext applicationContext)
		{
			_applicationContext = applicationContext;
		}

		public IActionResult Index()
		{

			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(string mail, string password)
		{
			if (mail == "admin@admin.com" && password == "maraskamuadmin")
			{
				return RedirectToAction("Index");
			}
			return View();
		}

		public async Task<IActionResult> Submissions()
		{
			var submissions = await _applicationContext.TeamApplicationForms.Include(x=>x.TeamMembers).ToListAsync();
			return View(submissions);
		}

		[Route("/admin/deletesubmission/{id}")]
		public async Task<IActionResult> DeleteSubmission(int id)
		{
			var submission = await _applicationContext.TeamApplicationForms.Where(x=>x.Id == id ).FirstOrDefaultAsync();

			if (submission != null)
			{
				_applicationContext.Remove(submission);
				await _applicationContext.SaveChangesAsync();
			}

			return RedirectToAction("Submissions");
		}

		[Route("/admin/SubmissionDetail/{id}")]
		[HttpGet]
		public async Task<IActionResult> SubmissionDetail(int id)
		{
			var submission = await _applicationContext.TeamApplicationForms.Where(x=>x.Id==id).Include(x=>x.TeamMembers).Include(x=>x.File).FirstOrDefaultAsync();
			return View(submission);
		}
	}
}
