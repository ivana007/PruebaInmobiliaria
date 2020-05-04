using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
	public class RepositorioContrato
	{
		private readonly string connectionString;
		private readonly IConfiguration configuration;

		public RepositorioContrato(IConfiguration configuration)
		{
			this.configuration = configuration;
			connectionString = configuration["ConnectionStrings:DefaultConnection"];
		}
		public int Alta(Contrato entidad)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Contratos (FechaAlta, fechaBaja, IdGarante, IdInquilino, IdInmueble ) " +
					"VALUES (@fechaAlta, @fechaBaja, @idGarante,  @idInquilino, @idInmueble );" +
					"SELECT SCOPE_IDENTITY();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (var command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@fechaAlta", entidad.FechaAlta);
					command.Parameters.AddWithValue("@fechaBaja", entidad.FechaBaja);
					command.Parameters.AddWithValue("@idGarante", entidad.IdGarante);

					command.Parameters.AddWithValue("@idInquilino", entidad.IdInquilino);
					command.Parameters.AddWithValue("@idInmueble", entidad.IdInmueble);
					//command.Parameters.AddWithValue("@idPropietario", entidad.IdPropietario);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					entidad.IdContrato = res;
					connection.Close();
				}
			}
			return res;
		}
		public int Baja(int id)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"DELETE FROM Contratos WHERE Id = {id}";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		public int BajaLogica(Contrato entidad)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "UPDATE Contratos SET " +
					"FechaAlta=@fechaAlta, FechaBaja=@fechaBaja, IdGarante=@idGarante, IdInquilino=@idInquilino, IdInmueble=@idInmueble " +
					"WHERE IdContrato = @idContrato";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@fechaAlta", entidad.FechaAlta);
					command.Parameters.AddWithValue("@fechaBaja", entidad.FechaBaja);
					command.Parameters.AddWithValue("@idGarante", entidad.IdGarante);

					command.Parameters.AddWithValue("@idInquilino", entidad.IdInquilino);
					command.Parameters.AddWithValue("@idInmueble", entidad.IdInmueble);
					//command.Parameters.AddWithValue("@IdPropietario", entidad.IdPropietario);
					command.Parameters.AddWithValue("@idContrato", entidad.IdContrato);
					command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public int Modificacion(Contrato entidad)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "UPDATE Contratos SET " +
					"FechaAlta=@fechaAlta, FechaBaja=@fechaBaja, IdGarante=@idGarante, IdInquilino=@idInquilino, IdInmueble=@idInmueble " +
					"WHERE IdContrato = @idContrato";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@fechaAlta", entidad.FechaAlta);
					command.Parameters.AddWithValue("@fechaBaja", entidad.FechaBaja);
					command.Parameters.AddWithValue("@idGarante", entidad.IdGarante);

					command.Parameters.AddWithValue("@idInquilino", entidad.IdInquilino);
					command.Parameters.AddWithValue("@idInmueble", entidad.IdInmueble);
					//command.Parameters.AddWithValue("@IdPropietario", entidad.IdPropietario);
					command.Parameters.AddWithValue("@idContrato", entidad.IdContrato);
					command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Contrato> ObtenerTodos()
		{
			IList<Contrato> res = new List<Contrato>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "SELECT  IdContrato, FechaAlta, FechaBaja, c.IdGarante, c.IdInquilino, c.IdInmueble ," +
					" g.Nombre , g.Apellido, i.Nombre, i.Apellido,ii.Precio" +
					" FROM Contratos c INNER JOIN Garantes g ON c.IdGarante = g.IdGarante" +
					" INNER JOIN Inquilinos i ON c.IdInquilino = i.IdInquilino" +
					" INNER JOIN Inmuebles ii ON c.IdInmueble = ii.IdInmueble ";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato entidad = new Contrato
						{
							IdContrato = reader.GetInt32(0),
							FechaAlta = reader.GetDateTime(1),
							FechaBaja = reader.GetDateTime(2),
							IdGarante = reader.GetInt32(3),
							garante = new Garante
							{
								Nombre = reader.GetString(6),
								Apellido = reader.GetString(7),
							},
							//IdPago = reader.GetInt32(4),
							IdInquilino = reader.GetInt32(4),
							inquilino = new Inquilino
							{
								Nombre = reader.GetString(8),
								Apellido = reader.GetString(9),
							},
							IdInmueble = reader.GetInt32(5),
							inmueble = new Inmueble
							{
								Precio = reader.GetDecimal(10)
							}

						};// termina contrato entidad
						res.Add(entidad);
					}//while
					connection.Close();
				}//using conexion
			}//using consulta
			return res;
		}//termina metodo


		public Contrato ObtenerPorId(int id)//contrato
		{
			Contrato entidad = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "SELECT  IdContrato, FechaAlta, FechaBaja, c.IdGarante, c.IdInquilino, c.IdInmueble ," +
					" g.Nombre, g.Apellido , i.Nombre,i.Apellido, ii.Precio " +
					" FROM Contratos c INNER JOIN Garantes g ON c.IdGarante = g.IdGarante " +

					"INNER JOIN Inquilinos i ON c.IdInquilino = i.IdInquilino " +
					"INNER JOIN Inmuebles ii ON c.IdInmueble = ii.IdInmueble " +
					$" WHERE c.IdContrato=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						entidad = new Contrato
						{
							IdContrato = reader.GetInt32(0),
							FechaAlta = reader.GetDateTime(1),
							FechaBaja = reader.GetDateTime(2),
							IdGarante = reader.GetInt32(3),
							garante = new Garante
							{
								//IdPropietario = reader.GetInt32(8),
								Nombre = reader.GetString(6),
								Apellido = reader.GetString(7),
							},
							IdInquilino = reader.GetInt32(4),
							inquilino = new Inquilino
							{
								//IdPropietario = reader.GetInt32(8),
								Nombre = reader.GetString(8),
								Apellido = reader.GetString(9),
							},

							IdInmueble = reader.GetInt32(5),
							inmueble = new Inmueble
							{
								//IdPropietario = reader.GetInt32(8),
								Precio = reader.GetDecimal(10)

							}

						};
					}
					connection.Close();
				}
			}
			return entidad;
		}
		public IList<Contrato> ObtenerPorDni(string dni)//busco el contrato de un inquilino pasando el id
		{
			IList<Contrato> res = new List<Contrato>();

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "SELECT  IdContrato, FechaAlta, FechaBaja, c.IdGarante, c.IdInquilino, c.IdInmueble ," +
					" g.Nombre, g.Apellido , i.Nombre,i.Apellido, ii.Precio " +
					" FROM Contratos c INNER JOIN Garantes g ON c.IdGarante = g.IdGarante " +

					"INNER JOIN Inquilinos i ON c.IdInquilino = i.IdInquilino " +
					"INNER JOIN Inmuebles ii ON c.IdInmueble = ii.IdInmueble " +
					$" WHERE i.Dni=@dni";// debo mostrar tambien los contratos vigentes
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@dni", dni);
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato entidad = new Contrato
						{
							IdContrato = reader.GetInt32(0),
							FechaAlta = reader.GetDateTime(1),
							FechaBaja = reader.GetDateTime(2),
							IdGarante = reader.GetInt32(3),
							garante = new Garante
							{
								//IdPropietario = reader.GetInt32(8),
								Nombre = reader.GetString(6),
								Apellido = reader.GetString(7),
							},
							IdInquilino = reader.GetInt32(4),
							inquilino = new Inquilino
							{
								//IdPropietario = reader.GetInt32(8),
								Nombre = reader.GetString(8),
								Apellido = reader.GetString(9),
							},

							IdInmueble = reader.GetInt32(5),
							inmueble = new Inmueble
							{
								//IdPropietario = reader.GetInt32(8),
								Precio = reader.GetDecimal(10)
							}

						};//termina entidad
						res.Add(entidad);
					}//termina whilw
					connection.Close();
				}//termina using	
			}

			return res;
		}
	}	
}
