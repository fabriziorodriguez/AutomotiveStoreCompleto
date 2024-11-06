using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using AutomotiveStore.Models;
using Microsoft.AspNetCore.Cors;

namespace AutomotiveStore.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioAdicionalController : ControllerBase
    {
        private readonly string cadenaSQL;

        public ServicioAdicionalController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }


        [HttpGet]
        [Route("TopServicioAdicional")]
        public IActionResult ObtenerTopServicioAdicional(int startMonth, int endMonth)
        {
            var resultado = new List<ServicioAdicional>();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_listar_servicio_mas_vendido", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StartMonth", startMonth);
                    cmd.Parameters.AddWithValue("@EndMonth", endMonth);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resultado.Add(new ServicioAdicional()
                            {
                                Id = Convert.ToInt32(reader["ID"]),
                                DescripcionServicio = reader["SERVICIO"].ToString(),
                                Cantidad = Convert.ToInt32(reader["CANTIDAD"])
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
