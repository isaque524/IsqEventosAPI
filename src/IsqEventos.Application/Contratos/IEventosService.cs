using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsqEventos.Application.Dtos;


namespace IsqEventos.Application.Contratos
{
    public interface IEventosService
    {
        Task<EventoDto> addEventos(EventoDto model);

        Task<EventoDto> UpdateEvento(int eventoId, EventoDto model);

        Task<bool> DeleteEvento(int eventoId);


        Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false);

        Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false);

        Task<EventoDto> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false);

    }
}