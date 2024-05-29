using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsqEventos.Application.Dtos;
using IsqEventos.Persistencia.Models;


namespace IsqEventos.Application.Contratos
{
    public interface IPalestranteService
    {
        Task<PalestranteDto> AddPalestrantes(int userId, PalestranteAddDto model);

        Task<PalestranteDto> UpdatePalestrante(int userId, PalestranteUpdateDto model);


        Task<PageList<PalestranteDto>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false);

        Task<PalestranteDto> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false);

    }


}