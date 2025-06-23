using DevSpot.Repositories;
using Microsoft.AspNetCore.Mvc;
using DevSpot.Models;
using Microsoft.AspNetCore.Identity;
using DevSpot.ViewModels;

namespace DevSpot.Controllers
{
    public class JobPostingsController : Controller
    {
        private readonly IRepository<JobPosting> _repository;
        private readonly UserManager<IdentityUser> _userManager;


        public JobPostingsController(
         IRepository<JobPosting> repository ,
         UserManager<IdentityUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var jobPostings = await _repository.GetAllAsync();
            return View(jobPostings);

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(JobPostingViewModel jobPostingVm)
        {
            //ModelState.Remove("User");
            //ModelState.Remove("UserId");
            
            if(ModelState.IsValid)
            {
                JobPosting jobPosting = new() 
                {
                    Title = jobPostingVm.Title,
                    Description = jobPostingVm.Description,
                    Company = jobPostingVm.Company,
                    Location = jobPostingVm.Location,
                    UserId = _userManager.GetUserId(User),
                    //PostedDate = DateTime.UtcNow,
                };
                //jobPosting.UserId = _userManager.GetUserId(User);
                await _repository.AddAsync(jobPosting);
                
            }
            return RedirectToAction(nameof(Index));
            //return View(jobPostingVm); // IF MOdelState not valid go to View page.
        }

    }
}
