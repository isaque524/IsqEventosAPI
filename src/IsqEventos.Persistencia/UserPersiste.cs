using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsqEventos.Domain.Identity;
using IsqEventos.Persistencia.contextos;
using IsqEventos.Persistencia.Contratos;
using Microsoft.EntityFrameworkCore;

namespace IsqEventos.Persistencia
{
    public class UserPersiste : GeralPersiste, IUserPersistencia
    {

        private readonly IsqEventosContext _context;

        public UserPersiste(IsqEventosContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }


        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _context.Users.SingleOrDefaultAsync(user => user.UserName == userName.ToLower());
        }
    }
}