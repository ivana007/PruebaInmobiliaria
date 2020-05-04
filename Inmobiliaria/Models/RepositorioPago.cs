using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public class RepositorioPago
    {
        private readonly string connectionString;
        private readonly IConfiguration configuration;

        public RepositorioPago(IConfiguration configuration)
        {
            this.configuration = configuration;
            
            connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }
		public int Alta(Pago p)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Pagos (FechaPago, NumeroPago, Importe ,IdContrato) " +
					$"VALUES (@fechaPago, @numeroPago, @importe ,@idContrato);" +
					$"SELECT SCOPE_IDENTITY();";//devuelve el id insertado
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@fechaPago", p.FechaPago);
					command.Parameters.AddWithValue("@numeroPago", p.NumeroPago);
					command.Parameters.AddWithValue("@importe", p.Importe);
					command.Parameters.AddWithValue("@idContrato", p.IdContrato);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					p.IdPago = res;
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
				string sql = $"DELETE FROM Pagos WHERE IdPago = @id";
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

		public int BajaLogica(Pago p)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Pagos SET FechaPago=@fechaPago, NumeroPago=@numeroPago, Importe=@importe ,IdContrato=@idContrato" +
					$"WHERE IdPago = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", p.IdPago);
					//command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@fechaPago", p.FechaPago);
					command.Parameters.AddWithValue("@numeroPago", p.NumeroPago);
					command.Parameters.AddWithValue("@importe", p.Importe);
					command.Parameters.AddWithValue("@idContrato", p.IdContrato);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}



		public int Modificacion(Pago p)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Pagos SET FechaPago=@fechaPago, NumeroPago=@numeroPago, Importe=@importe " +
					$"WHERE IdPago = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@fechaPago", p.FechaPago);
					command.Parameters.AddWithValue("@numeroPago", p.NumeroPago);
					command.Parameters.AddWithValue("@importe", p.Importe);
					command.Parameters.AddWithValue("@id", p.IdPago);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Pago> ObtenerTodos()
		{
			IList<Pago> res = new List<Pago>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "SELECT  IdPago, FechaPago, NumeroPago, Importe, p.IdContrato, c.IdInquilino, i.Nombre , i.Apellido" +
					" FROM Pagos p INNER JOIN Contratos c ON p.IdContrato = c.IdContrato" +
					" INNER JOIN Inquilinos i ON c.IdInquilino = i.IdInquilino";
					
					
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Pago p = new Pago
						{
							IdPago = reader.GetInt32(0),
							FechaPago = reader.GetDateTime(1),
							NumeroPago = reader.GetInt32(2),
							Importe = reader.GetDecimal(3),
							IdContrato= reader.GetInt32(4),
							contrato= new Contrato
							{ 
								IdInquilino= reader.GetInt32(5),
								inquilino = new Inquilino
								{
									Nombre = reader.GetString(6),
									Apellido = reader.GetString(7),
								},
							}
							
							
						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Pago ObtenerPorId(int id)
		{
			Pago p = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdPago, FechaPago, NumeroPago, Importe  FROM Pagos" +
					$" WHERE IdPago=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						p = new Pago
						{
							IdPago = reader.GetInt32(0),
							FechaPago = reader.GetDateTime(1),
							NumeroPago = reader.GetInt32(2),
							Importe = reader.GetDecimal(3),
							
						};
					}
					connection.Close();
				}
			}
			return p;
		}

		public int ObtenerCantidadPagos(int id)// debere pasar el id del contrato
		{
			int  p = 0;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT COUNT(NumeroPago)  FROM Pagos" +
					$" WHERE IdContrato=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						p = reader.GetInt32(0);
					}
					connection.Close();
				}
			}
			return p;
		}


	}
}
