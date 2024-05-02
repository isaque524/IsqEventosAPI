
using IsqEventos.Domain;
using IsqEventos.Persistencia.Contatos;
using IsqEventos.Persistencia.contextos;
using Microsoft.EntityFrameworkCore;

namespace IsqEventos.Persistencia
{
    public class LotePersiste : ILotePersistencia
    {

        private readonly IsqEventosContext _context;

        public LotePersiste(IsqEventosContext context)
        {
            _context = context;
        }

        public async Task<Lote> GetLoteByIdsAsync(int eventoId, int id)
        {
            IQueryable<Lote> query = _context.Lotes;

            query = query.AsNoTracking()
                        .Where(lote => lote.EventoId == eventoId
                                     && lote.Id == id);

            return await query.FirstOrDefaultAsync();
        }


        public async Task<Lote[]> GetLotesByEventoIdAsync(int eventoId)
        {

            IQueryable<Lote> query = _context.Lotes;

            query = query.AsNoTracking()
                        .Where(lote => lote.EventoId == eventoId);

            return await query.ToArrayAsync();
        }
    }
}