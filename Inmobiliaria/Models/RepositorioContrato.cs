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
				string sql = $"INSERT INTO Contratos (FechaAlta, fechaBaja, IdGarante, IdPago, IdInquilino, IdInmueble ) " +
					"VALUES (@fechaAlta, @fechaBaja, @idGarante, @idPago, @idInquilino, @idInmueble );" +
					"SELECT SCOPE_IDENTITY();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (var command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@direccion", entidad.FechaAlta);
					command.Parameters.AddWithValue("@tipoInmueble", entidad.FechaBaja);
					command.Parameters.AddWithValue("@precio", entidad.IdGarante);
					command.Parameters.AddWithValue("@cantHambientes", entidad.IdPago);
					command.Parameters.AddWithValue("@uso", entidad.IdInquilino);
					command.Parameters.AddWithValue("@estado", entidad.IdInmueble);
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
					"FechaAlta=@fechaAlta, FechaBaja=@fechaBaja, IdGarante=@idGarante, IdPago=@idPago, IdInquilino=@idInquilino, IdInmueble=@idInmueble " +
					"WHERE IdContrato = @idContrato";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@fechaAlta", entidad.FechaAlta);
					command.Parameters.AddWithValue("@fechaBaja", entidad.FechaBaja);
					command.Parameters.AddWithValue("@idGarante", entidad.IdGarante);
					command.Parameters.AddWithValue("@idPago", entidad.IdPago);
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
					"FechaAlta=@fechaAlta, FechaBaja=@fechaBaja, IdGarante=@idGarante, IdPago=@idPago, IdInquilino=@idInquilino, IdInmueble=@idInmueble " +
					"WHERE IdContrato = @idContrato";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@fechaAlta", entidad.FechaAlta);
					command.Parameters.AddWithValue("@fechaBaja", entidad.FechaBaja);
					command.Parameters.AddWithValue("@idGarante", entidad.IdGarante);
					command.Parameters.AddWithValue("@idPago", entidad.IdPago);
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
				string sql = "SELECT  IdContrato, FechaAlta, FechaBaja, c.IdGarante, c.IdPago, c.IdInquilino, c.IdInmueble ," +
					" g.Nombre, g.Apellido , p.Fechapago , i.Nombre,i.Apellido, ii.Precio " +
					" FROM Contratos c INNER JOIN Garantes g ON c.IdGarante = g.IdGarante " +
					"INNER JOIN Pagos p ON c.IdPago = p.IdPago " +
					"INNER JOIN Inquilinos i ON c.IdInquilino = i.IdInquilino " +
					"INNER JOIN Inmuebles ii ON c.IdInmueble = ii.IdInmueble" ;
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
							IdPago = reader.GetInt32(4),
							IdInquilino = reader.GetInt32(5),
							IdInmueble = reader.GetInt32(6),
							garante = new Garante
							{
								Nombre = reader.GetString(7),
								Apellido = reader.GetString(8),
							},
							pago=new Pago
							{ 
								FechaPago= reader.GetDateTime(9),
							},
							inquilino=new Inquilino
							{ 
								Nombre=reader.GetString(10),
								Apellido=reader.GetString(11),
							},
							inmueble=new Inmueble 
							{
								Precio=reader.GetDecimal(12),
							}

						};
						res.Add(entidad);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Inmueble ObtenerPorId(int id)
		{
			Inmueble entidad = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdInmueble, Direccion, TipoInmueble, Precio, CantHambientes, Uso, Estado, i.IdPropietario, p.Nombre, p.Apellido" +
					$" FROM Inmuebles i INNER JOIN Propietarios p ON i.IdPropietario = p.IdPropietario" +
					$" WHERE IdInmueble=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						entidad = new Inmueble
						{
							IdInmueble = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							TipoInmueble = reader.GetString(2),
							Precio = reader.GetDecimal(3),
							CantHambientes = reader.GetInt32(4),
							Uso = reader.GetString(5),
							Estado = reader.GetString(6),
							IdPropietario = reader.GetInt32(7),
							Propietario = new Propietario
							{
								//IdPropietario = reader.GetInt32(8),
								Nombre = reader.GetString(8),
								Apellido = reader.GetString(9),
							}
						};
					}
					connection.Close();
				}
			}
			return entidad;
		}

		public IList<Inmueble> BuscarPorPropietario(int idPropietario)
		{
			List<Inmueble> res = new List<Inmueble>();
			Inmueble entidad = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdInmueble, Direccion, TipoInmueble, Precio, CantHambientes, Uso, Estado, i.IdPropietario, p.Nombre, p.Apellido" +
					$" FROM Inmuebles i INNER JOIN Propietarios p ON i.IdPropietario = p.IdPropietario" +
					$" WHERE i.IdPropietario=@idPropietario";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@idPropietario", SqlDbType.Int).Value = idPropietario;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						entidad = new Inmueble
						{
							IdInmueble = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							TipoInmueble = reader.GetString(2),
							Precio = reader.GetDecimal(3),
							CantHambientes = reader.GetInt32(4),
							Uso = reader.GetString(5),
							Estado = reader.GetString(6),
							IdPropietario = reader.GetInt32(7),
							Propietario = new Propietario
							{
								//IdPropietario = reader.GetInt32(6),
								Nombre = reader.GetString(8),
								Apellido = reader.GetString(9),
							}
						};
						res.Add(entidad);
					}
					connection.Close();
				}
			}
			return res;
		}
	}
}
