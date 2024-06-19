using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepo : ICommentRepository
    {
        private readonly ApplicationDBContext _context;
        public CommentRepo(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var existing = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if(existing == null)
            {
                return null;
            }
            _context.Comments.Remove(existing);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            //FirstOrDefaultAsync
            return await _context.Comments.FindAsync(id);
        }

        public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
        {
            var existing = await _context.Comments.FindAsync(id);
            if(existing == null)
            {
                return null;
            }

            existing.Title = commentModel.Title;
            existing.Content = commentModel.Content;
            await _context.SaveChangesAsync();
            return existing;
        }
    }
}