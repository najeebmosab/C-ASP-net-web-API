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
    public class ImagesServices
    {
        private readonly ItemsContextForAllTables m_db;//using this DBContext to get and coniction Data btween Table from Data in sql and services
        private readonly IHostingEnvironment _host;

        public ImagesServices(ItemsContextForAllTables db, IHostingEnvironment host)
        {
            m_db = db; // task a equal value from db mean data from table and throw in virable in m_db
            _host = host;
        }

        //name of class parallel to table from data

        public List<Images> GetImages() 
        {
            return m_db.Images.ToList<Images>();
        }

        public Images GetImage(int Id)
        {
            var e = m_db.Images.Where(I => I.id == Id).FirstOrDefault();
            return e;
        }

        public async Task<List<Images>> GetImagesByProductId(int Id)
        {

            return  m_db.Images.Where(IP => IP.productId == Id).ToList();
             
        }

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmss");
        }

        public async Task< bool> AddImages(List<IFormFile> formFiles,int id)
        {
            int c = 0;
            var timeStamp = GetTimestamp(DateTime.Now);
            for (int i = 0; i < formFiles.Count;i++)
            {
                Images newImg = new Images();
                double num = double.Parse(timeStamp) + i;
                timeStamp = num.ToString();
                var filePath = Path.Combine(_host.WebRootPath + "/images/productImages", timeStamp + formFiles[i].FileName);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                   await formFiles[i].CopyToAsync(fileStream);
                }

                newImg.Path = timeStamp + formFiles[i].FileName;
                newImg.productId = id;
                newImg.Create_AT = DateTime.Now.ToLocalTime();
                newImg.Update_AT = DateTime.Now.ToLocalTime();
                m_db.Images.AddAsync(newImg);

                c = await m_db.SaveChangesAsync();
            }
            return c > 0;

        }

        public async Task<bool> AddImages(int id,Images img)
        {
            Images newImg = new Images();
            newImg.Path = img.Path;
            newImg.productId = id;
            newImg.Create_AT = DateTime.Now.ToLocalTime();
            newImg.Update_AT = DateTime.Now.ToLocalTime();
            m_db.Images.AddAsync(newImg);
            int c = await m_db.SaveChangesAsync();
            return c > 0;

        }

        

        public ResponseDTO DeleteImages(int id)
        {
            Images deleteImages = GetImage(id);

            if (deleteImages == null)
            {
                return new ResponseDTO() { Status = StatusCode.Error, StatusText = $"Entity With id:{id} not in DB" };
            }




            var a = File.Exists(Path.Combine(_host.WebRootPath + "/images/productImages", deleteImages.Path));
            if (a)
            {
                File.Delete(Path.Combine(_host.WebRootPath + "/images/productImages", deleteImages.Path));
            }

            m_db.Images.Remove(deleteImages);
            int c =  m_db.SaveChanges();
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
        public async Task<ResponseDTO> UpdateImagesAsync(int id, List<IFormFile> img)
        {

            // Items itemToDelete = m_db.Items.Where(item =>item.id == id).FirstOrDefault();
            Images ImagesToUpdate = GetImage(id);
            if (ImagesToUpdate == null)//בדיקה אם הישות קיים במאגר נתוניים
            {
                return new ResponseDTO()
                {
                    Status = StatusCode.Error,
                    StatusText = $"Entity With id:{id} not in DB"
                };
            }
            var a = File.Exists(Path.Combine(_host.WebRootPath + "/images/productImages", ImagesToUpdate.Path));
            if (a)
            {
                File.Delete(Path.Combine(_host.WebRootPath + "/images/productImages", ImagesToUpdate.Path));
            }

            int c = 0;
            var timeStamp = GetTimestamp(DateTime.Now);

            for (int i = 0; i < img.Count; i++)
            {
                
                double num = double.Parse(timeStamp) + i;
                timeStamp = num.ToString();
                var filePath = Path.Combine(_host.WebRootPath + "/images/productImages", timeStamp + img[i].FileName);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await img[i].CopyToAsync(fileStream);
                }

                ImagesToUpdate.Path = timeStamp + img[i].FileName;
                ImagesToUpdate.productId = ImagesToUpdate.productId;
                ImagesToUpdate.Create_AT = ImagesToUpdate.Create_AT;
                ImagesToUpdate.Update_AT = DateTime.Now.ToLocalTime();
                m_db.Images.Update(ImagesToUpdate);
                c =  m_db.SaveChanges();
            }
            
            
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

        public async Task<ResponseDTO> UpdateImagesAsync(int id, Images img)
        {

            // Items itemToDelete = m_db.Items.Where(item =>item.id == id).FirstOrDefault();
            Images ImagesToUpdate = GetImage(id);
            if (ImagesToUpdate == null)//בדיקה אם הישות קיים במאגר נתוניים
            {
                return new ResponseDTO()
                {
                    Status = StatusCode.Error,
                    StatusText = $"Entity With id:{id} not in DB"
                };
            }

            ImagesToUpdate.id = img.id;
            ImagesToUpdate.Path = img.Path;
            ImagesToUpdate.productId = img.productId;


            m_db.Images.Update(ImagesToUpdate);
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

        //internal void UpdateImages(IList<Images> image)
        //{
        //    Images newImg = new Images();
        //    int coun = image.Count;
        //    int c = 0;
        //    for (int i = 0; i < coun; i++)
        //    {

        //        UpdateImages(image[i].ImageID,image[i]);
        //    }

        //}
    }

}
