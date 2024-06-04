using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IsqEventos.Application.Contratos;
using IsqEventos.Domain;
using IsqEventos.Application.Dtos;
using IsqEventos.Persistencia.Contratos;
using IsqEventos.Persistencia.Models;

namespace IsqEventos.Application
{
    public class PalestranteService : IPalestranteService
    {

        private readonly IPalestrantesPersistencia _palestrantesPersistencia;

        private readonly IMapper _mapper;

        public PalestranteService(
                            IPalestrantesPersistencia palestrantesPersiste,
                            IMapper mapper)
        {
            _palestrantesPersistencia = palestrantesPersiste;
            _mapper = mapper;
        }


    public async Task<PalestranteDto> AddPalestrantes(int userId, PalestranteAddDto model)
        {
            try
            {
                var Palestrante = _mapper.Map<Palestrante>(model);
                Palestrante.UserId = userId;

                _palestrantesPersistencia.Add<Palestrante>(Palestrante);

                if (await _palestrantesPersistencia.SaveChangesAsync())
                {
                    var PalestranteRetorno = await _palestrantesPersistencia.GetPalestranteByUserIdAsync(userId, false);

                    return _mapper.Map<PalestranteDto>(PalestranteRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDto> UpdatePalestrante(int userId, PalestranteUpdateDto model)
        {
            try
            {
                var Palestrante = await _palestrantesPersistencia.GetPalestranteByUserIdAsync(userId, false);
                if (Palestrante == null) return null;

                model.Id = Palestrante.Id;
                model.UserId = userId;

                _mapper.Map(model, Palestrante);

                _palestrantesPersistencia.Update<Palestrante>(Palestrante);

                if (await _palestrantesPersistencia.SaveChangesAsync())
                {
                    var PalestranteRetorno = await _palestrantesPersistencia.GetPalestranteByUserIdAsync(userId, false);

                    return _mapper.Map<PalestranteDto>(PalestranteRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageList<PalestranteDto>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false)
        {
            try
            {
                var Palestrantes = await _palestrantesPersistencia.GetAllPalestrantesAsync(pageParams, includeEventos);
                if (Palestrantes == null) return null;

                var resultado = _mapper.Map<PageList<PalestranteDto>>(Palestrantes);

                resultado.CurrentPage = Palestrantes.CurrentPage;
                resultado.TotalPages = Palestrantes.TotalPages;
                resultado.PageSize = Palestrantes.PageSize;
                resultado.TotalCount = Palestrantes.TotalCount;

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDto> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false)
        {
            try
            {
                var Palestrante = await _palestrantesPersistencia.GetPalestranteByUserIdAsync(userId, includeEventos);
                if (Palestrante == null) return null;

                var resultado = _mapper.Map<PalestranteDto>(Palestrante);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}