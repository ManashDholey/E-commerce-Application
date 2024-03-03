using Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUserService
    {
        //Task<AppUser?> Authenticate(AuthenticateRequest model);
        Task<IEnumerable<AppUser>> GetAll();
        Task<AppUser?> GetById(string id);
        //Task<AppUser?> AddAndUpdateUser(AppUser userObj);
    }
}
