using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsqEventos.Domain;
using IsqEventos.Persistencia.Models;

namespace IsqEventos.Persistencia.Contratos
{
    public interface IPalestrantesPersistencia : IGeralPersistencia
    {


        Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false);

        Task<Palestrante> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false);

    }
}