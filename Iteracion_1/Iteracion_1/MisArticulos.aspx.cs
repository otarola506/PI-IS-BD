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
            var UsuarioActual = Request["value1"];
            if (UsuarioActual == null || UsuarioActual == "")
            {
                Response.Redirect("https://localhost:44338/Cuenta/Ingresar");
            }
            connection(); 
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter ad = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("Recuperar_Articulos_Autor",con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@userName", SqlDbType.VarChar).Value = UsuarioActual; // En este caso está quemado el nombre de usuario
            SqlDataReader reader = cmd.ExecuteReader();
            

            if ( reader.Read())
            {
                string nombre = reader[4].ToString();
                lblArticulo.Text = "Bienvenido a sus artículos, " + nombre;
                reader.Close();
                ad.SelectCommand = cmd;
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    tablaArticulos.DataSource = dt;
                    tablaArticulos.DataBind();
                    tablaArticulos.Columns[0].Visible = false;

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
            string tipoArticulo = "";
            string estado = "";
            if (reader.Read())
            {
                tipoArticulo = reader[7].ToString();
                estado = reader[6].ToString();

            }
            if (string.Compare(estado,"pendiente") == 0)
            {
                Response.Write("<script>alert('Este artículo se encuentra en revisión por lo tanto no puede ser editado.')</script>");
            }
            else
            {
                if (string.Compare(tipoArticulo, "corto") == 0)
                {
                    Session["articuloID"] = artId;
                    Session["estadoArt"] = estado;
                    //Response.Redirect("EditorArticuloModificado.aspx");
                    string Location = "http://localhost:51359/EditorArticuloModificado?value1=";
                    const string SessionKeyUsuario = "UsuarioActual";
                    var UsuarioActual = Request["value1"];
                    //string UsuarioActual = HttpContext.Session.GetString(SessionKeyUsuario);
                    if (UsuarioActual != null)
                    {
                        Response.Redirect(Location + UsuarioActual);
                    }

                }
                else
                {
                    Session["articuloID"] = artId;
                    //Response.Redirect("EditorResumen.aspx");
                    string Location = "http://localhost:51359/EditorResumen?value1=";
                    const string SessionKeyUsuario = "UsuarioActual";
                    var UsuarioActual = Request["value1"];
                    //string UsuarioActual = HttpContext.Session.GetString(SessionKeyUsuario);
                    if (UsuarioActual != null)
                    {
                        Response.Redirect(Location + UsuarioActual);
                    }
                }
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

            string tipoArticulo = "";
            reader.Read();
            tipoArticulo = reader[7].ToString();
            reader.Close();
            if (string.Compare(tipoArticulo,"corto") == 0)
            {
                Session["articuloID"] = artId;
                Response.Redirect("MostrarContenido.aspx");
            }
            else {
                //Descargar archivo desde base de datos
                reader = cmd.ExecuteReader();
                reader.Read();
                string fileName = reader["nombreArchivo"].ToString();
                byte[] contenidoArt = (byte[])reader["contenido"];
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                Response.BinaryWrite(contenidoArt);
                Response.Flush();
                Response.End();



            }



        }

        protected void btnAgregarArticulo_Click(object sender, EventArgs e)
        {
            string Location = "http://localhost:51359/Editor?value1=";
            const string SessionKeyUsuario = "UsuarioActual";
            var UsuarioActual = Request["value1"];
            //string UsuarioActual = HttpContext.Session.GetString(SessionKeyUsuario);
            if (UsuarioActual != null)
            {
                Response.Redirect(Location + UsuarioActual);
            }
            //Response.Redirect("Editor.aspx");
        }

        
    }
}