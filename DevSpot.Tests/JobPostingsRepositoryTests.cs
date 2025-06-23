using DevSpot.Data;
using DevSpot.Models;
using DevSpot.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSpot.Tests
{
    public class JobPostingsRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public JobPostingsRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("JobPostingDb") //in -memory db for testing
                .Options;
        }

        private ApplicationDbContext CreateDbContext() => new ApplicationDbContext(_options);

        [Fact]
        public async Task AddAsync_ShouldAddJobPosting()
        {
            //db context
            var db=CreateDbContext();
            
            //job posting repository 
            var repository = new JobPostingRepository(db);
            
            //job posting
            var jobPosting = new JobPosting
            {
                Title = "Test Title",
                Description = "TEst Descirption",
                PostedDate = DateTime.Now,
                Company = "Test company",
                Location = "Test Location",
                UserId = "1234"
            };

            //execute
            await repository.AddAsync(jobPosting);
            
            //result
            var result = db.JobPostings.SingleOrDefault(x => x.Title == "Test Title");

            //assert
            Assert.NotNull(result);

            Assert.Equal("Test Title", result.Title);

        }

        [Fact]
        public async Task GetByUdAsync_ShouldReturnJobPosting()
        {
            var db = CreateDbContext();
            var jobPosting = new JobPosting
            {
                Title = "Test Title",
                Description = "Test Description",
                PostedDate = DateTime.Now,
                Company = "Test Company",
                Location = "Test Location",
                UserId = "TestUserId"
            };

            await db.JobPostings.AddAsync(jobPosting);
            await db.SaveChangesAsync();

            var result = await repository.GetIdAsync(JobPosting.Id);
            Assert.NotNull(result);
            Assert.Equal("Test Title", result.Title);
        }
    }
}
