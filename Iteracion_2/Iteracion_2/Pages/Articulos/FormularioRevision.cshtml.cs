﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Iteracion_2.Controllers;
using Microsoft.AspNetCore.Http;
using System.Windows;


namespace Iteracion_2.Pages.Articulos
{
    public class FormularioRevisionModel : PageModel
    {
        FormularioRevisionController FormularioContro { get; set; }

        ArticuloController ArticuloContro { get; set; }

        public string[] InformacionArticulo { get; private set; }

        public string Autores { get; set; }
        [TempData]
        public string Message { get; set; }

        public List<string> Autor { get; private set; }

        const string SessionKeyUsuario = "UsuarioActual";
        const string SessionKeyPesoUsuario = "PesoActual";

        public string ArticuloID { get; private set; }

        public IActionResult OnGet(string artId)
        {
            string UsuarioActual = HttpContext.Session.GetString(SessionKeyUsuario);
            string PesoActual = HttpContext.Session.GetString(SessionKeyPesoUsuario);

            if(PesoActual == "5" && artId != null)
            {
                ArticuloContro = new ArticuloController();
                Autores = "";
                InformacionArticulo = ArticuloContro.RetornarDatos(artId);
                Autor = ArticuloContro.RetornarAutor(artId);
                for (int index = 0; index < Autor.Count; index++)
                {
                    if (index != 0)
                    {
                        Autores += " , ";
                    }
                    Autores += Autor[index] + " ";
                }


                ArticuloID = artId;


                return Page();
            }
            return RedirectToPage("/Cuenta/Ingresar", new { Mensaje = "Permisos insuficientes" });

        }

        
        public IActionResult OnPost(string artId)
        {
            FormularioContro = new FormularioRevisionController();
            string UsuarioActual = HttpContext.Session.GetString(SessionKeyUsuario);
            string opinion = Request.Form["Opinion"].ToString();
            string contribucion = Request.Form["Contribucion"].ToString();
            string forma = Request.Form["Forma"].ToString();
            string observaciones = "" + Request.Form["comentarios"].ToString();

            if (!FormularioContro.ValidarEntradas(opinion, contribucion, forma))
            {
                Message = "No ha seleccionado todas las calificaciones";
                return RedirectToPage("FormularioRevision", new { artId });
            }


            int opinionInt = Int16.Parse(opinion);
            int contribucionInt = Int16.Parse(contribucion);
            int formaInt = Int16.Parse(forma);



            ArticuloID = artId;



           
            bool validado = FormularioContro.ProcesarFormulario(opinionInt, contribucionInt, formaInt, observaciones, UsuarioActual, ArticuloID);
            if (validado)
            {
                return RedirectToPage("/Articulos/Revision");
            }
            else {
                Message = "Intentar de nuevo.";
                return RedirectToPage("FormularioRevision", new { artId });

            }

            

        }
    }
}