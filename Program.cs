using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;
using System.Data;
class Program
{
    static string GetConnectionString()
    {

        IConfiguration config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", true, true)
        .Build();

        var strConnection = config["ConnectionStrings:MyStoreDB"];

        return strConnection;

    }
    static void ViewProducts()
    {

        DbProviderFactory factory = SqlClientFactory.Instance;

        using DbConnection connection = factory.CreateConnection();

        if (connection == null)
        {

            Console.WriteLine($"Unable to create the connection object.");
            return;

        }

        connection.ConnectionString = GetConnectionString();
        connection.Open();

        DbCommand command = factory.CreateCommand();

        if (command == null)
        {
            Console.WriteLine($"Unable to create the command object.");
            return;
        }
        command.Connection = connection;
        command.CommandText = "Select * From productlist";
        using DbDataReader dataReader = command.ExecuteReader();
        Console.WriteLine("**** Produc List ****");
        while (dataReader.Read())
        {
            Console.WriteLine($"ProductID: {dataReader["ProductID"]}, " +
                $"ProductName: {dataReader["ProductName"]}.");
        }
    }
    static void Main(string[] args)
    {
        ViewProducts();
        Console.ReadLine();
    }
}