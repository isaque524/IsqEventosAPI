using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsqEventos.Domain;
using IsqEventos.Persistencia.contextos;
using IsqEventos.Persistencia.Contratos;
using Microsoft.EntityFrameworkCore;

namespace IsqEventos.Persistencia
{
    public class RedeSocialPersiste : GeralPersiste, IRedeSocialPersistencia
    {
        private readonly IsqEventosContext _context;

        public RedeSocialPersiste(IsqEventosContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RedeSocial> GetRedeSocialEventoByIdsAsync(int eventoId, int id)
        {

            IQueryable<RedeSocial> query = _context.RedesSociais;

            query = query.AsNoTracking().Where(rs => rs.EventoId == eventoId && rs.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<RedeSocial> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int id)
        {

            IQueryable<RedeSocial> query = _context.RedesSociais;
            query = query.AsNoTracking().Where(rs => rs.PalestranteId == palestranteId && rs.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<RedeSocial[]> GetAllByEventoIdsAsync(int eventoId)
        {

            IQueryable<RedeSocial> query = _context.RedesSociais;

            query = query.AsNoTracking().Where(rs => rs.EventoId == eventoId);

            return await query.ToArrayAsync();
        }

        public async Task<RedeSocial[]> GetAllByPalestranteIdsAsync(int palestranteId)
        {

            IQueryable<RedeSocial> query = _context.RedesSociais;

            query = query.AsNoTracking().Where(rs => rs.PalestranteId == palestranteId);

            return await query.ToArrayAsync();
        }

    }

}