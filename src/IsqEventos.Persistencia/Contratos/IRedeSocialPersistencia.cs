using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsqEventos.Domain;

namespace IsqEventos.Persistencia.Contratos
{
    public interface IRedeSocialPersistencia : IGeralPersistencia
    {
        Task<RedeSocial> GetRedeSocialEventoByIdsAsync(int EventoId, int id);

        Task<RedeSocial> GetRedeSocialPalestranteByIdsAsync(int PalestranteId, int id);

        Task<RedeSocial[]> GetAllByEventoIdsAsync(int EventoId);

        Task<RedeSocial[]> GetAllByPalestranteIdsAsync(int PalestranteId);

    }
}