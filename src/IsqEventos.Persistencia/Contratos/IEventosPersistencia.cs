using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsqEventos.Domain;
using IsqEventos.Persistencia.Models;

namespace IsqEventos.Persistencia.Contratos
{
    public interface IEventosPersistencia
    {

        Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes = false);

        Task<Evento> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false);

    }
}