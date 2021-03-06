﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iteracion_2.Models;

namespace Iteracion_2.Controllers
{
    public class FormularioRevisionController
    {

        FormularioRevisionModel FormularioMod { get; set; }
        
        public bool ValidarEntradas(string opinion, string contribucion, string forma)
        {
            FormularioMod = new FormularioRevisionModel();
            return FormularioMod.ValidarEntradas(opinion, contribucion, forma);
        }

        public bool ProcesarFormulario(int opinion, int contribucion, int forma, string observaciones, string miembroID, string artID)
        {
            FormularioMod = new FormularioRevisionModel();
            return FormularioMod.ProcesarFormulario(opinion, contribucion, forma, observaciones, miembroID, artID);

        }


    }
}
