using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GoodStore
{
    public class GoodStoreRepository : IDisposable
    {
        private SqlConnection connection;

        public GoodStoreRepository()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            IConfigurationRoot config = builder.Build();
            string conStr = config["connectionString"];
            connection = new SqlConnection(conStr);
        }

        public void Dispose()
        {
            connection.Dispose();
        }

        public List<IGood> GetGoods()
        {
            List<IGood> goods = new List<IGood>();
            SqlCommand cmd = new SqlCommand("select * from good", connection);
            connection.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    goods.Add(new Good
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Unit = reader.GetString(2),
                        Price = reader.GetInt32(3),
                        Amount = reader.GetInt32(4)
                    });
                }
            }
            connection.Close();
            return goods;
        }

        public bool AddGood(IGood good)
        {
            string cmdStr = @"INSERT INTO Good (Name, Unit, Price, Amount)
                        VALUES (@Name, @Unit, @Price, @Amount)";
            SqlCommand cmd = new SqlCommand(cmdStr, connection);
            cmd.Parameters.AddWithValue("@Name", good.Name);
            cmd.Parameters.AddWithValue("@Unit", good.Unit);
            cmd.Parameters.AddWithValue("@Price", good.Price);
            cmd.Parameters.AddWithValue("@Amount", 0);
            connection.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }

        public bool AddShipment(IShipment shipment, IEnumerable<IShipmentPart> shipmentParts)
        {
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            string cmdStr = @"INSERT INTO Shipment (Import, Date)
                        VALUES (@Import, @Date);
                        SELECT CAST(scope_identity() AS int)";
            SqlCommand cmd = new SqlCommand(cmdStr, connection, transaction);
            cmd.Parameters.AddWithValue("@Import", shipment.Import);
            cmd.Parameters.AddWithValue("@Date", shipment.Date);
            try
            {
                int shipmentId = (int)cmd.ExecuteScalar();

                foreach (IShipmentPart sp in shipmentParts)
                {
                    cmdStr = @"INSERT INTO ShipmentPart (GoodId, ShipmentId, Amount)
                        VALUES (@GoodId, @ShipmentId, @Amount);
                        UPDATE Good
                        SET Amount = Amount + @DeltaAmount
                        WHERE Id = @GoodId;";
                    cmd = new SqlCommand(cmdStr, connection, transaction);
                    cmd.Parameters.AddWithValue("@GoodId", sp.GoodId);
                    cmd.Parameters.AddWithValue("@ShipmentId", shipmentId);
                    cmd.Parameters.AddWithValue("@Amount", sp.Amount);
                    cmd.Parameters.AddWithValue("@DeltaAmount", sp.Amount * (shipment.Import ? 1 : -1));
                    cmd.Parameters.AddWithValue("@NewAmount", sp.Amount);
                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }
    }
}
