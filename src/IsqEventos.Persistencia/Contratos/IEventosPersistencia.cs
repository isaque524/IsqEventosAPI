using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsqEventos.Domain;

namespace IsqEventos.Persistencia.Contatos
{
    public interface IEventosPersistencia
    {

        Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false);

        Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false);

        Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false);

    }
}