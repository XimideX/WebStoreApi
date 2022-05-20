using WebStoreDAO.CoreDAO;
using WebStoreModel.Entities;

namespace WebStoreDAO.EntityDAO
{
    public class ImageDAO : GenericDAO<Image>
    {
        public ImageDAO(WebStoreContext _context) : base(_context)
        {
        }
    }
}