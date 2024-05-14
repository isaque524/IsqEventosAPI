using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsqEventos.Domain;

namespace IsqEventos.Persistencia.Contratos
{
    public interface ILotePersistencia
    {

        Task<Lote[]> GetLotesByEventoIdAsync(int eventoId);

        Task<Lote> GetLoteByIdsAsync(int eventoId, int id);

    }
}