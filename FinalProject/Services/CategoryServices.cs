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
    public class CategoryServices
    {
        private readonly ItemsContextForAllTables m_db;
        private readonly IMapper Mapper;
        private readonly IHostingEnvironment _host;
        public CategoryServices(ItemsContextForAllTables db, IMapper _Mapper, IHostingEnvironment host)
        {
            m_db = db;
            Mapper = _Mapper;
            _host = host;
        }

        public async Task<List<Category>> GetCategorys()
        {
            return m_db.Category.ToList<Category>();
        }

        public Category GetCategory(int? Id)
        {
            var e = m_db.Category.Where(cat => cat.id == Id).Include(CP=>CP.products).FirstOrDefault();
            return e;
        }

        public async Task<bool> AddCategory(CategoryDTO Category,IFormFile img)
        {
            Category newItem = new Category();
            var timeStamp = GetTimestamp(DateTime.Now);
            var filePath = Path.Combine(_host.WebRootPath + "/images/category", timeStamp + img.FileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {

                await img.CopyToAsync(fileStream);
                newItem.picture = timeStamp + img.FileName;
            }
            newItem.categoryName = Category.categoryName;
            newItem.Create_AT = DateTime.Now.ToLocalTime();
            newItem.Update_AT = DateTime.Now.ToLocalTime();
            m_db.Category.Add(newItem);

            int c = await m_db.SaveChangesAsync();
            return c > 0;
        }

        public async Task <bool> AddCategory(CategoryDTO Categ)
        {
                Category newItem = new Category();
                newItem.categoryName = Categ.categoryName;
                newItem.Create_AT = DateTime.Now.ToLocalTime();
                newItem.Update_AT = DateTime.Now.ToLocalTime();
                m_db.Category.Add(newItem);
          
                int c = await m_db.SaveChangesAsync();
                return c > 0;
        }

        public async Task<bool> AddCategory(CategoryDTO Categ,ProductsDTO pro)
        {

            Category newItem = new Category();
            newItem.categoryName = Categ.categoryName;
            m_db.Category.Add(newItem);

            int c = await m_db.SaveChangesAsync();
            return c > 0;
        }


        public ResponseDTO DeleteCategory(int id)
        {

            // Items itemToDelete = m_db.Items.Where(item =>item.id == id).FirstOrDefault();
            Category itemToDelete = GetCategory(id);
            if (itemToDelete == null)//בדיקה אם הישות קיים במאגר נתוניים
            {
                return new ResponseDTO() {
                    Status = StatusCode.Error,
                    StatusText = $"Entity With id:{id} not in DB" 
                };
            }
            
            m_db.Category.Remove(itemToDelete);
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

        public ResponseDTO DeleteCategory(int deleteId,int changeId)
        {

            // Items itemToDelete = m_db.Items.Where(item =>item.id == id).FirstOrDefault();
            Category itemToDelete = GetCategory(deleteId);
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
                itemToDelete.products[i].categoryId = changeId;
                m_db.Product.Update(itemToDelete.products[i]);
            }
            itemToDelete.products = null;
            m_db.Category.Remove(itemToDelete);
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
        public async Task<ResponseDTO> UpdetCategoryAsync(int id, CategoryDTO newCategory,IFormFile img)
        {

            // Items itemToDelete = m_db.Items.Where(item =>item.id == id).FirstOrDefault();
            Category CategoryToUpdet = GetCategory(id);
            if (CategoryToUpdet == null)//בדיקה אם הישות קיים במאגר נתוניים
            {
                return new ResponseDTO()
                {
                    Status = StatusCode.Error,
                    StatusText = $"Entity With id:{id} not in DB"
                };
            }
            if (img != null)
            {
                try
                {
                    var a = File.Exists(Path.Combine(_host.WebRootPath + "/images/category", CategoryToUpdet.picture));
                    if (a)
                    {
                        File.Delete(Path.Combine(_host.WebRootPath + "/images/category", CategoryToUpdet.picture));
                    }
                }
                catch (Exception) { Console.WriteLine(); }

                var timeStamp = GetTimestamp(DateTime.Now);
                var filePath = Path.Combine(_host.WebRootPath + "/images/category", timeStamp + img.FileName);

                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        
                        await img.CopyToAsync(fileStream);
                        CategoryToUpdet.picture = timeStamp + img.FileName;
                    }

                

            }
            else {
                CategoryToUpdet.picture = CategoryToUpdet.picture;
            }
            CategoryToUpdet.categoryName = newCategory.categoryName;
            CategoryToUpdet.Create_AT = CategoryToUpdet.Create_AT;
            CategoryToUpdet.Update_AT = DateTime.Now.ToLocalTime();
            

            m_db.Category.Update(CategoryToUpdet);
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
