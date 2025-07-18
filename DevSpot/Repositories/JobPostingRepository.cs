﻿using DevSpot.Data;
using DevSpot.Models;
using Microsoft.EntityFrameworkCore;

namespace DevSpot.Repositories;

public class JobPostingRepository : IRepository<JobPosting>
{
    private readonly ApplicationDbContext _context;
    public JobPostingRepository(ApplicationDbContext context)
    {
        _context = context;

    }
    public async Task AddAsync(JobPosting entity)
    {
        //throw new NotImplementedException();
        await _context.JobPostings.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        //throw new NotImplementedException();
        var jobPosting =await _context.JobPostings.FindAsync(id);
        if(jobPosting==null)
        {

            throw new KeyNotFoundException();
        }
        _context.JobPostings.Remove(jobPosting);
        await _context.SaveChangesAsync();
    }
       

    public async Task<IEnumerable<JobPosting>> GetAllAsync()
    {
        //throw new NotImplementedException();
        return await _context.JobPostings.ToListAsync();
    }

    public async Task<JobPosting> GetByIdAsync(int id)
    {
        //throw new NotImplementedException();
        var jobPosting = await _context.JobPostings.FindAsync(id);
        if(jobPosting==null)
        {
            throw new KeyNotFoundException();
     
        }
        return jobPosting;
    }

    public async Task UpdateAsync(JobPosting entity)
    {
        //throw new NotImplementedException();

        _context.JobPostings.Update(entity);
        await _context.SaveChangesAsync();
    }
}
