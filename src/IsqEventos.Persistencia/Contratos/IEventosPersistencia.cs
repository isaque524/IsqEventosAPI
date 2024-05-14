using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsqEventos.Domain;

namespace IsqEventos.Persistencia.Contratos
{
    public interface IEventosPersistencia
    {

        Task<Evento[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false);

        Task<Evento[]> GetAllEventosAsync( int userId, bool includePalestrantes = false);

        Task<Evento> GetEventoByIdAsync( int userId, int eventoId, bool includePalestrantes = false);

    }
}