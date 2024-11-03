using AutomotiveStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace AutomotiveStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticuloResumenController : ControllerBase
    {
        private readonly string cadenaSQL;
        public ArticuloResumenController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Route("ResumenArticulos")]
        public IActionResult ObtenerResumenArticulos(int cantidadMinima, char letraInicio, char letraFin)
        {
            var resultado = new List<ArticuloResumen>(); 

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_resumen_articulos", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros
                    cmd.Parameters.AddWithValue("@CantidadMinima", cantidadMinima);
                    cmd.Parameters.AddWithValue("@LetraInicio", letraInicio);
                    cmd.Parameters.AddWithValue("@LetraFin", letraFin);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resultado.Add(new ArticuloResumen
                            {
                                Articulo = reader["ARTICULO"].ToString(),
                                TipoArticulo = reader["TIPO DE ARTICULO"].ToString(),
                                CostoTotal = reader.GetDecimal(reader.GetOrdinal("COSTO TOTAL")),
                                PromedioCantidadVendida = Convert.ToInt32(reader["PROMEDIO CANTIDAD VENDIDA"]) // Cambiado a int
                            });
                        }
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = resultado });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, Response = resultado });
            }
        }

    }
}
