using AutomotiveStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace AutomotiveStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumenFacturasController : ControllerBase
    {
        private readonly string cadenaSQL;

        public ResumenFacturasController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Route("ResumenFacturas")]
        public IActionResult ObtenerResumenFacturas(int mes, int? anio = null)
        {
            var resultado = new ResumenFacturas();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_resumen_facturas", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mes", mes);
                    cmd.Parameters.AddWithValue("@Anio", anio ?? DateTime.Now.Year);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            resultado = new ResumenFacturas
                            {
                                TotalFacturas = reader["TOTAL DE FACTURAS"] != DBNull.Value ? Convert.ToInt32(reader["TOTAL DE FACTURAS"]) : 0,
                                CantidadArticulosVendidos = reader["CANTIDAD DE ARTICULOS VENDIDOS"] != DBNull.Value ? Convert.ToInt32(reader["CANTIDAD DE ARTICULOS VENDIDOS"]) : 0,
                                CostoTotal = reader["COSTO TOTAL"] != DBNull.Value ? Convert.ToDecimal(reader["COSTO TOTAL"]) : 0m
                            };
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
