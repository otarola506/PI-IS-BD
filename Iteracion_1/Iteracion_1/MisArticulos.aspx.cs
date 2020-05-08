﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

namespace Iteracion_1
{
    public partial class MisArticulos : System.Web.UI.Page
    {
        private SqlConnection con;
        Encoding unicode = Encoding.Unicode;
        private void connection()
        {
            string conString = ConfigurationManager.ConnectionStrings["grupo2Conn"].ToString();
            con = new SqlConnection(conString);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                llenarTabla();
                

            }
        }
        void llenarTabla()
        {
            connection();
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter ad = new SqlDataAdapter("Recuperar_ID_Titulo_Resumen", con);
            ad.Fill(dt);
            tablaArticulos.DataSource = dt;
            tablaArticulos.DataBind();
            if (dt.Rows.Count > 0)
            {
                tablaArticulos.DataSource = dt;
                tablaArticulos.DataBind();

            }
            else
            {
                dt.Rows.Add(dt.NewRow());
                tablaArticulos.DataSource = dt;
                tablaArticulos.DataBind();
                tablaArticulos.Rows[0].Cells.Clear();
                tablaArticulos.Rows[0].Cells.Add(new TableCell());
                tablaArticulos.Rows[0].Cells[0].ColumnSpan = dt.Columns.Count;
                tablaArticulos.Rows[0].Cells[0].Text = "No se han encontrado datos.";
                tablaArticulos.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;

            }


        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = (DataRowView)e.Row.DataItem;
                string resumen = unicode.GetString((byte[])dr["resumen"]);
                (e.Row.FindControl("resumen") as Label).Text = resumen;
            }
        }

        public int retornarValorIdArticulo(object sender)
        {
            ImageButton btn = (ImageButton)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            string temp = ((Label)gvr.Cells[0].FindControl("artId")).Text;
            int id = Int32.Parse(temp);
            return id;


        }
        public void lnkEdicion(object sender, EventArgs e)
        {
            int artId = retornarValorIdArticulo(sender);
            connection();
            con.Open();
            SqlCommand cmd = new SqlCommand("RecuperarArticulo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = artId;
            SqlDataReader reader = cmd.ExecuteReader();
            Boolean tipoArticulo = false;
            if (reader.Read())
            {
                tipoArticulo = (Boolean)reader[5];

            }
            if (tipoArticulo == false)
            {
                Session["articuloID"] = artId;
                Response.Redirect("EditorArticulo.aspx");
            }
            else
            {
                Response.Write("<script>alert('No se pueden editar artículos subidos a la pagina')</script>");

            }

        }

        public void lnkBorrado(object sender, EventArgs e)
        {
            int artId = retornarValorIdArticulo(sender);
            connection();
            SqlCommand cmd = new SqlCommand("Borrar_Articulo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = artId;
            con.Open();
            cmd.ExecuteNonQuery();
            llenarTabla();
            Response.Write("<script>alert('Articulo Borrado con éxito')</script>");

        }
       

        public void lnkVerMasArt(object sender, EventArgs e)
        {
            connection();
            con.Open();
            int artId = retornarValorIdArticulo(sender);
            SqlCommand cmd = new SqlCommand("RecuperarArticulo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = artId;
            SqlDataReader reader = cmd.ExecuteReader();

            Boolean tipoArticulo = false;
            reader.Read();
            tipoArticulo = (Boolean)reader[5];
            reader.Close();

            if (tipoArticulo == false)
            {
                Session["articuloID"] = artId;
                Response.Redirect("MostrarContenido.aspx");
            }
            
            

        }

        protected void btnAgregarArticulo_Click(object sender, EventArgs e)
        {
            Response.Redirect("Editor.aspx");
        }

        protected void tablaArticulos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}