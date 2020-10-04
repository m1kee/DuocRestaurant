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
                booking.Code = Convert.ToInt32($"{booking.UserId}{booking.TableId}{booking.Date:yyMMddHH}");

                string query = $"INSERT INTO Reserva (" +
                    $"{Booking.ColumnNames.UserId}, " +
                    $"{Booking.ColumnNames.Code}, " +
                    $"{Booking.ColumnNames.TableId}, " +
                    $"{Booking.ColumnNames.Date}, " +
                    $"{Booking.ColumnNames.Diners}, " +
                    $"{Booking.ColumnNames.Active} " +
                    $") VALUES (" +
                    $"{booking.UserId}, " +
                    $"{booking.Code}, " +
                    $"{booking.TableId}, " +
                    $"TO_DATE('{booking.Date:ddMMyyyyHHmm}', 'DDMMYYYYHH24MI'), " +
                    $"{booking.Diners}, " +
                    $"{1} " +
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
                booking.Date = booking.Date.ToLocalTime();

                result = booking;
            }

            return result;
        }

        public bool Delete(RestaurantDatabaseSettings ctx, int bookingId)
        {
            bool result = false;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"UPDATE Reserva " +
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
                string query = $"UPDATE Reserva " +
                    $"SET " +
                    $"{Booking.ColumnNames.UserId} = {booking.UserId}, " +
                    $"{Booking.ColumnNames.TableId} = {booking.TableId}, " +
                    $"{Booking.ColumnNames.Date} = TO_DATE('{booking.Date:ddMMyyyyHHmm}', 'DDMMYYYYHH24MI'), " +
                    $"{Booking.ColumnNames.Diners} = {booking.Diners}, " +
                    $"{Booking.ColumnNames.Active} = {(booking.Active ? 1 : 0)} " +
                    $"WHERE {Booking.ColumnNames.Id} = {bookingId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                cmd.ExecuteNonQuery();

                booking.Date = booking.Date.ToLocalTime();
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
                    $"r.{Booking.ColumnNames.Id}, " +
                    $"r.{Booking.ColumnNames.Code}, " +
                    $"r.{Booking.ColumnNames.UserId}, " +
                    $"r.{Booking.ColumnNames.TableId}, " +
                    $"r.{Booking.ColumnNames.Date}, " +
                    $"r.{Booking.ColumnNames.Diners}, " +
                    $"r.{Booking.ColumnNames.Active}, " +
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
                    $"FROM Reserva r " +
                    $"JOIN Mesa m ON m.Id = r.{Booking.ColumnNames.TableId} " +
                    $"JOIN Usuario u ON u.Id = r.{Booking.ColumnNames.UserId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new Booking()
                    {
                        Id = Convert.ToInt32(reader[Booking.ColumnNames.Id]),
                        Code = Convert.ToInt32(reader[Booking.ColumnNames.Code]),
                        UserId = Convert.ToInt32(reader[Booking.ColumnNames.UserId]),
                        TableId = Convert.ToInt32(reader[Booking.ColumnNames.TableId]),
                        Date = Convert.ToDateTime(reader[Booking.ColumnNames.Date]).ToLocalTime(),
                        Diners = Convert.ToInt32(reader[Booking.ColumnNames.Diners]),
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
                    $"r.{Booking.ColumnNames.Id}, " +
                    $"r.{Booking.ColumnNames.Code}, " +
                    $"r.{Booking.ColumnNames.UserId}, " +
                    $"r.{Booking.ColumnNames.TableId}, " +
                    $"r.{Booking.ColumnNames.Date}, " +
                    $"r.{Booking.ColumnNames.Diners}, " +
                    $"r.{Booking.ColumnNames.Active}, " +
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
                    $"FROM Reserva r " +
                    $"JOIN Mesa m ON m.Id = r.{Booking.ColumnNames.TableId} " +
                    $"JOIN Usuario u ON u.Id = r.{Booking.ColumnNames.UserId} " +
                    $"WHERE r.{Booking.ColumnNames.Id} = {bookingId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {

                    result = new Booking()
                    {
                        Id = Convert.ToInt32(reader[Booking.ColumnNames.Id]),
                        Code = Convert.ToInt32(reader[Booking.ColumnNames.Code]),
                        UserId = Convert.ToInt32(reader[Booking.ColumnNames.UserId]),
                        TableId = Convert.ToInt32(reader[Booking.ColumnNames.TableId]),
                        Date = Convert.ToDateTime(reader[Booking.ColumnNames.Date]).ToLocalTime(),
                        Diners = Convert.ToInt32(reader[Booking.ColumnNames.Diners]),
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

        public Booking GetByCode(RestaurantDatabaseSettings ctx, int bookingCode)
        {
            Booking result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"SELECT " +
                    $"r.{Booking.ColumnNames.Id}, " +
                    $"r.{Booking.ColumnNames.Code}, " +
                    $"r.{Booking.ColumnNames.UserId}, " +
                    $"r.{Booking.ColumnNames.TableId}, " +
                    $"r.{Booking.ColumnNames.Date}, " +
                    $"r.{Booking.ColumnNames.Diners}, " +
                    $"r.{Booking.ColumnNames.Active}, " +
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
                    $"FROM Reserva r " +
                    $"JOIN Mesa m ON m.Id = r.{Booking.ColumnNames.TableId} " +
                    $"JOIN Usuario u ON u.Id = r.{Booking.ColumnNames.UserId} " +
                    $"WHERE r.{Booking.ColumnNames.Code} = {bookingCode}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {

                    result = new Booking()
                    {
                        Id = Convert.ToInt32(reader[Booking.ColumnNames.Id]),
                        Code = Convert.ToInt32(reader[Booking.ColumnNames.Code]),
                        UserId = Convert.ToInt32(reader[Booking.ColumnNames.UserId]),
                        TableId = Convert.ToInt32(reader[Booking.ColumnNames.TableId]),
                        Date = Convert.ToDateTime(reader[Booking.ColumnNames.Date]).ToLocalTime(),
                        Diners = Convert.ToInt32(reader[Booking.ColumnNames.Diners]),
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
