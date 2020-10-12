using WebStoreDAO.CoreDAO;
using WebStoreModel.Entities;

namespace WebStoreDAO.EntityDAO
{
    public class UsuarioDAO : GenericDAO<Usuario>
    {
        public UsuarioDAO(WebStoreContext context) 
            : base(context)
        {
        }
    }
}