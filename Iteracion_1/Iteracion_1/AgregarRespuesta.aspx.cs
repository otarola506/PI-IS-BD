﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Iteracion_1
{
    public partial class AgregarRespuesta : System.Web.UI.Page
    {
        private SqlConnection con;

        private void connection()
        {
            string conStr = ConfigurationManager.ConnectionStrings["grupo2Conn"].ToString();
            con = new SqlConnection(conStr);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void GRButton_OnClick(object sender, EventArgs e)
        {
            int pregID = Convert.ToInt32(Session["PregID"]);
            connection();
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand sqlCmd = new SqlCommand("AddRespuesta", con);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@Respuesta", TxtBoxRN.Text.Trim());
            sqlCmd.Parameters.AddWithValue("@PregID", pregID);
            sqlCmd.Parameters.AddWithValue("@MiembroID", 0);
            sqlCmd.ExecuteNonQuery();
            con.Close();
            Response.Redirect("RespuestasPreg.aspx");
        }

        protected void VolverButton_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("RespuestasPreg.aspx");
        }
    }
}