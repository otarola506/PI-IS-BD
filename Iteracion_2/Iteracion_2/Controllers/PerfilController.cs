﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iteracion_2.Models;

namespace Iteracion_2.Controllers
{
    public class PerfilController
    {
        private PerfilModel perfilModel { set; get; }
        public List<String> RetornarDatosPerfil(string nombreUsuario) {
            perfilModel = new PerfilModel();

            return perfilModel.RetornarDatosPerfil(nombreUsuario);
        }

        public List<List<string>> RetornarArticulosMiembro(string nombreUsuario)
        {
            perfilModel = new PerfilModel();

            return perfilModel.RetornarArticulosMiembro(nombreUsuario);
        }

        public void GuardarDatosPerfil(string nombreUsuario, string[] informacionActualizada)
        {
            perfilModel = new PerfilModel();

            perfilModel.GuardarDatosPerfil(nombreUsuario, informacionActualizada);

        }
    }
}
