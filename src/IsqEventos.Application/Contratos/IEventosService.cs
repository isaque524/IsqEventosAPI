using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsqEventos.Application.Dtos;


namespace IsqEventos.Application.Contratos
{
    public interface IEventosService
    {
        Task<EventoDto> addEventos(int userId, EventoDto model);

        Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto model);

        Task<bool> DeleteEvento(int userId, int eventoId);


        Task<EventoDto[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false);

        Task<EventoDto[]> GetAllEventosAsync(int userId, bool includePalestrantes = false);

        Task<EventoDto> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false);

    }
}