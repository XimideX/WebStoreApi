using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IAmazonS3 amazonS3;


        public ProductController(ILogger<ProductController> logger, UnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment, IAmazonS3 amazonS3)
        {
            this.unitOfWork = unitOfWork;
            this.amazonS3 = amazonS3;
            // this.amazonS3 = options.CreateServiceClient<IAmazonS3>();
          
            _logger = logger;
        }

        // [Authorize]
        [HttpGet("[action]")]
        public List<Product> SelectAll()
        {
            var lista = this.unitOfWork.ProductDAO.SelectAll().ToList();
            return lista;
        }

        [HttpPost("[action]")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> InsertImage([FromForm] IFormFile image)
        {
            using (var client = new AmazonS3Client("AKIA3DDKICZFJNM5CRUH", "d0GEDb4Ahap/lViHPPxzouuAQxWhsZquazgPAI+u", Amazon.RegionEndpoint.SAEast1))
            {
                var putRequest = new PutObjectRequest()
                {
                    BucketName = "upload-photos-menulabel-bucket",
                    Key = image.FileName,
                    InputStream = image.OpenReadStream(),
                    ContentType = image.ContentType
                };
                var result = await client.PutObjectAsync(putRequest);
                return Ok(result);
            }

        }

        [HttpGet("[action]")]
        public Task<string> GetImage()
        {
            using (var client = new AmazonS3Client("AKIA3DDKICZFJNM5CRUH", "d0GEDb4Ahap/lViHPPxzouuAQxWhsZquazgPAI+u", Amazon.RegionEndpoint.SAEast1))
            {
                string uploadedFileUrl = client.GetPreSignedURL(new GetPreSignedUrlRequest()
                { 
                    BucketName = "upload-photos-menulabel-bucket",
                    Key = "eu.jpeg",
                    Expires = DateTime.Now.AddYears(5)
                });
                Console.WriteLine("File download URL: {0}", uploadedFileUrl);
                return Task.FromResult((uploadedFileUrl));

                // TransferUtility fileTransferUtility =
                // new TransferUtility(
                //     new AmazonS3Client("AKIA3DDKICZFJNM5CRUH", "d0GEDb4Ahap/lViHPPxzouuAQxWhsZquazgPAI+u", Amazon.RegionEndpoint.CACentral1));

                // // Note the 'fileName' is the 'key' of the object in S3 (which is usually just the file name)
                // fileTransferUtility.Download("C:\\Users\\rschm\\Documents\\ImagesAWS", "upload-photos-menulabel-bucket", "eu.jpeg");
                
                // var response = await client.GetObjectAsync("upload-photos-menulabel-bucket", "eu.jpeg");
                // using (var reader = new StreamReader(response.ResponseStream))
                // {
                //     var image = await reader.ReadToEndAsync().ConfigureAwait(false);
                        
                //     // return Ok(image);
                //     return File(image, "image/jpeg");
                // }
            }
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
            unitOfWork.Save();
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