using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity.Validation;
using System.Net.Mail;
using System.Text;
using DPS_Seletiene.data;
using DPS_Seletiene.Models;

using System.Security.Cryptography;


namespace DPS_Seletiene.Controllers.api
{
    public class userController : ApiController
    {
        private seletieneEntities db = new seletieneEntities();

        // GET: api/userappApi
     
        
        [System.Web.Http.HttpGet]         
        [ResponseType(typeof(userapp))]
        public userapp Login(string email, string pass)
        {
            db.Configuration.LazyLoadingEnabled = false;

            pass=MD5Manager.Encrypt(pass, true);
            List<userapp> user = db.userapp.Where(r => r.email.Equals(email)).Where(r => r.passw.Equals(pass)).ToList();
               if(user.Count > 0)
               {
                   userapp userapp=user.ElementAt(0);
                     return userapp;
            
               }

              return null;
        }
       
        [System.Web.Http.HttpGet]
        [ResponseType(typeof(userapp))]
        public userapp Actualizar(int  id, string telephone,string cellphone,string email)
        {
            userapp user = db.userapp.Find(id);
            if (user == null)
            {
                user.telephone = telephone;
                user.cellphone = cellphone;
                user.email = email;
                 db.Entry(user).State = EntityState.Modified;

            
            
                 db.SaveChanges();
            
                return user;

            }

            return null;
        }

        [System.Web.Http.HttpGet]
        public Task<HttpResponseMessage> Resetpass(string email)
        {
            List<userapp> user = db.userapp.Where(r => r.email.Equals(email)).ToList();
            if (user.Count > 0)
            {
                sendemail(user.ElementAt(0),email);
                HttpResponseMessage response = new HttpResponseMessage()
                {
                    Content = new StringContent("Correo enviado")
                };
                return Task.FromResult(response);

            }
            else
            {
                HttpResponseMessage response = new HttpResponseMessage()
                {
                    Content = new StringContent("Email invalido")
                };
            return Task.FromResult(response);
              
            }

        }



        public string CreatePassword(int length)

        {
        const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        StringBuilder res = new StringBuilder();
        Random rnd = new Random();
        while (0 < length--)
        {
            res.Append(valid[rnd.Next(valid.Length)]);
        }
        return res.ToString();

        }


        public void sendemail(userapp user, string email)
        {
            user.passw = CreatePassword(6);
            user.passw = MD5Manager.Encrypt(user.passw, true);

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
                
            var fromAddress = new MailAddress("info@salud.ucaldas.edu.co", "Se le tiene");
            var toAddress = new MailAddress(user.email, "To Name");
            const string fromPassword = "descargar";
            const string subject = "Cambio de contraseña";
            string body = "<h3>Cordial saludo</h3><h3 style=\"text-align: justify;\">Tu nueva contraseña es " + user.passw + "</p>";

            try
            {

                var smtp = new SmtpClient
                {
                    Host = "72.29.75.91",
                    Port = 25,
                    EnableSsl = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Timeout = 10000,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                var message = new MailMessage(fromAddress, toAddress);

                message.IsBodyHtml = true;
                message.Subject = subject;
                message.Body = body;



                smtp.EnableSsl = false;
                smtp.Send(message);


            }


            catch (Exception e)
            {

                Console.WriteLine("Ouch!" + e.ToString());

            }
        }
        // GET: api/userappApi/5
        [ResponseType(typeof(userapp))]
        public async Task<IHttpActionResult> Getuserapp(int id)
        {
            userapp userapp = await db.userapp.FindAsync(id);
            if (userapp == null)
            {
                return NotFound();
            }

            return Ok(userapp);
        }

        // PUT: api/userappApi/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putuserapp(int id, userapp userapp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userapp.id)
            {
                return BadRequest();
            }
        
            db.Entry(userapp).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!userappExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/userappApi
        [ResponseType(typeof(userapp))]
        public async Task<IHttpActionResult> Postuserapp(userapp userapp)
        {
           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                List<userapp> user = db.userapp.Where(r => r.email.Equals(userapp.email)).ToList();
                if (user.Count > 0)
                {
                    return Content(HttpStatusCode.BadRequest, "Usuario existente");
 

                }
                userapp.passw = MD5Manager.Encrypt(userapp.passw, true);
                userapp.status = 1;
                db.userapp.Add(userapp);
            await db.SaveChangesAsync();
      
            }
               catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            return Ok(userapp);
        }

        // DELETE: api/userappApi/5
        [ResponseType(typeof(userapp))]
        public async Task<IHttpActionResult> Deleteuserapp(int id)
        {
            userapp userapp = await db.userapp.FindAsync(id);
            if (userapp == null)
            {
                return NotFound();
            }

            db.userapp.Remove(userapp);
            await db.SaveChangesAsync();

            return Ok(userapp);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool userappExists(int id)
        {
            return db.userapp.Count(e => e.id == id) > 0;
        }
    }
}