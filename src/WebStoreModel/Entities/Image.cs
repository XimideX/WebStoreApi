using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using WebStoreModel.CoreModel;

namespace WebStoreModel.Entities
{
    public class Image : BaseModel
    {
        public string Name { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}