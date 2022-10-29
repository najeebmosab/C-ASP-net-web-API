using AutoMapper;
using FinalProject.Datas;
using FinalProject.Datas.DTO;
using FinalProject.Datas.Edintites;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Services
{
    public class ProductsServices
    {

        private readonly ItemsContextForAllTables m_db;
        private readonly ImagesServices imgService;
        private readonly CategoryServices categoryServices;
        private readonly IHostingEnvironment _host;
        private readonly IMapper Mapper;

        public ProductsServices(ItemsContextForAllTables db, CategoryServices _categoryServices, ImagesServices _imgService, IMapper _Mapper, IHostingEnvironment host)
        {
            m_db = db;
            categoryServices = _categoryServices;
            imgService = _imgService;
            Mapper = _Mapper;
            _host = host;
        }


        public async Task<List<ProductsDTO>> GetProducts()
        {
            var e = m_db.Product.Select(ee => new ProductsDTO()
            {
                id = ee.id,
                imageProduct = ee.imageProduct,
                productName = ee.productName,
                shortDescription = ee.shortDescription,
                gender = ee.gender,
                price = ee.price,
                salePrice = ee.salePrice,
                categoryId = ee.categoryId,
                category = ee.category,
                brandId = ee.brandId,
                brand = ee.brand,

            }).ToList();
            return e;
        }

        public async Task<List<Products>> GetProductByCategoryId(int id) 
        {
            return await m_db.Product.Where(P => P.categoryId == id).ToListAsync();
        }

        public async Task<List<Products>> GetAllProductsForAdmin()
        {
            return await m_db.Product.ToListAsync();

        }

        public async Task<List<Products>> GetnewProduct() 
        {
            return await m_db.Product.OrderByDescending(P=>P.Create_AT).Take(3).ToListAsync();
        }


        public async Task<List<Products>> GetProductsName(string name)
        {
            if(!String.IsNullOrEmpty(name)){ 
            var e = await m_db.Product.Where(
                P => 
                P.productName.Contains(name) || P.description.Contains(name) || P.shortDescription.Contains(name)
                ).ToListAsync();
                return e;
            }
            return null;
        }

        public async Task<List<Products>> GetDisCountProducts()
        {
            return await m_db.Product.Where(P => P.salePrice > 0).OrderByDescending(P => P.salePrice).Take(9).ToListAsync();
        }
        public async Task<Products> GetProduct(int id)
        {
            //return await m_db.Product.Where(P => P.id == id).FirstOrDefault();

            var e = m_db.Product.Where(p => p.id == id).Include(CP => CP.category).Include(BP => BP.brand).Include(I => I.Image).FirstOrDefault();
            return e;
        }
        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssff");
        }

        public  async Task<Products> AddProducts(ProductsDTO Produ,IFormFile img)
        {
                Products newProducts = new Products();

                newProducts.id = Produ.id;
                newProducts.productName = Produ.productName;
                newProducts.price = Produ.price;
                newProducts.salePrice = Produ.salePrice;
                newProducts.description = Produ.description;
                newProducts.shortDescription = Produ.shortDescription;
                newProducts.quantity = Produ.quantity;
                newProducts.actualPrice = Produ.actualPrice;
                newProducts.gender = Produ.gender;
                newProducts.categoryId = Produ.categoryId;
                newProducts.brandId = Produ.brandId;
                newProducts.Create_AT = DateTime.Now.ToLocalTime();
                newProducts.Update_AT = DateTime.Now.ToLocalTime();
                String timeStamp = GetTimestamp(DateTime.Now);
                var filePath = Path.Combine(_host.WebRootPath + "/images/products",timeStamp+img.FileName);
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await img.CopyToAsync(fileStream);
                }
                newProducts.imageProduct = timeStamp + img.FileName;
                m_db.Product.Add(newProducts);
                int c = await m_db.SaveChangesAsync();

                if (c > 0) 
                {
                    return newProducts;
                }
                return null;
        }

   

        public  async Task <ResponseDTO> DeleteProducts(int id)
        {
            Products ProductsToDelete = await GetProduct(id);
            if (ProductsToDelete == null)//בדיקה אם הישות קיים במאגר נתוניים
            {
                return new ResponseDTO()
                {
                    Status = StatusCode.Error,
                    StatusText = $"Entity With id:{id} not in DB"
                };
            }
            var a = File.Exists(Path.Combine(_host.WebRootPath + "/images/products", ProductsToDelete.imageProduct));
             if (a)
            {
                File.Delete(Path.Combine(_host.WebRootPath + "/images/products", ProductsToDelete.imageProduct));
            }

            IList<Images> img = ProductsToDelete.Image;

            for (int i = 0; i < img.Count; i++)
            {
                this.imgService.DeleteImages(img[i].id);
            }

            m_db.Product.Remove(ProductsToDelete);
            int c = m_db.SaveChanges();
            ResponseDTO response = new ResponseDTO();
            if (c > 0)
            {
                response.Status = StatusCode.Success;
            }
            else
            {
                response.Status = StatusCode.Error;
                response.StatusText = "Error";
            }
            return response;
        }


        public async Task <ResponseDTO> UpdateProducts(int id, ProductsDTO newProducts,IFormFile img)
        {

            // Items itemToDelete = m_db.Items.Where(item =>item.id == id).FirstOrDefault();
            Products ProductsToUpdet = await GetProduct(id);
            if (ProductsToUpdet == null)//בדיקה אם הישות קיים במאגר נתוניים
            {
                return new ResponseDTO()
                {
                    Status = StatusCode.Error,
                    StatusText = $"Entity With id:{id} not in DB"
                };
            }

            
            String timeStamp = GetTimestamp(DateTime.Now);
            if (img != null && img.Length > 0)
            {
                var filePath = Path.Combine(_host.WebRootPath + "/images/products", timeStamp + img.FileName);
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    var a = File.Exists(Path.Combine(_host.WebRootPath + "/images/products", ProductsToUpdet.imageProduct));
                    if (a)
                    {
                        File.Delete(Path.Combine(_host.WebRootPath + "/images/products", ProductsToUpdet.imageProduct));
                    }
                    await img.CopyToAsync(fileStream);
                    ProductsToUpdet.imageProduct = timeStamp+img.FileName;

                }
            }
            else {
                ProductsToUpdet.imageProduct = newProducts.imageProduct;
            }
            ProductsToUpdet.productName = newProducts.productName;
            ProductsToUpdet.price = newProducts.price;
            ProductsToUpdet.salePrice = newProducts.salePrice;
            ProductsToUpdet.description = newProducts.description;
            ProductsToUpdet.shortDescription = newProducts.shortDescription;
            ProductsToUpdet.quantity = newProducts.quantity;
            ProductsToUpdet.actualPrice = newProducts.actualPrice;
            ProductsToUpdet.gender = newProducts.gender;
            ProductsToUpdet.categoryId = newProducts.categoryId;
            ProductsToUpdet.brandId = newProducts.brandId;
            ProductsToUpdet.Create_AT = ProductsToUpdet.Create_AT;
            ProductsToUpdet.Update_AT = DateTime.Now.ToLocalTime();

            m_db.Product.Update(ProductsToUpdet);
            int c = await m_db.SaveChangesAsync();
           
            ResponseDTO response = new ResponseDTO();
            if (c > 0)
            {
                response.Status = StatusCode.Success;
            }
            else
            {
                response.Status = StatusCode.Error;
                response.StatusText = "Error";
            }
            return response;
        }
    }
}
