﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;


namespace Iteracion_2.Models
{
    public class EmailModel
    {
        private SqlConnection Con;
        private ConexionModel ConnectionString { get; set; }
        public void Connection()
        {
            ConnectionString = new ConexionModel();
            Con = ConnectionString.Connection();
        }
        public async Task enviarCorreo(string destinatario, string asunto, string contenido,IFormFile archivo) {
            
            MailMessage mm = new MailMessage();
            mm.To.Add(destinatario);
            mm.Subject = asunto;
            AlternateView imgview = AlternateView.CreateAlternateViewFromString(contenido + "<br/><img src=cid:imgpath height=200 width=400>", null, "text/html");
            LinkedResource lr = new LinkedResource(@"Images/shieldship.jpg", MediaTypeNames.Image.Jpeg);
            lr.ContentId = "imgpath";
            imgview.LinkedResources.Add(lr);
            mm.AlternateViews.Add(imgview);
            mm.Body = lr.ContentId;
            if (archivo != null)
            {
                var ms = new MemoryStream();
                archivo.CopyTo(ms);
                var fileBytes = ms.ToArray();
                mm.Attachments.Add(new Attachment(new MemoryStream(fileBytes), archivo.FileName));
            }
            mm.IsBodyHtml = true;
            mm.From = new MailAddress("comunidadshieldship@gmail.com");
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.Port = 587;
            smtp.UseDefaultCredentials = true;
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential("comunidadshieldship@gmail.com", "BASESdatos176");
            await smtp.SendMailAsync(mm);

        }

        public async Task enviarSolicitud(string contenido,string Usuario)
        {
            await CorreoDefault("comunidadshieldship@gmail.com", "Solicitud de promocion de rango de " + Usuario, contenido);

           
        }

        public List<List<String>> RecuperarCorreosNucleo() {
            Connection();
            List<List<String>> Results = new List<List<String>>();
            string query = "SELECT nombreUsuarioPk,correo FROM Miembro WHERE pesoMiembro = 5";
            SqlCommand command = new SqlCommand(query, Con)
            {
                CommandType = CommandType.Text
            };
            DataTable dTable = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dTable);
            for (int index = 0; index < dTable.Rows.Count; index++)
            {
                Results.Add(new List<string> {
                                    dTable.Rows[index][0].ToString(), // nombreUsuario
                                        dTable.Rows[index][1].ToString(), // correo
                            });

            }

            Con.Close();
            return Results;
        }

        public async Task EnviarSolicitudNucleo(string titulo) {
            List<List<String>> correosNucleo = RecuperarCorreosNucleo();
            for (int index = 0; index < correosNucleo.Count; index++)
            {
                await CorreoDefault(correosNucleo[index][1],
                 "Colaboración en el artículo " + titulo,
                 "Estimado "+correosNucleo[index][0]+" se le solicita la colaboración en el proceso de revisión del artículo "+titulo);
            }

        }

        public async Task CorreoDefault(string correo, string asunto, string mensaje)
        {
            MailMessage mm = new MailMessage();
            mm.To.Add(correo);
            mm.Subject = asunto;
            AlternateView imgview = AlternateView.CreateAlternateViewFromString(mensaje + "<br/><img src=cid:imgpath height=200 width=400>", null, "text/html");
            LinkedResource lr = new LinkedResource(@"Images/shieldship.jpg", MediaTypeNames.Image.Jpeg);
            lr.ContentId = "imgpath";
            imgview.LinkedResources.Add(lr);
            mm.AlternateViews.Add(imgview);
            mm.Body = lr.ContentId;
            mm.IsBodyHtml = false;
            mm.From = new MailAddress("comunidadshieldship@gmail.com");
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.Port = 587;
            smtp.UseDefaultCredentials = true;
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential("comunidadshieldship@gmail.com", "BASESdatos176");
            await smtp.SendMailAsync(mm);
        }

    }
}