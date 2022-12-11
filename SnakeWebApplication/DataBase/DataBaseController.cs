using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Npgsql;
namespace SnakeWebApplication.DataBase;

public class DataBaseController: IDataBaseController
{
    private readonly NpgsqlConnection _dbConnection;

    public DataBaseController()
    {
        _dbConnection = SnakeDataBaseConnector.CreateSession();
    }
    
    public string GetHighScore(string username)
    {
        _dbConnection.Open();
        var sql = "";

        sql = @$"
        SELECT id FROM ""Account""
        WHERE username like '{username}'
        ";
        using var cmdSelect = new NpgsqlCommand(sql, _dbConnection);

        if (int.TryParse(cmdSelect.ExecuteScalar()?.ToString(), out var userId))
        {
            sql =
                @$"
            SELECT Sc.score_amount
            FROM ""Score"" Sc
            JOIN ""Account"" Acc on Acc.id = Sc.user_id
            WHERE user_id = {userId}
            ORDER BY Sc.score_amount DESC 
            LIMIT 1
            ";
            using var cmd = new NpgsqlCommand(sql, _dbConnection);
            var result = cmd.ExecuteScalar()?.ToString();
        
            _dbConnection.Close();
        
            return result;
        }

        _dbConnection.Close();
        throw new NullReferenceException("Invalid username");
    }

    public bool LogInUser(string username, string password)
    {
        var hashedPassword = Hasher.HashString(password);
        
        var sql = 
            @$"
            SELECT * from ""Account"" Ac
            WHERE username like '{username}' 
              and hash_password like '{hashedPassword}'
            ";
        
        _dbConnection.Open();
        
        using var cmd = new NpgsqlCommand(sql, _dbConnection);
        var result = cmd.ExecuteScalar()?.ToString();
        
        _dbConnection.Close();
        
        return result != null;
    }

    public bool RegisterUser(string username, string password)
    {
        var hashedPassword = Hasher.HashString(password);
        
        var sql = 
            @$"
            INSERT INTO ""Account""
            (username, hash_password)
            SELECT '{username}', '{hashedPassword}'
            WHERE
                NOT EXISTS (
                    SELECT * FROM ""Account""
                            WHERE username = '{username}')
            ";
        
        _dbConnection.Open();
        
        using var cmd = new NpgsqlCommand(sql, _dbConnection);
        var result = cmd.ExecuteNonQuery();
        
        _dbConnection.Close();
        
        return result > 0;
    }

    public void SetScore(string username, int score)
    {
        _dbConnection.Open();
        var sql = "";

        sql = @$"
        SELECT id FROM ""Account""
        WHERE username like '{username}'
        ";
        using var cmdSelect = new NpgsqlCommand(sql, _dbConnection);
        
        if (int.TryParse(cmdSelect.ExecuteScalar()?.ToString(), out var userId))
        {
            sql = @$"
            INSERT INTO ""Score"" (user_id, score_amount)
            SELECT'{userId}', {score}
            ";
            using var cmdInsert = new NpgsqlCommand(sql, _dbConnection);
            cmdInsert.ExecuteNonQuery();
            _dbConnection.Close();
        }
        else
        {
            _dbConnection.Close();
            throw new NullReferenceException("Invalid username");
        }
    }
    public bool SetCurrentUser(string username, string password)
    {
        var hashedPassword = Hasher.HashString(password);
        
        var sql = 
            @$"
            INSERT INTO ""Account""
            (username, hash_password)
            SELECT '{username}', '{hashedPassword}'
            WHERE
                NOT EXISTS (
                    SELECT * FROM ""Account""
                            WHERE username = '{username}')
            ";
        
        _dbConnection.Open();
        
        using var cmd = new NpgsqlCommand(sql, _dbConnection);
        var result = cmd.ExecuteNonQuery();
        
        _dbConnection.Close();
        
        return result > 0;
    }
    
}