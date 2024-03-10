using Core.Entities.Identity;
using Core.Interfaces;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly AppIdentityDbContext _appIdentityDbContext;

        public UserRepository(AppIdentityDbContext appIdentityDbContext)
        {
            _appIdentityDbContext = appIdentityDbContext;
         }
        public async Task<IEnumerable<AppUser>> GetAll()
        {
            return await _appIdentityDbContext.Users.ToListAsync();
        }

        public async Task<AppUser?> GetById(string id)
        {
            return await _appIdentityDbContext.Users.Where(e => e.Id == id).FirstOrDefaultAsync();
        }
    }
}
