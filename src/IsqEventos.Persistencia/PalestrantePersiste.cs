using IsqEventos.Domain;
using IsqEventos.Persistencia.contextos;
using IsqEventos.Persistencia.Contratos;
using IsqEventos.Persistencia.Models;
using Microsoft.EntityFrameworkCore;

namespace IsqEventos.Persistencia
{
    public class PalestrantePersiste : GeralPersiste, IPalestrantesPersistencia
    {

        private readonly IsqEventosContext _context;

        public PalestrantePersiste(IsqEventosContext context) : base(context)
        {
            _context = context;
        }
        public async Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
            .Include(p => p.User)
            .Include(e => e.RedesSociais);

            if (includeEventos)

            {
                query = query
                .Include(p => p.PalestrantesEventos)
                .ThenInclude(pe => pe.Evento);
            }

            query = query.AsNoTracking()
                                .Where(p => (p.MiniCurriculo.ToLower().Contains(pageParams.Term.ToLower()) ||
                                      p.User.PrimeiroNome.ToLower().Contains(pageParams.Term.ToLower()) ||
                                      p.User.UltimoNome.ToLower().Contains(pageParams.Term.ToLower())) &&
                                      p.User.Funcao == Domain.Enum.Funcao.Palestrante)
                         .OrderBy(p => p.Id);

            return await PageList<Palestrante>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<Palestrante> GetPalestranteByUserIdAsync(int userId, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
            .Include(p => p.User)
          .Include(e => e.RedesSociais);

            if (includeEventos)

            {
                query = query
                .Include(p => p.PalestrantesEventos)
                .ThenInclude(pe => pe.Evento);
            }

            query = query.AsNoTracking().OrderBy(p => p.Id)
                        .Where(p => p.UserId == userId);

            return await query.FirstOrDefaultAsync();
        }


    }
}