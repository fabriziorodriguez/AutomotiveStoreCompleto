using AutomotiveStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace AutomotiveStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly string cadenaSQL;

        public VentasController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Route("ResumenVentasVendedores")]
        public IActionResult ObtenerResumenVentasVendedores(DateTime fechaInicio, DateTime fechaFin, string nacionalidadCliente, decimal precioMinimo, string metodosPago)
        {
            var resultado = new List<VendedorVentasResumen>();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_total_articulos_vendidos_por_vendedor", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros
                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin);
                    cmd.Parameters.AddWithValue("@NacionalidadCliente", nacionalidadCliente);
                    cmd.Parameters.AddWithValue("@PrecioMinimo", precioMinimo);
                    cmd.Parameters.AddWithValue("@MetodosPago", metodosPago);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resultado.Add(new VendedorVentasResumen
                            {
                                SellerName = reader["SellerName"].ToString(),
                                TotalItemsSold = reader.GetInt32(reader.GetOrdinal("TotalItemsSold")),
                                TotalSalesAmount = reader.GetDecimal(reader.GetOrdinal("TotalSalesAmount"))
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
