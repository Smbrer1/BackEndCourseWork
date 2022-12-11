using System;
using Npgsql;
namespace SnakeWebApplication.DataBase;

public static class SnakeDataBaseConnector
{
    public static NpgsqlConnection CreateSession()
    {
        const string session = "Host=localhost;Username=postgres;Password=postgres;Database=SnakeWebApp";
        var connection = new NpgsqlConnection(session);
        return connection;
    }
    
}