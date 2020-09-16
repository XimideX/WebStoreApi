
using WebStoreDAO.CoreDAO;
using WebStoreModel.Entities;

namespace WebStoreDAO.EntityDAO
{
    public class ProductDAO : GenericDAO<Product>
    {
        public ProductDAO(WebStoreContext _context)
            :base(_context)
        {

        }
        
    }
}