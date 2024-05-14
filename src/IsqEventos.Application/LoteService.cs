using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IsqEventos.Application.Contratos;
using IsqEventos.Domain;
using IsqEventos.Application.Dtos;
using IsqEventos.Persistencia.Contratos;

namespace IsqEventos.Application
{
    public class LoteService : ILotesService
    {

        private readonly IGeralPersistencia _geralPersistencia;
        private readonly ILotePersistencia _lotePersistencia;

        private readonly IMapper _mapper;

        public LoteService(IGeralPersistencia geralPersiste,
                            ILotePersistencia lotePersiste,
                            IMapper mapper)
        {
            _geralPersistencia = geralPersiste;
            _lotePersistencia = lotePersiste;
            _mapper = mapper;
        }


        public async Task AddLote(int eventoId, LoteDto model)
        {
            try
            {
                var lote = _mapper.Map<Lote>(model);
                lote.EventoId = eventoId;

                _geralPersistencia.Add<Lote>(lote);

                await _geralPersistencia.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] models)
        {
            try
            {
                var lotes = await _lotePersistencia.GetLotesByEventoIdAsync(eventoId);
                if (lotes == null) return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddLote(eventoId, model);
                    }
                    else
                    {
                        var lote = lotes.FirstOrDefault(lote => lote.Id == model.Id);
                        model.EventoId = eventoId;

                        _mapper.Map(model, lote);

                        _geralPersistencia.Update<Lote>(lote);

                        await _geralPersistencia.SaveChangesAsync();
                    }
                }

                var loteRetorno = await _lotePersistencia.GetLotesByEventoIdAsync(eventoId);

                return _mapper.Map<LoteDto[]>(loteRetorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteLote(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotePersistencia.GetLoteByIdsAsync(eventoId, loteId);
                if (lote == null) throw new Exception("Lote para delete não encontrado.");

                _geralPersistencia.Delete<Lote>(lote);
                return await _geralPersistencia.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId)
        {
            try
            {
                var lotes = await _lotePersistencia.GetLotesByEventoIdAsync(eventoId);
                if (lotes == null) return null;

                var resultado = _mapper.Map<LoteDto[]>(lotes);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto> GetLoteByIdsAsync(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotePersistencia.GetLoteByIdsAsync(eventoId, loteId);
                if (lote == null) return null;

                var resultado = _mapper.Map<LoteDto>(lote);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}