using Domain;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public class BookingService : IBookingService
    {
        public Booking Add(RestaurantDatabaseSettings ctx, Booking booking)
        {
            Booking result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"INSERT INTO Agenda (" +
                    $"{Booking.ColumnNames.UserId}, " +
                    $"{Booking.ColumnNames.TableId}, " +
                    $"{Booking.ColumnNames.Date}, " +
                    $"{Booking.ColumnNames.Active} " +
                    $") VALUES (" +
                    $"{booking.UserId}, " +
                    $"{booking.TableId}, " +
                    $"TO_DATE('{booking.Date:YYYY-mm-dd HH:mm:ss}', 'YYYY-MM-DD HH24:mm:ss'), " +
                    $"{1}, " +
                    $") RETURNING {Booking.ColumnNames.Id} INTO :{Booking.ColumnNames.Id}";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(new OracleParameter()
                {
                    ParameterName = $":{Booking.ColumnNames.Id}",
                    OracleDbType = OracleDbType.Decimal,
                    Direction = System.Data.ParameterDirection.Output
                });

                conn.Open();

                cmd.ExecuteNonQuery();

                booking.Id = Convert.ToInt32(cmd.Parameters[$":{Booking.ColumnNames.Id}"].Value.ToString());

                result = booking;
            }

            return result;
        }

        public bool Delete(RestaurantDatabaseSettings ctx, int bookingId)
        {
            bool result = false;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"UPDATE Agenda " +
                    $"SET {Booking.ColumnNames.Active} = 0 " +
                    $"WHERE {Booking.ColumnNames.Id} = {bookingId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                cmd.ExecuteNonQuery();

                result = true;
            }

            return result;
        }

        public Booking Edit(RestaurantDatabaseSettings ctx, int bookingId, Booking booking)
        {
            Booking result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"UPDATE Agenda " +
                    $"SET " +
                    $"{Booking.ColumnNames.UserId} = {booking.UserId}, " +
                    $"{Booking.ColumnNames.TableId} = {booking.TableId}, " +
                    $"{Booking.ColumnNames.Date} = {booking.Date}, " +
                    $"{Booking.ColumnNames.Active} = {(booking.Active ? 1 : 0)} " +
                    $"WHERE {Booking.ColumnNames.Id} = {bookingId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                cmd.ExecuteNonQuery();

                result = booking;
            }

            return result;
        }

        public IList<Booking> Get(RestaurantDatabaseSettings ctx)
        {
            IList<Booking> result = new List<Booking>();

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"SELECT " +
                    $"a.{Booking.ColumnNames.Id}, " +
                    $"a.{Booking.ColumnNames.UserId}, " +
                    $"a.{Booking.ColumnNames.TableId}, " +
                    $"a.{Booking.ColumnNames.Date}, " +
                    $"a.{Booking.ColumnNames.Active}, " +
                    $"m.{Table.ColumnNames.Id} AS Table{Table.ColumnNames.Id}, " +
                    $"m.{Table.ColumnNames.Number} AS Table{Table.ColumnNames.Number}, " +
                    $"m.{Table.ColumnNames.Capacity} AS Table{Table.ColumnNames.Capacity}, " +
                    $"m.{Table.ColumnNames.Description} AS Table{Table.ColumnNames.Description}, " +
                    $"m.{Table.ColumnNames.Active} AS Table{Table.ColumnNames.Active}, " +
                    $"m.{Table.ColumnNames.InUse} AS Table{Table.ColumnNames.InUse}, " +
                    $"u.{User.ColumnNames.Id} AS User{User.ColumnNames.Id}, " +
                    $"u.{User.ColumnNames.RoleId} AS User{User.ColumnNames.RoleId}, " +
                    $"u.{User.ColumnNames.Name} AS User{User.ColumnNames.Name}, " +
                    $"u.{User.ColumnNames.LastName} AS User{User.ColumnNames.LastName}, " +
                    $"u.{User.ColumnNames.Email} AS User{User.ColumnNames.Email}, " +
                    $"u.{User.ColumnNames.Phone} AS User{User.ColumnNames.Phone}, " +
                    $"u.{User.ColumnNames.Address} AS User{User.ColumnNames.Address} " +
                    $"FROM Agenda a " +
                    $"JOIN Mesa m ON m.Id = a.{Booking.ColumnNames.TableId} " +
                    $"JOIN Usuario u ON u.Id = a.{Booking.ColumnNames.UserId} " +
                    $"WHERE a.{Booking.ColumnNames.Active} = 1";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new Booking()
                    {
                        Id = Convert.ToInt32(reader[Booking.ColumnNames.Id]),
                        UserId = Convert.ToInt32(reader[Booking.ColumnNames.UserId]),
                        TableId = Convert.ToInt32(reader[Booking.ColumnNames.TableId]),
                        Date = Convert.ToDateTime(reader[Booking.ColumnNames.Date]),
                        Active = Convert.ToBoolean(reader[Booking.ColumnNames.Active]),
                        Table = new Table()
                        {
                            Id = Convert.ToInt32(reader[$"Table{Table.ColumnNames.Id}"]),
                            Number = Convert.ToInt32(reader[$"Table{Table.ColumnNames.Number}"]),
                            Capacity = Convert.ToInt32(reader[$"Table{Table.ColumnNames.Capacity}"]),
                            Description = reader[$"Table{Table.ColumnNames.Description}"]?.ToString(),
                            Active = Convert.ToBoolean(reader[$"Table{Table.ColumnNames.Active}"]),
                            InUse = Convert.ToBoolean(reader[$"Table{Table.ColumnNames.InUse}"])
                        },
                        User = new User()
                        {
                            Id = Convert.ToInt32(reader[$"User{User.ColumnNames.Id}"]),
                            Email = reader[$"User{User.ColumnNames.Email}"]?.ToString(),
                            Name = reader[$"User{User.ColumnNames.Name}"]?.ToString(),
                            LastName = reader[$"User{User.ColumnNames.LastName}"]?.ToString(),
                            Address = reader[$"User{User.ColumnNames.Address}"]?.ToString(),
                            Phone = reader[$"User{User.ColumnNames.Phone}"]?.ToString(),
                            RoleId = Convert.ToInt32(reader[$"User{User.ColumnNames.RoleId}"])
                        }
                    });
                }

                reader.Dispose();
            }

            return result;
        }

        public Booking Get(RestaurantDatabaseSettings ctx, int bookingId)
        {
            Booking result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"SELECT " +
                    $"a.{Booking.ColumnNames.Id}, " +
                    $"a.{Booking.ColumnNames.UserId}, " +
                    $"a.{Booking.ColumnNames.TableId}, " +
                    $"a.{Booking.ColumnNames.Date}, " +
                    $"a.{Booking.ColumnNames.Active}, " +
                    $"m.{Table.ColumnNames.Id} AS Table{Table.ColumnNames.Id}, " +
                    $"m.{Table.ColumnNames.Number} AS Table{Table.ColumnNames.Number}, " +
                    $"m.{Table.ColumnNames.Capacity} AS Table{Table.ColumnNames.Capacity}, " +
                    $"m.{Table.ColumnNames.Description} AS Table{Table.ColumnNames.Description}, " +
                    $"m.{Table.ColumnNames.Active} AS Table{Table.ColumnNames.Active}, " +
                    $"m.{Table.ColumnNames.InUse} AS Table{Table.ColumnNames.InUse}, " +
                    $"u.{User.ColumnNames.Id} AS User{User.ColumnNames.Id}, " +
                    $"u.{User.ColumnNames.RoleId} AS User{User.ColumnNames.RoleId}, " +
                    $"u.{User.ColumnNames.Name} AS User{User.ColumnNames.Name}, " +
                    $"u.{User.ColumnNames.LastName} AS User{User.ColumnNames.LastName}, " +
                    $"u.{User.ColumnNames.Email} AS User{User.ColumnNames.Email}, " +
                    $"u.{User.ColumnNames.Phone} AS User{User.ColumnNames.Phone}, " +
                    $"u.{User.ColumnNames.Address} AS User{User.ColumnNames.Address} " +
                    $"FROM Agenda a " +
                    $"JOIN Mesa m ON m.Id = a.{Booking.ColumnNames.TableId} " +
                    $"JOIN Usuario u ON u.Id = a.{Booking.ColumnNames.UserId} " +
                    $"WHERE p.{Booking.ColumnNames.Id} = {bookingId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {

                    result = new Booking()
                    {
                        Id = Convert.ToInt32(reader[Booking.ColumnNames.Id]),
                        UserId = Convert.ToInt32(reader[Booking.ColumnNames.UserId]),
                        TableId = Convert.ToInt32(reader[Booking.ColumnNames.TableId]),
                        Date = Convert.ToDateTime(reader[Booking.ColumnNames.Date]),
                        Active = Convert.ToBoolean(reader[Booking.ColumnNames.Active]),
                        Table = new Table()
                        {
                            Id = Convert.ToInt32(reader[$"Table{Table.ColumnNames.Id}"]),
                            Number = Convert.ToInt32(reader[$"Table{Table.ColumnNames.Number}"]),
                            Capacity = Convert.ToInt32(reader[$"Table{Table.ColumnNames.Capacity}"]),
                            Description = reader[$"Table{Table.ColumnNames.Description}"]?.ToString(),
                            Active = Convert.ToBoolean(reader[$"Table{Table.ColumnNames.Active}"]),
                            InUse = Convert.ToBoolean(reader[$"Table{Table.ColumnNames.InUse}"])
                        },
                        User = new User()
                        {
                            Id = Convert.ToInt32(reader[$"User{User.ColumnNames.Id}"]),
                            Email = reader[$"User{User.ColumnNames.Email}"]?.ToString(),
                            Name = reader[$"User{User.ColumnNames.Name}"]?.ToString(),
                            LastName = reader[$"User{User.ColumnNames.LastName}"]?.ToString(),
                            Address = reader[$"User{User.ColumnNames.Address}"]?.ToString(),
                            Phone = reader[$"User{User.ColumnNames.Phone}"]?.ToString(),
                            RoleId = Convert.ToInt32(reader[$"User{User.ColumnNames.RoleId}"])
                        }
                    };
                }

                reader.Dispose();
            }

            return result;
        }
    }
}
