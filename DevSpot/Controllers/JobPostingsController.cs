using DevSpot.Repositories;
using Microsoft.AspNetCore.Mvc;
using DevSpot.Models;
using Microsoft.AspNetCore.Identity;
using DevSpot.ViewModels;
using Microsoft.AspNetCore.Authorization;
using DevSpot.Constants;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace DevSpot.Controllers
{
    [Authorize]
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

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            if( User.IsInRole(Roles.Employee)) //restricts filtered view to employee only
            {
                // If the user is an admin, show all job postings
                var alljobPostings = await _repository.GetAllAsync();
                //return View(jobPostings);

                var userId = _userManager.GetUserId(User);

                var filteredJobPostings = alljobPostings.Where(jp => jp.UserId == userId);
                return View(filteredJobPostings);
            }
            var jobPostings = await _repository.GetAllAsync();
            return View(jobPostings);

        }

        [Authorize(Roles = "Admin, Employer")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Employer")]
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

        //JobPostings/Delete/5
        [HttpDelete]
        [Authorize(Roles = "Admin, Employer")]
        public async Task<IActionResult> Delete(int id)
        {
            var jobPosting = await _repository.GetByIdAsync(id);

            if(jobPosting == null)
            {
                return NotFound();
            }

            var userId=_userManager.GetUserId(User);

            if(User.IsInRole(Roles.Admin)==false && jobPosting.UserId!= userId)
            {
                return Forbid(); // If the user is not an admin and does not own the job posting, return 403 Forbidden
            }
            return Ok();
        }

    }
}
