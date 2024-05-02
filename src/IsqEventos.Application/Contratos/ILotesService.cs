using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsqEventos.Application.Dtos;


namespace IsqEventos.Application.Contratos
{
    public interface ILotesService
    {
        Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] model);

        Task<bool> DeleteLote(int eventoId, int loteId);


        Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId);

        Task<LoteDto> GetLoteByIdsAsync(int eventoId, int loteId);

    }
}