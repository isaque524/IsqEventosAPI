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
    public class EventoService : IEventosService
    {

        private readonly IGeralPersistencia _geralPersistencia;
        private readonly IEventosPersistencia _eventosPersistencia;

        private readonly IMapper _mapper;

        public EventoService(IGeralPersistencia geralPersiste,
                            IEventosPersistencia eventosPersiste,
                            IMapper mapper)
        {
            _geralPersistencia = geralPersiste;
            _eventosPersistencia = eventosPersiste;
            _mapper = mapper;
        }


        public async Task<EventoDto> addEventos(int userId, EventoDto model)
        {

            try
            {
                var evento = _mapper.Map<Evento>(model);
                evento.UserId = userId;

                _geralPersistencia.Add<Evento>(evento);
                if (await _geralPersistencia.SaveChangesAsync())
                {
                    var eventoRetorno = await _eventosPersistencia.GetEventoByIdAsync(userId, evento.Id, false);
                    return _mapper.Map<EventoDto>(eventoRetorno);
                }
                return null;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        public async Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto model)
        {

            try
            {
                var evento = await _eventosPersistencia.GetEventoByIdAsync(userId, eventoId, false);
                if (evento == null) return null;

                model.Id = evento.Id;
                model.UserId = userId;

                _mapper.Map(model, evento);

                _geralPersistencia.Update<Evento>(evento);
                if (await _geralPersistencia.SaveChangesAsync())
                {
                    var eventoRetorno = await _eventosPersistencia.GetEventoByIdAsync(userId, evento.Id, false);
                    return _mapper.Map<EventoDto>(eventoRetorno);
                }
                return null;


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> DeleteEvento(int userId, int eventoId)
        {
            try
            {
                var evento = await _eventosPersistencia.GetEventoByIdAsync(userId, eventoId, false);
                if (evento == null) throw new Exception("Evento para DELETE não encontrado.");

                foreach (var redeSocial in evento.RedesSociais.ToList())
                {
                    _geralPersistencia.Delete<RedeSocial>(redeSocial);
                }


                _geralPersistencia.Delete<Evento>(evento);
                return await _geralPersistencia.SaveChangesAsync();


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        public async Task<EventoDto[]> GetAllEventosAsync(int userId, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventosPersistencia.GetAllEventosAsync(userId, includePalestrantes);
                if (eventos == null) return null;

                var resultado = _mapper.Map<EventoDto[]>(eventos);

                return resultado;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventosPersistencia.GetAllEventosByTemaAsync(userId, tema, includePalestrantes);
                if (eventos == null) return null;

                var resultado = _mapper.Map<EventoDto[]>(eventos);

                return resultado;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var evento = await _eventosPersistencia.GetEventoByIdAsync(userId, eventoId, includePalestrantes);
                if (evento == null) return null;

                var resultado = _mapper.Map<EventoDto>(evento);

                return resultado;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


    }
}