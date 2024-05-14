using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsqEventos.Domain;

namespace IsqEventos.Persistencia.Contratos
{
    public interface IPalestrantesPersistencia
    {


        //Palestrantes
        Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos);

        Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos);

        Task<Palestrante> GetPalestranteByIdAsync(int palestranteId, bool includeEventos);

    }
}