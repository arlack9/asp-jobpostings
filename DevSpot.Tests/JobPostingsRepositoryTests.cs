using DevSpot.Data;
using DevSpot.Models;
using DevSpot.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;
using NuGet.Packaging.Signing;
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
        public async Task GetByIdAsync_ShouldReturnJobPosting()

        {
            //db context
            //var db = CreateDbContext();
            
            var db = CreateDbContext();

            //job posting repository 
            var repository = new JobPostingRepository(db);
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

            var result = await repository.GetByIdAsync(jobPosting.Id);
            Assert.NotNull(result);
            Assert.Equal("Test Title", result.Title);
        }

        //not found

        [Fact]

        public async Task GetByIdAsync_ShouldThrowKeyNotFoundException()
        {
            var db = CreateDbContext();
            var repository = new JobPostingRepository(db);
            // Act & Assert

            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => repository.GetByIdAsync(999)
                );

        }

        //fetch all

        [Fact]
        public async Task GetAllAsync()
        {
            var db = CreateDbContext();
            var repository = new JobPostingRepository(db);
            var jobPosting1 = new JobPosting
            {
                Title = "Test Title 1",
                Description = "Test Description 1",
                PostedDate = DateTime.Now,
                Company = "Test Company 1",
                Location = "Test Location 1",
                UserId = "TestUserId1"
            };
            var jobPosting2 = new JobPosting
            {
                Title = "Test Title 2",
                Description = "Test Description 2",
                PostedDate = DateTime.Now,
                Company = "Test Company 2",
                Location = "Test Location 2",
                UserId = "TestUserId2"
            };
            await db.JobPostings.AddRangeAsync(jobPosting1,jobPosting2);
          
            await db.SaveChangesAsync();

            var result =await repository.GetAllAsync();
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }


        //update

        [Fact]
        public async Task UpdateAsync_ShouldUpdateJobPosting()
        {
            var db = CreateDbContext();
            var repository =new JobPostingRepository(db);

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
            jobPosting.Title = "Updated Title";
            jobPosting.Description = "Updated Description";

            await repository.UpdateAsync(jobPosting);

            var result = db.JobPostings.Find(jobPosting.Id);
            Assert.NotNull(result);
            Assert.Equal("Updated Title", result.Title);

        }


        //delete
        [Fact]
        public async Task DeleteAsync_ShouldDeleteJobPosting()
        {
            var db = CreateDbContext();
            var repository = new JobPostingRepository(db);
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
            await repository.DeleteAsync(jobPosting.Id);
            var result = await db.JobPostings.FindAsync(jobPosting.Id);
            Assert.Null(result);
        }
    }
}
