using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsqEventos.Application.Contratos;
using IsqEventos.Domain;
using IsqEventos.Persistencia.Contatos;

namespace IsqEventos.Application
{
    public class EventoService : IEventosService
    {

        private readonly IGeralPersistencia _geralPersistencia;
        private readonly IEventosPersistencia _eventosPersistencia;
        public EventoService(IGeralPersistencia geralPersiste, IEventosPersistencia eventosPersiste)
        {
            _geralPersistencia = geralPersiste;
            _eventosPersistencia = eventosPersiste;
        }


        public async Task<Evento> addEventos(Evento model)
        {
            try
            {
                _geralPersistencia.Add<Evento>(model);
                if (await _geralPersistencia.SaveChangesAsync())
                {
                    return await _eventosPersistencia.GetEventoByIdAsync(model.Id, false);
                }
                return null;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        public async Task<Evento> UpdateEvento(int eventoId, Evento model)
        {
            try
            {
                var evento = await _eventosPersistencia.GetEventoByIdAsync(eventoId, false);
                if (evento == null) return null;

                model.Id = evento.Id;

                _geralPersistencia.Update(model);
                if (await _geralPersistencia.SaveChangesAsync())
                {
                    return await _eventosPersistencia.GetEventoByIdAsync(model.Id, false);
                }
                return null;


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> DeleteEvento(int eventoId)
        {
            try
            {
                var evento = await _eventosPersistencia.GetEventoByIdAsync(eventoId, false);
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


        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventosPersistencia.GetAllEventosAsync(includePalestrantes);
                if (eventos == null) return null;

                return eventos;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventosPersistencia.GetAllEventosByTemaAsync(tema, includePalestrantes);
                if (eventos == null) return null;

                return eventos;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventosPersistencia.GetEventoByIdAsync(eventoId, includePalestrantes);
                if (eventos == null) return null;

                return eventos;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


    }
}