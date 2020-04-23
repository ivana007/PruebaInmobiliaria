using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public class RepositorioGarante
    {
		private readonly string connectionString;
		private readonly IConfiguration configuration;

		public RepositorioGarante(IConfiguration configuration)
		{
			this.configuration = configuration;
			//connectionString =configuration["connectionString:DefaultConnection"];
			connectionString = configuration["ConnectionStrings:DefaultConnection"];
		}



		public int Alta(Garante g)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Garantes (Nombre, Apellido, Dni, Mail, Telefono ) " +
					$"VALUES (@nombre, @apellido, @dni, @mail, @telefono );" +
					$"SELECT SCOPE_IDENTITY();";//devuelve el id insertado
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", g.Nombre);
					command.Parameters.AddWithValue("@apellido", g.Apellido);
					command.Parameters.AddWithValue("@dni", g.Dni);
					
					command.Parameters.AddWithValue("@mail", g.Mail);
					command.Parameters.AddWithValue("@telefono", g.Telefono);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					g.IdGarante = res;
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
				string sql = $"DELETE FROM Garantes WHERE IdGarante = {id}";
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
		public int BajaLogica(Garante p)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Garantes SET Nombre=@nombre, Apellido=@apellido, Dni=@dni,  Mail=@mail, Telefono=@telefono " +
					$"WHERE IdGarante = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", p.IdGarante);
					//command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", p.Nombre);
					command.Parameters.AddWithValue("@apellido", p.Apellido);
					command.Parameters.AddWithValue("@dni", p.Dni);
					command.Parameters.AddWithValue("@telefono", p.Telefono);
					command.Parameters.AddWithValue("@mail", p.Mail);
					
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public int Modificacion(Garante p)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Garantes SET Nombre=@nombre, Apellido=@apellido, Dni=@dni,  Mail=@mail, Telefono=@telefono " +
					$"WHERE IdGarante = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", p.Nombre);
					command.Parameters.AddWithValue("@apellido", p.Apellido);
					command.Parameters.AddWithValue("@dni", p.Dni);
					command.Parameters.AddWithValue("@mail", p.Mail);
					command.Parameters.AddWithValue("@telefono", p.Telefono);
					command.Parameters.AddWithValue("@id", p.IdGarante);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Garante> ObtenerTodos()
		{
			IList<Garante> res = new List<Garante>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdGarante, Nombre, Apellido, Dni, Mail, Telefono FROM Garantes" ;
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Garante p = new Garante
						{
							IdGarante = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Dni = reader.GetString(3),
							
							Mail = reader.GetString(4),
							Telefono = reader.GetString(5),
						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Garante ObtenerPorId(int id)
		{
			Garante p = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdGarante, Nombre, Apellido, Dni,  Mail, Telefono  FROM Garantes" +
					$" WHERE IdGarante=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						p = new Garante
						{
							IdGarante = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Dni = reader.GetString(3),
							
							Mail = reader.GetString(4),
							Telefono = reader.GetString(5),
						};
					}
					connection.Close();
				}
			}
			return p;
		}

		
	}
}
