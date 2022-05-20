using System;
using Microsoft.EntityFrameworkCore;
using WebStoreDAO.CoreDAO;
using WebStoreDAO.EntityDAO;

namespace WebStoreDAO.CoreDAO
{
    public class UnitOfWork
    {
        private bool disposed = false;

        private WebStoreContext _context;
        public UnitOfWork(WebStoreContext webStoreContext) {
            _context = webStoreContext;
            _context.Database.OpenConnection();
        }

        private ProductDAO productDAO;
        public ProductDAO ProductDAO
        {
            get 
            {
                if (this.productDAO == null)
                {
                    this.productDAO = new ProductDAO(this._context);
                }
                return this.productDAO;
            }
        }

        private UsuarioDAO usuarioDAO;
        public UsuarioDAO UsuarioDAO
        {
            get 
            {
                if (this.usuarioDAO == null)
                {
                    this.usuarioDAO = new UsuarioDAO(this._context);
                }
                return this.usuarioDAO;
            }
        }

        private ImageDAO imageDAO;
        public ImageDAO ImageDAO
        {
            get 
            {
                if (this.imageDAO == null)
                {
                    this.imageDAO = new ImageDAO(this._context);
                }
                return this.imageDAO;
            }
        }

        public void Save()
        {
            try{
                _context.SaveChanges();
            } catch (DbUpdateException e)
            {
                foreach (var eve in e.Entries)
                {
                    Console.WriteLine("Entidade do tipo \"{0}\" no estado \"{1}\" tem os seguintes erros de validação:",
                        eve.Entity.GetType().Name, eve.State);
                }
                throw;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Database.CloseConnection();
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}