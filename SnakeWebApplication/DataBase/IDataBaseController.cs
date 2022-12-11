using System.Collections.Generic;
using System.Data;

namespace SnakeWebApplication.DataBase;

public interface IDataBaseController
{
    public string GetHighScore(string username);
    public bool LogInUser(string username, string password );
    public bool RegisterUser(string username, string password);
    public void SetScore(string username, int score);
}