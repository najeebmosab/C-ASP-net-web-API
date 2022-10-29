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
    public class SlidersServices
    {
        private readonly ItemsContextForAllTables m_db;
        private readonly IHostingEnvironment _host;
        public SlidersServices(ItemsContextForAllTables db, IHostingEnvironment host)
        {
            m_db = db;
            _host = host;
        }

        public List<Sliders> GetSliders()
        {
            return m_db.Slider.ToList<Sliders>();
        }

        public Sliders GetSlider(int Id)
        {
            var e = m_db.Slider.Where(i => i.id == Id).FirstOrDefault();//lampada

            return e;
        }
        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
        public async Task<Sliders> AddSliders(SlidersDTO Slider,IFormFile img)
        {
            Sliders newItem = new Sliders();
            newItem.heading= Slider.heading;
            newItem.linke = Slider.linke;
            newItem.linkeName = Slider.linkeName;
            String timeStamp = GetTimestamp(DateTime.Now);
            var filePath = Path.Combine(_host.WebRootPath + "/images/sliders",timeStamp+img.FileName);
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await img.CopyToAsync(fileStream);
            }
            newItem.image = timeStamp + img.FileName;
            newItem.status = Slider.status;
            newItem.description = Slider.description;
            newItem.Create_AT = DateTime.Now.ToLocalTime();
            newItem.Update_AT = DateTime.Now.ToLocalTime();
            m_db.Slider.AddAsync(newItem);
            int c = await m_db.SaveChangesAsync();
            return newItem;
        }

        public ResponseDTO DeleteSliders(int id)
        {

            // Items itemToDelete = m_db.Items.Where(item =>item.id == id).FirstOrDefault();
            Sliders itemToDelete = GetSlider(id);
            if (itemToDelete == null)//בדיקה אם הישות קיים במאגר נתוניים
            {
                return new ResponseDTO()
                {
                    Status = StatusCode.Error,
                    StatusText = $"Entity With id:{id} not in DB"
                };
            }
            var a = File.Exists(Path.Combine(_host.WebRootPath + "/images/sliders", itemToDelete.image));
            if (a)
            {
                File.Delete(Path.Combine(_host.WebRootPath + "/images/sliders", itemToDelete.image));
            }

            m_db.Slider.Remove(itemToDelete);
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


        public async Task<ResponseDTO> UpdateSlidersAsync(int id, SlidersDTO Slider, IFormFile img)
        {

            // Items itemToDelete = m_db.Items.Where(item =>item.id == id).FirstOrDefault();
            Sliders itemToUpdate = GetSlider(id);
            if (itemToUpdate == null)//בדיקה אם הישות קיים במאגר נתוניים
            {
                return new ResponseDTO()
                {
                    Status = StatusCode.Error,
                    StatusText = $"Entity With id:{id} not in DB"
                };
            }
            itemToUpdate.heading = Slider.heading;
            itemToUpdate.linke = Slider.linke;
            itemToUpdate.linkeName = Slider.linkeName;
                if (img != null)
            {
                String timeStamp = GetTimestamp(DateTime.Now);
                var filePath = Path.Combine(_host.WebRootPath + "/images/sliders", timeStamp+img.FileName);
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                     var a = File.Exists(Path.Combine(_host.WebRootPath + "/images/sliders", itemToUpdate.image));
                    if (a)
                    {
                        File.Delete(Path.Combine(_host.WebRootPath + "/images/sliders", itemToUpdate.image));
                    }
                    await img.CopyToAsync(fileStream);
                }
                itemToUpdate.image = timeStamp+img.FileName;
            }
            else {
                itemToUpdate.image = itemToUpdate.image;
            }
            itemToUpdate.status = Slider.status;
            itemToUpdate.description = Slider.description;

            itemToUpdate.Create_AT = itemToUpdate.Create_AT;
            itemToUpdate.Update_AT = DateTime.Now.ToLocalTime();
            m_db.Slider.Update(itemToUpdate);
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
