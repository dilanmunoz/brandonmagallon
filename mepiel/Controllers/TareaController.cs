using Antlr.Runtime.Misc;
using mepiel.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace mepiel.Controllers
{
    public class TareaController : Controller
    {
        public SqlConnection con;
        public TareaController()
        {
            this.con = new SqlConnection(
                "data source= DESKTOP-2L76QTK; database=prueba; integrated security=SSPI");
        }
        public ActionResult Index(string mensaje = "")
        {
            ViewBag.status = mensaje;
            using (SqlCommand cmd = new SqlCommand(" SELECT * FROM Tarea"))
            {
                this.con.Open();
                
                List<Tarea> tareas = new List<Tarea>();
                cmd.Connection = con;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader != null && reader.Read())
                {
                    var tarea = new Tarea
                    {
                        id = Convert.ToInt16(reader["id"]),
                        titulo = reader["titulo"].ToString(),
                        descripcion = reader["descripcion"].ToString(),
                        prioridad = Convert.ToByte(reader["prioridad"]),
                        estado = Convert.ToByte(reader["estado"]),
                        registro = Convert.ToDateTime(reader["registro"]),
                        vencimiento = Convert.ToDateTime(reader["vencimiento"])
                    };
                    tareas.Add(tarea);
                }
                return View(tareas);
            }
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(Tarea tarea)
        {
            string status = "";
            var query = "INSERT INTO Tarea (titulo, descripcion, prioridad, estado, registro, vencimiento)" +
                "VALUES(@titulo, @descripcion, @prioridad, @estado, GETDATE(), @vencimiento)";
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Connection = con;
                this.con.Open();
                cmd.Parameters.AddWithValue("@titulo", tarea.titulo);
                cmd.Parameters.AddWithValue("@descripcion", tarea.descripcion);
                cmd.Parameters.AddWithValue("@prioridad", tarea.prioridad);
                cmd.Parameters.AddWithValue("@estado", tarea.estado);
                cmd.Parameters.AddWithValue("@vencimiento", tarea.vencimiento);
                status = (cmd.ExecuteNonQuery() >= 1) ? "Se guardo Correctamente" : "No se Guardo";
                ViewBag.status = status;
                return View();
            }           
        }                
        public ActionResult Estado(estado estado) 
        {           
                string status = "";
                var query = "UPDATE Tarea SET estado = @estado WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    this.con.Open();
                    cmd.Parameters.AddWithValue("@estado", estado.valor == 1 ? 0 : 1);
                    cmd.Parameters.AddWithValue("@id", estado.id);
                    status = (cmd.ExecuteNonQuery() >= 1) ? "Estado Cambiado con exito" : "No se aplico el cambio";
                    ViewBag.status = status;                                  
                    return RedirectToAction("Index", "Tarea", new { mensaje = status });
            }                                          
        }
        public ActionResult Delete(estado estado)
        {
            string status = "";
            var query = "DELETE Tarea WHERE id = @id";
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Connection = con;
                this.con.Open();                
                cmd.Parameters.AddWithValue("@id", estado.id);
                status = (cmd.ExecuteNonQuery() >= 1) ? "Tarea Eliminada" : "No pudo Eliminar";
                ViewBag.status = status;
                return RedirectToAction("Index", "Tarea", new { mensaje = status });
            }                              
        }
        public ActionResult Editar(int id)
        {
            using (SqlCommand cmd = new SqlCommand(" SELECT * FROM Tarea WHERE id = @id"))
            {
                this.con.Open();
                cmd.Parameters.AddWithValue("@id", id);
                var tarea = new Tarea(); 
                cmd.Connection = con;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader != null && reader.Read())
                {
                    tarea.id = Convert.ToInt16(reader["id"]);
                    tarea.titulo = reader["titulo"].ToString();
                    tarea.descripcion = reader["descripcion"].ToString();
                    tarea.prioridad = Convert.ToByte(reader["prioridad"]);
                    tarea.estado = Convert.ToByte(reader["estado"]);
                    tarea.registro = Convert.ToDateTime(reader["registro"]);
                    tarea.vencimiento = Convert.ToDateTime(reader["vencimiento"]);
                }
                return View(tarea);
            }            
        }
        [HttpPost]
        public ActionResult Editar(Tarea tarea)
        {
            string status = "";
            var query = "UPDATE Tarea SET titulo = @titulo, descripcion = @descripcion," +
                "prioridad = @prioridad, vencimiento = @vencimiento " +
                "WHERE id = @id";
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Connection = con;
                this.con.Open();
                cmd.Parameters.AddWithValue("@id", tarea.id);
                cmd.Parameters.AddWithValue("@titulo", tarea.titulo);
                cmd.Parameters.AddWithValue("@descripcion", tarea.descripcion);
                cmd.Parameters.AddWithValue("@prioridad", tarea.prioridad);
                cmd.Parameters.AddWithValue("@vencimiento", tarea.vencimiento);
                status = (cmd.ExecuteNonQuery() >= 1) ? "exito" : "error";
                ViewBag.status = status;
                return View();
            }

        }
    }
}