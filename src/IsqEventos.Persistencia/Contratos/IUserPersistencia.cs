using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsqEventos.Domain.Identity;

namespace IsqEventos.Persistencia.Contratos
{
    public interface IUserPersistencia : IGeralPersistencia
    {
        Task<IEnumerable<User>> GetUsersAsync();

        Task<User> GetUserByIdAsync(int id);

        Task<User> GetUserByUserNameAsync(string userName);
    }
}