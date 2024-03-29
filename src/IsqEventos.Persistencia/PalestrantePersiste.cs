using IsqEventos.Domain;
using IsqEventos.Persistencia.Contatos;
using IsqEventos.Persistencia.contextos;
using Microsoft.EntityFrameworkCore;

namespace IsqEventos.Persistencia
{
    public class PalestrantePersiste : IPalestrantesPersistencia
    {

        private readonly IsqEventosContext _context;

        public PalestrantePersiste(IsqEventosContext context)
        {
            _context = context;
        }
        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
          .Include(e => e.RedesSociais);

            if (includeEventos)

            {
                query = query
                .Include(p => p.PalestrantesEventos)
                .ThenInclude(pe => pe.Evento);
            }

            query = query.AsNoTracking().OrderBy(p => p.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
          .Include(e => e.RedesSociais);

            if (includeEventos)

            {
                query = query
                .Include(p => p.PalestrantesEventos)
                .ThenInclude(pe => pe.Evento);
            }

            query = query.AsNoTracking().OrderBy(p => p.Id)
                        .Where(p => p.Nome.ToLower().Contains(nome.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante> GetPalestranteByIdAsync(int palestranteId, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
          .Include(e => e.RedesSociais);

            if (includeEventos)

            {
                query = query
                .Include(p => p.PalestrantesEventos)
                .ThenInclude(pe => pe.Evento);
            }

            query = query.AsNoTracking().OrderBy(p => p.Id)
                        .Where(p => p.Id == palestranteId);

            return await query.FirstOrDefaultAsync();
        }


    }
}