using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsqEventos.Application.Dtos;
using IsqEventos.Persistencia.Models;


namespace IsqEventos.Application.Contratos
{
    public interface IEventosService
    {
        Task<EventoDto> addEventos(int userId, EventoDto model);

        Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto model);

        Task<bool> DeleteEvento(int userId, int eventoId);

        Task<PageList<EventoDto>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes = false);

        Task<EventoDto> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false);

    }
}