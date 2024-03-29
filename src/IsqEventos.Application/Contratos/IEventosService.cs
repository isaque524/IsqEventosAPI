using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsqEventos.Domain;

namespace IsqEventos.Application.Contratos
{
    public interface IEventosService
    {
        Task<Evento> addEventos(Evento model);

        Task<Evento> UpdateEvento(int eventoId, Evento model);

        Task<bool> DeleteEvento(int eventoId);


        Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false);

        Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false);

        Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false);

    }
}