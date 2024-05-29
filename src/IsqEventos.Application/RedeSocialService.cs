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
    public class RedeSocialService : IRedeSocialService
    {

        private readonly IRedeSocialPersistencia _redeSocialPersiste;

        private readonly IMapper _mapper;

        public RedeSocialService(
                            IRedeSocialPersistencia redeSocialPersiste,
                            IMapper mapper)
        {

            _redeSocialPersiste = redeSocialPersiste;
            _mapper = mapper;
        }


        public async Task AddRedeSocial(int Id, RedeSocialDto model, bool isEvento)
        {
            try
            {
                var RedeSocial = _mapper.Map<RedeSocial>(model);
                if (isEvento)
                {
                    RedeSocial.EventoId = Id;
                    RedeSocial.PalestranteId = null;
                }
                else
                {
                    RedeSocial.EventoId = null;
                    RedeSocial.PalestranteId = Id;
                }

                _redeSocialPersiste.Add<RedeSocial>(RedeSocial);

                await _redeSocialPersiste.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> SaveByEvento(int eventoId, RedeSocialDto[] models)
        {
            try
            {
                var RedeSocials = await _redeSocialPersiste.GetAllByEventoIdsAsync(eventoId);
                if (RedeSocials == null) return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddRedeSocial(eventoId, model, true);
                    }
                    else
                    {
                        var RedeSocial = RedeSocials.FirstOrDefault(RedeSocial => RedeSocial.Id == model.Id);
                        model.EventoId = eventoId;

                        _mapper.Map(model, RedeSocial);

                        _redeSocialPersiste.Update<RedeSocial>(RedeSocial);

                        await _redeSocialPersiste.SaveChangesAsync();
                    }
                }

                var RedeSocialRetorno = await _redeSocialPersiste.GetAllByEventoIdsAsync(eventoId);

                return _mapper.Map<RedeSocialDto[]>(RedeSocialRetorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> SaveByPalestrante(int palestranteId, RedeSocialDto[] models)
        {
            try
            {
                var RedeSocials = await _redeSocialPersiste.GetAllByPalestranteIdsAsync(palestranteId);
                if (RedeSocials == null) return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddRedeSocial(palestranteId, model, false);
                    }
                    else
                    {
                        var RedeSocial = RedeSocials.FirstOrDefault(RedeSocial => RedeSocial.Id == model.Id);
                        model.PalestranteId = palestranteId;

                        _mapper.Map(model, RedeSocial);

                        _redeSocialPersiste.Update<RedeSocial>(RedeSocial);

                        await _redeSocialPersiste.SaveChangesAsync();
                    }
                }

                var RedeSocialRetorno = await _redeSocialPersiste.GetAllByPalestranteIdsAsync(palestranteId);

                return _mapper.Map<RedeSocialDto[]>(RedeSocialRetorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteByEvento(int eventoId, int redeSocialId)
        {
            try
            {
                var RedeSocial = await _redeSocialPersiste.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
                if (RedeSocial == null) throw new Exception("Rede Social por Evento para delete não encontrado.");

                _redeSocialPersiste.Delete<RedeSocial>(RedeSocial);
                return await _redeSocialPersiste.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteByPalestrante(int palestranteId, int redeSocialId)
        {
            try
            {
                var RedeSocial = await _redeSocialPersiste.GetRedeSocialPalestranteByIdsAsync(palestranteId, redeSocialId);
                if (RedeSocial == null) throw new Exception("Rede Social por Palestrante para delete não encontrado.");

                _redeSocialPersiste.Delete<RedeSocial>(RedeSocial);
                return await _redeSocialPersiste.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> GetAllByEventoIdAsync(int eventoId)
        {
            try
            {
                var RedeSocials = await _redeSocialPersiste.GetAllByEventoIdsAsync(eventoId);
                if (RedeSocials == null) return null;

                var resultado = _mapper.Map<RedeSocialDto[]>(RedeSocials);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> GetAllByPalestranteIdAsync(int palestranteId)
        {
            try
            {
                var RedeSocials = await _redeSocialPersiste.GetAllByPalestranteIdsAsync(palestranteId);
                if (RedeSocials == null) return null;

                var resultado = _mapper.Map<RedeSocialDto[]>(RedeSocials);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto> GetRedeSocialEventoByIdsAsync(int eventoId, int redeSocialId)
        {
            try
            {
                var RedeSocial = await _redeSocialPersiste.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
                if (RedeSocial == null) return null;

                var resultado = _mapper.Map<RedeSocialDto>(RedeSocial);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int redeSocialId)
        {
            try
            {
                var RedeSocial = await _redeSocialPersiste.GetRedeSocialPalestranteByIdsAsync(palestranteId, redeSocialId);
                if (RedeSocial == null) return null;

                var resultado = _mapper.Map<RedeSocialDto>(RedeSocial);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}