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
                var palestrante = await _palestrantesPersistencia.GetPalestranteByUserIdAsync(userId, false);
                if (palestrante == null) return null;

                model.Id = palestrante.Id;
                model.UserId = userId;

                _mapper.Map(model, palestrante);

                _palestrantesPersistencia.Update<Palestrante>(palestrante);
                if (await _palestrantesPersistencia.SaveChangesAsync())
                {
                    var palestranteRetorno = await _palestrantesPersistencia.GetPalestranteByUserIdAsync(userId, false);
                    return _mapper.Map<PalestranteDto>(palestranteRetorno);
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
                var palestrante = await _palestrantesPersistencia.GetAllPalestrantesAsync(pageParams, includeEventos);
                if (palestrante == null) return null;

                var resultado = _mapper.Map<PageList<PalestranteDto>>(palestrante);

                resultado.CurrentPage = palestrante.CurrentPage;

                resultado.TotalPages = palestrante.TotalPages;

                resultado.PageSize = palestrante.PageSize;

                resultado.TotalCount = palestrante.TotalCount;


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
                var palestrante = await _palestrantesPersistencia.GetPalestranteByUserIdAsync(userId, includeEventos);
                if (palestrante == null) return null;

                var resultado = _mapper.Map<PalestranteDto>(palestrante);

                return resultado;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


    }
}