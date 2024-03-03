using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly AppIdentityDbContext _appIdentityDbContext;
        private readonly IUserRepository _userRepository;

        public UserService(IOptions<AppSettings> appSettings, AppIdentityDbContext appIdentityDbContext, IUserRepository userRepository)
        {
            _appSettings = appSettings.Value;
            _appIdentityDbContext = appIdentityDbContext;
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<AppUser>> GetAll()
        {
            return await _userRepository.GetAll();
        }

        public async Task<AppUser?> GetById(string id)
        {
            return await _userRepository.GetById(id);
        }
    }
}
