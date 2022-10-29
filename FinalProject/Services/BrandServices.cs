using AutoMapper;
using FinalProject.Datas;
using FinalProject.Datas.DTO;
using FinalProject.Datas.Edintites;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Services
{
    public class BrandServices
    {
        private readonly ItemsContextForAllTables m_db;
        private readonly IHostingEnvironment _host;
        private readonly IMapper Mapper;

        public BrandServices(ItemsContextForAllTables db, IMapper _Mapper, IHostingEnvironment host)
        {
            m_db = db;
            Mapper = _Mapper;
            _host = host;
        }

        public async Task<List<Brands>> GetBrands()
        {
            return m_db.Brands.ToList<Brands>();
        }

        public async Task<Brands> GetBrand(int id)
        {
            return m_db.Brands.Where(B=>B.id == id).FirstOrDefault();
        }

        public async Task<Brands> AddBrands(BrandsDTO brand,IFormFile img)
        {
            Brands newItems = new Brands();

            newItems.name = brand.name;
            var timeStamp = GetTimestamp(DateTime.Now);
            var filePath = Path.Combine(_host.WebRootPath + "/images/brands", timeStamp + img.FileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await img.CopyToAsync(fileStream);
                newItems.image = timeStamp + img.FileName;
            }

            newItems.Create_AT = DateTime.Now.ToLocalTime();
            newItems.Update_AT = DateTime.Now.ToLocalTime();
            m_db.Brands.Add(newItems);
            int c = await m_db.SaveChangesAsync();

            if (c > 0)
            {
                return newItems;
            }
            return null;
        }

        public async Task<ResponseDTO> DeleteBrands(int id)
        {
            Brands itemsToDelete = await GetBrand(id);
            if (itemsToDelete == null)//בדיקה אם הישות קיים במאגר נתוניים
            {
                return new ResponseDTO()
                {
                    Status = StatusCode.Error,
                    StatusText = $"Entity With id:{id} not in DB"
                };
            }
            
            m_db.Brands.Remove(itemsToDelete);
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

        public async Task<ResponseDTO> DeleteBrandAsync(int deleteId, int changeId)
        {

            Brands itemToDelete = await GetBrand(deleteId);
            if (itemToDelete == null)//בדיקה אם הישות קיים במאגר נתוניים
            {
                return new ResponseDTO()
                {
                    Status = StatusCode.Error,
                    StatusText = $"Entity With id:{deleteId} not in DB"
                };
            }
            for (int i = 0; i < itemToDelete.products.Count; i++)
            {
                itemToDelete.products[i].brandId = changeId;
                m_db.Product.Update(itemToDelete.products[i]);
            }
            itemToDelete.products = null;
            m_db.Brands.Remove(itemToDelete);
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

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmss");
        }
        public async Task<ResponseDTO> UpdateBrands(int id, BrandsDTO newBrands,IFormFile img)
        {

            // Items itemToDelete = m_db.Items.Where(item =>item.id == id).FirstOrDefault();
            Brands itemsToUpdet = await GetBrand(id);
            if (itemsToUpdet == null)//בדיקה אם הישות קיים במאגר נתוניים
            {
                return new ResponseDTO()
                {
                    Status = StatusCode.Error,
                    StatusText = $"Entity With id:{id} not in DB"
                };
            }

            itemsToUpdet.name = newBrands.name;
            if (img != null)
            {
                try
                {
                    var a = File.Exists(Path.Combine(_host.WebRootPath + "/images/category", itemsToUpdet.image));
                    if (a)
                    {
                        File.Delete(Path.Combine(_host.WebRootPath + "/images/brands", itemsToUpdet.image));
                    }
                }
                catch (Exception) { Console.WriteLine(); }

                var timeStamp = GetTimestamp(DateTime.Now);
                var filePath = Path.Combine(_host.WebRootPath + "/images/brands", timeStamp + img.FileName);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {

                    await img.CopyToAsync(fileStream);
                    itemsToUpdet.image = timeStamp + img.FileName;
                }
            }
            itemsToUpdet.Create_AT = itemsToUpdet.Create_AT;
            itemsToUpdet.Update_AT = DateTime.Now.ToLocalTime();

            m_db.Brands.Update(itemsToUpdet);
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

    }
}
