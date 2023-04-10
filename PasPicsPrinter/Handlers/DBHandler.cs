using Microsoft.Data.Sqlite;
using PasPicsPrinter.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PasPicsPrinter.Handlers
{
    public static class DBHandler
    {
        private const string path = @".\localDbLog.db"; 
        private const string connString = @"Data Source=" + path + @";"; 

        public static FakturaModel StoreFinishedOrders(OrderModel orderModel, DateTime date)
        {
            string fakturaNr = GetNextFakturaNumber().ToString();
            var imageNames = orderModel.images.Select(x => x.imageName).ToList();
            var guid = Guid.NewGuid(); 
            var faktura = new FakturaModel() { 
                fakturaNumber = fakturaNr, 
                customerModel = orderModel.customer, 
                dateShipped = date, 
                guid = guid, 
                imageNames = imageNames, 
                isPayed = false, 
                datePayed = DateTime.MinValue }; 
            
            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open(); 
                using (var command = new SqliteCommand(
                    "INSERT INTO fakturas (guid, fakturaNumber, customer, dateShipped, datePayed, isPayed) VALUES(@guid, @fakturaNumber, @customer, @dateShipped, @datePayed, @isPayed); ", conn))
                {
                    command.Parameters.Add(new SqliteParameter("guid", SqliteType.Text) { Value = guid.ToString() });
                    command.Parameters.Add(new SqliteParameter("fakturaNumber", SqliteType.Integer) { Value = fakturaNr });
                    command.Parameters.Add(new SqliteParameter("customer", SqliteType.Text) { Value = faktura.customerModel.customerName.ToString() });
                    command.Parameters.Add(new SqliteParameter("dateShipped", SqliteType.Text) { Value = date.ToShortDateString() });
                    command.Parameters.Add(new SqliteParameter("datePayed", SqliteType.Text) { Value = "not payed" });
                    command.Parameters.Add(new SqliteParameter("isPayed", SqliteType.Integer) { Value = false });

                    command.ExecuteNonQuery(); 
                }
            }

            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                using(var command = new SqliteCommand("", conn))
                {
                    for (int i = 0; i < imageNames.Count; i++)
                    {
                        command.CommandText = "INSERT INTO fakturaImages (fakturaGuid, imageName) VALUES(@fakturaGuid, @imageName); "; 

                        command.Parameters.Add(new SqliteParameter("fakturaGuid", SqliteType.Text) { Value = guid });
                        command.Parameters.Add(new SqliteParameter("imageName", SqliteType.Text) { Value = imageNames[i] });

                        command.ExecuteNonQuery();
                    }
                }
            } 

            return faktura; 
        }

        public static void StoreNewCustomer(CustomerModel customerModel) 
        {
            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();
                using (var command = new SqliteCommand(
                    "INSERT INTO customers (customerName, contact, address, payment) VALUES(@customerName, @contact, @address, @payment); ", conn))
                {
                    command.Parameters.Add(new SqliteParameter("customerName", SqliteType.Text) { Value = customerModel.customerName });
                    command.Parameters.Add(new SqliteParameter("contact", SqliteType.Text) { Value = customerModel.customerContact });
                    command.Parameters.Add(new SqliteParameter("address", SqliteType.Text) { Value = customerModel.customerAddress });
                    command.Parameters.Add(new SqliteParameter("payment", SqliteType.Text) { Value = customerModel.customerPayment });

                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateCustomer(string username, string toChange, string newValue)
        {
            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open(); 
                using (var command = new SqliteCommand("UPDATE customers SET @toChange = @newValue WHERE customerName = @customerName", conn))
                {
                    command.Parameters.Add(new SqliteParameter("customerName", SqliteType.Text) { Value = username });

                    switch (toChange)
                    {
                        case ("address"): 
                            {
                                command.Parameters.Add(new SqliteParameter("address", SqliteType.Text) { Value = newValue }); 
                                break;
                            }
                        case ("payment"): 
                            {
                                command.Parameters.Add(new SqliteParameter("payment", SqliteType.Text) { Value = newValue });
                                break;
                            }
                        case ("contact"): 
                            {
                                command.Parameters.Add(new SqliteParameter("contact", SqliteType.Text) { Value = newValue });
                                break;
                            }
                    }

                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<FakturaModel> GetUnfinishedFakturas()
        {
            List<FakturaModel> toReturn = new List<FakturaModel>();

            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();
                using (var command = new SqliteCommand("SELECT fakturas.*, (SELECT COUNT(*) FROM fakturaImages WHERE fakturaImages.fakturaGuid = fakturas.guid) AS TOT FROM fakturas; ", conn)) 
                {
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var faktura = new FakturaModel()
                        {
                            guid = new Guid(reader["guid"].ToString()),
                            dateShipped = DateTime.Parse(reader["dateShipped"].ToString()),
                            isPayed = false,
                            fakturaNumber = reader["fakturaNumber"].ToString(),
                            imageNames = new List<Guid>(), datePayed = DateTime.MinValue, 
                            customerModel = new CustomerModel() { 
                                customerAddress = reader["address"].ToString(), 
                                customerContact = reader["contact"].ToString(), 
                                customerName = reader["customerName"].ToString(), 
                                customerPayment = reader["payment"].ToString() }
                        };

                        toReturn.Add(faktura); 
                    }
                }
            }

            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();
                using (var command = new SqliteCommand("", conn))
                {
                    for (int i = 0; i < toReturn.Count; i++)
                    {
                        command.Parameters.Clear();
                        command.CommandText = "SELECT * FROM fakturaImages WHERE fakturaGuid = @guid; ";
                        command.Parameters.Add(new SqliteParameter("fakturaGuid", SqliteType.Text) { Value = toReturn[i].guid });

                        var reader = command.ExecuteReader();

                        List<Guid> imageNames = new List<Guid>(); 
                        while (reader.Read())
                        {
                            imageNames.Add(new Guid(reader["imageName"].ToString())); 
                        }
                        toReturn[i].imageNames = imageNames; 
                    }
                }
            }

            return toReturn; 
        }

        public static void SetFakturaToDone(Guid guid)
        {
            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();
                using (var command = new SqliteCommand("UPDATE fakturas SET isPayed = 1, datePayed = @datePayed WHERE guid = @guid; ", conn))
                {
                    command.Parameters.Add(new SqliteParameter("guid", SqliteType.Text) { Value = guid });
                    command.Parameters.Add(new SqliteParameter("datePayed", SqliteType.Text) { Value = DateTime.UtcNow.ToShortDateString() }); 

                    command.ExecuteNonQuery();
                }
            }
        }

        private static int GetNextFakturaNumber()
        {
            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();
                using (var command = new SqliteCommand("SELECT MAX(fakturaNumber) as max FORM fakturas; ", conn))
                {
                    var reader = command.ExecuteReader();                    
                    if (!reader.Read())
                    {
                        return 1; 
                    }                    
                    return Convert.ToInt32(reader["max"]) + 1; 
                }
            }
        }
    }
}
