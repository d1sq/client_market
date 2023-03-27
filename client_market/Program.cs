using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class SupermarketClient
{
    private readonly string host;
    private readonly int port;

    public SupermarketClient(string host, int port)
    {
        this.host = host;
        this.port = port;
    }

    public void Request(string customer)
    {
        TcpClient client = new TcpClient(host, port);
        NetworkStream stream = client.GetStream();

        try
        {
            byte[] buffer = Encoding.UTF8.GetBytes(customer);
            stream.Write(buffer, 0, buffer.Length);

            //buffer = new byte[1024];
            //int bytesRead = stream.Read(buffer, 0, buffer.Length);
            //string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            //Console.WriteLine("Received response: " + response);
        }
        catch (IOException e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
        finally
        {
            client.Close();
        }
    }

    static void Main(string[] args)
    {
        // Создание клиента
        string host = "localhost";
        int port = 8888;
        SupermarketClient client = new SupermarketClient(host, port);

        // Добавление покупателей в очередь
        Random random = new Random();

        for (int i = 0; i < 5; i++)
        {
            new Thread(() =>
            {
                while (true)
                {
                    string customerName = "Customer" + random.Next(1, 101);
                    client.Request(customerName);
                    Console.WriteLine("Adding customer: " + customerName);
                    Thread.Sleep(100);
                }
            }).Start();
        }

        Console.ReadKey();
    }

}
