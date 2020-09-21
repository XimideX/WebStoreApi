using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStoreDAO.CoreDAO;
using WebStoreModel.Entities;

namespace WebStoreService.Controllers
{
    // [EnableCors(origins: "http://localhost:4200/", headers: "*", methods: "*")]
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly UnitOfWork unitOfWork;

        public ProductController(ILogger<ProductController> logger, UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet("[action]")]
        public List<Product> SelectAll()
        {
            return this.unitOfWork.ProductDAO.SelectAll().ToList();
        }

        [HttpGet("[action]")]
        public void DeleteOneById(int Id)
        {
            unitOfWork.ProductDAO.Delete(this.unitOfWork.ProductDAO.Select(p => p.Id == Id).FirstOrDefault());
            unitOfWork.Save();
        }

        [HttpGet("[action]")]
        public List<Product> SelectOneById(int id)
        {
            var product = new List<Product>();
            using (var db = new WebStoreContext())
            {
                // Read
                product = db.Products
                    .OrderBy(p => p.Id)
                    .ToList();
            }
            return product;
        }

        [HttpPost("[action]")]
        public Product InsertOrUpdateProduct(Product product)
        {
            if (product != null && product.Id > 0)
            {
                unitOfWork.ProductDAO.Update(product);
            }
            else
            {
                unitOfWork.ProductDAO.Insert(product);
            }
            return product;
        }

        // // POST: api/TodoItems
        // [HttpPost("[action]")]
        // public Product SetProduct(Product product)
        // {
        //     _context.Products.Add(product);
        //     _context.SaveChanges();
        //     return product;
        // }
    }
}