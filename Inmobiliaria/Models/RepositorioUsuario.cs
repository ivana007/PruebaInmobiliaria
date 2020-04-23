using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public class RepositorioUsuario
    {

		private readonly string connectionString;
		private readonly IConfiguration configuration;

		public RepositorioUsuario(IConfiguration configuration)
		{
			this.configuration = configuration;
			//connectionString =configuration["connectionString:DefaultConnection"];
			connectionString = configuration["ConnectionStrings:DefaultConnection"];
		}

		public int Alta(Usuario p)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Usuarios (Nombre, Apellido, Mail, Clave , Rol, Estado ) " +
					$"VALUES (@nombre, @apellido, @mail, @clave, @rol, @estado);" +
					$"SELECT SCOPE_IDENTITY();";//devuelve el id insertado
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", p.Nombre);
					command.Parameters.AddWithValue("@apellido", p.Apellido);
					command.Parameters.AddWithValue("@mail", p.Mail);
					command.Parameters.AddWithValue("@clave", p.Clave);
					command.Parameters.AddWithValue("@rol", p.Rol);
					command.Parameters.AddWithValue("@estado", p.Estado);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					p.IdUsuario = res;
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
				string sql = $"DELETE FROM Usuarios WHERE IdUsuario = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public int BajaLogica(Usuario p)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Usuarios SET Nombre=@nombre, Apellido=@apellido, Mail=@mail, Clave=@clave, Rol=@rol, Estado=@estado " +
					$"WHERE IdUsuario = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", p.IdUsuario);
					//command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", p.Nombre);
					command.Parameters.AddWithValue("@apellido", p.Apellido);
					command.Parameters.AddWithValue("@mail", p.Mail);
					command.Parameters.AddWithValue("@telefono", p.Clave);
					command.Parameters.AddWithValue("@mail", p.Rol);
					command.Parameters.AddWithValue("@estado", p.Estado);

					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}



		public int Modificacion(Usuario p)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Usuarios SET Nombre=@nombre, Apellido=@apellido, Mail=@mail, Clave=@clave, Rol=@rol, Estado=@estado " +
					$"WHERE IdUsuario = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", p.IdUsuario);
					//command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", p.Nombre);
					command.Parameters.AddWithValue("@apellido", p.Apellido);
					command.Parameters.AddWithValue("@mail", p.Mail);
					command.Parameters.AddWithValue("@telefono", p.Clave);
					command.Parameters.AddWithValue("@mail", p.Rol);
					command.Parameters.AddWithValue("@estado", p.Estado);

					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Usuario> ObtenerTodos()
		{
			IList<Usuario> res = new List<Usuario>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdUsuario, Nombre, Apellido, Mail,  Clave, Rol ,Estado  FROM Usuarios" +
					$" WHERE Estado LIKE '1'";
				;
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Usuario p = new Usuario
						{
							IdUsuario = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Mail = reader.GetString(3),
							Clave = reader.GetString(4),
							Rol = reader.GetString(5),
							Estado= reader.GetString(6),


						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Usuario ObtenerPorId(int id)
		{
			Usuario p = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdUsuario, Nombre, Apellido, Mail, Clave,Rol,Estado FROM Usuarios" +
					$" WHERE IdUsuario=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						p = new Usuario
						{
							IdUsuario = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Mail = reader.GetString(3),
							Clave = reader.GetString(4),
							Rol = reader.GetString(5),
							Estado= reader.GetString(6),
						};
					}
					connection.Close();
				}
			}
			return p;
		}

		public Usuario ObtenerPorEmail(string email)
		{
			Usuario p = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdUsuario, Nombre, Apellido, Mail, Clave,Rol,Estado FROM Usuarios"  +
					$" WHERE Mail=@mail";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.Add("@mail", SqlDbType.VarChar).Value = email;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						p = new Usuario
						{
							IdUsuario = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Mail = reader.GetString(3),
							Clave = reader.GetString(4),
							Rol = reader.GetString(5),
							Estado = reader.GetString(6),
						};
					}
					connection.Close();
				}
			}
			return p;
		}

		public IList<Usuario> BuscarPorNombre(string nombre)
		{
			List<Usuario> res = new List<Usuario>();
			Usuario p = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdUsuario, Nombre, Apellido, Mail, Clave, Rol, Estado FROM Usuarios" +
					$" WHERE Nombre LIKE %@nombre% OR Apellido LIKE %@nombre";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = nombre;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						p = new Usuario
						{
							IdUsuario = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Mail = reader.GetString(3),
							Clave = reader.GetString(4),
							Rol= reader.GetString(5),
							Estado = reader.GetString(6),
						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}

	}
}
