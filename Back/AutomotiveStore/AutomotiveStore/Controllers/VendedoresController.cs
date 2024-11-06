using AutomotiveStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace AutomotiveStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendedoresController : ControllerBase
    {
        private readonly string cadenaSQL;

        public VendedoresController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Route("ResumenVendedores")]
        public IActionResult ObtenerResumenVendedores(DateTime fechaInicio, DateTime fechaFin)
        {
            var resultado = new List<VendedorResumen>();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_Vendedores_Mejor_Rendimiento", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros
                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resultado.Add(new VendedorResumen
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ID")),
                                Nombre = reader["NAME"].ToString(),
                                Apellido = reader["LAST_NAME"].ToString(),
                                CantidadArticulosVendidos = reader.GetInt32(reader.GetOrdinal("CantidadArticulosVendidos")),
                                TotalVentas = reader.GetDecimal(reader.GetOrdinal("TotalVentas"))
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
