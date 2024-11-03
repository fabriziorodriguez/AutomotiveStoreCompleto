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
    public class ProductoController : ControllerBase
    {
        private readonly string cadenaSQL;

        public ProductoController(IConfiguration config)
        {

            cadenaSQL = config.GetConnectionString("CadenaSQL");

        }

        [HttpGet]
        [Route("Listar")]
        public IActionResult Listar()
        {
            List<Producto> lista = new List<Producto>();


            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_listar_producto", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Producto()
                            {
                                Id = Convert.ToInt32(reader["ID"]),
                                Name = reader["NAME"].ToString(),
                                Price = Convert.ToDecimal(reader["PRICE"]),
                                Stock = Convert.ToInt32(reader["STOCK"]),
                                IsActive = Convert.ToBoolean(reader["IS_ACTIVE"]),
                                IdArticulesTypes = Convert.ToInt32(reader["ID_ARTICULES_TYPES"])


                            });


                        }
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = lista });

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = error.Message, Response = lista });
            }
        }


        [HttpGet]
        [Route("Obtener/{idProducto:int}")]
        public IActionResult Obtener(int idProducto)
        {
            List<Producto> lista = new List<Producto>();
            Producto producto = new Producto();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_listar_producto", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Producto()
                            {
                                Id = Convert.ToInt32(reader["ID"]),
                                Name = reader["NAME"].ToString(),
                                Price = Convert.ToDecimal(reader["PRICE"]),
                                Stock = Convert.ToInt32(reader["STOCK"]),
                                IsActive = Convert.ToBoolean(reader["IS_ACTIVE"]),
                                IdArticulesTypes = Convert.ToInt32(reader["ID_ARTICULES_TYPES"])


                            });


                        }
                    }
                }

                producto = lista.Where(item => item.Id == idProducto).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = producto });

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = error.Message, Response = producto });
            }
        }




        [HttpGet]
        [Route("Obtener/{name}")]
        public IActionResult ObtenerNombre(string name)
        {
            List<Producto> lista = new List<Producto>();
            Producto producto = new Producto();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_listar_producto", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Producto()
                            {
                                Id = Convert.ToInt32(reader["ID"]),
                                Name = reader["NAME"].ToString(),
                                Price = Convert.ToDecimal(reader["PRICE"]),
                                Stock = Convert.ToInt32(reader["STOCK"]),
                                IsActive = Convert.ToBoolean(reader["IS_ACTIVE"]),
                                IdArticulesTypes = Convert.ToInt32(reader["ID_ARTICULES_TYPES"])


                            });


                        }
                    }
                }

                producto = lista.Where(item => item.Name == name).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = producto });

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = error.Message, Response = producto });
            }
        }


        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Producto objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_guardar_producto", conexion);
                    cmd.Parameters.AddWithValue("@name", objeto.Name);
                    cmd.Parameters.AddWithValue("@price", objeto.Price);
                    cmd.Parameters.AddWithValue("@stock", objeto.Stock);
                    cmd.Parameters.AddWithValue("@is_active", objeto.IsActive);
                    cmd.Parameters.AddWithValue("id_articules_types", objeto.IdArticulesTypes);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }



        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Producto objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_editar_producto", conexion);
                    cmd.Parameters.AddWithValue("@id", objeto.Id == 0 ? DBNull.Value : objeto.Id);
                    cmd.Parameters.AddWithValue("@name", string.IsNullOrEmpty(objeto.Name) ? DBNull.Value : objeto.Name);
                    cmd.Parameters.AddWithValue("@price", objeto.Price <= 0 ? DBNull.Value : objeto.Price);
                    cmd.Parameters.AddWithValue("@stock", objeto.Stock < 0 ? DBNull.Value : objeto.Stock);
                    cmd.Parameters.AddWithValue("@is_active", objeto.IsActive == null ? DBNull.Value : objeto.IsActive);
                    cmd.Parameters.AddWithValue("@id_articules_types",objeto.IdArticulesTypes != 1 && objeto.IdArticulesTypes != 2 && objeto.IdArticulesTypes != 3
    ? DBNull.Value
    : (object)objeto.IdArticulesTypes);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "editado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }




        [HttpDelete]
        [Route("Eliminar/{idProducto}")]
        public IActionResult Eliminar(int idProducto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_eliminar_producto", conexion);
                    cmd.Parameters.AddWithValue("@id",idProducto);
                   
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "eliminado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }




    }
}
