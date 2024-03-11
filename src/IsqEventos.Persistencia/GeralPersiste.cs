
using IsqEventos.Persistencia.Contatos;
using IsqEventos.Persistencia.contextos;


namespace IsqEventos.Persistencia
{
    public class GeralPersiste : IGeralPersistencia
    {

        private readonly IsqEventosContext _context;

        public GeralPersiste(IsqEventosContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void DeleteRange<T>(T[] entityArray) where T : class
        {
            _context.RemoveRange(entityArray);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }



    }
}