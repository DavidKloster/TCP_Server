using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Extensions;

public class TcpServer
{
    private static byte[] data;
    private static NetworkStream networkStream;
    private static int received;
    private static TcpClient tcpClient;
    private static TcpListener tcpListener;

    public static void Main()
    {
        data = new byte[1024];
        var localAddress = IPAddress.Parse("127.0.0.1");
        tcpListener = new TcpListener(localAddress, 9050);
        tcpListener.Start();


        //Startscreen
        Console.WriteLine("######################################################################################################");
        Console.WriteLine("#----------------------------------------------------------------------------------------------------#");
        Console.WriteLine("#----######################-------###---###--###----###--###--########--########--########--###------#");
        Console.WriteLine("#----#-####---------------#-------#-#--#-#---#--#---#-#--#-#--#------#--#------#--#------#--#-#------#");
        Console.WriteLine("#----#-####---------------#-------#-#-#-#----#---#--#-#--#-#--#-######--#-######--#-######--#-#------#");
        Console.WriteLine("#----#--------------------#-------#-##-#-----#-#--#-#-#--#-#--#-#-------#-#-------#-#-------#-#------#");
        Console.WriteLine("#----#--------####--------#-------#---#------#-#----#-#--#-#--#-######--#-######--#-######--#-#------#");
        Console.WriteLine("#----#--------####--------#-------#---#------#-#-#--#-#--#-#--#------#--#------#--#-######--#-#------#");
        Console.WriteLine("#----#--------------------#-------#-##-#-----#-#--#-#-#--#-#--#-######--#-######--#-#-------#-#------#");
        Console.WriteLine("#----#---------------####-#-------#-#-#-#----#-#---##-#--#-#--#-#-------#-#-------#-######--#-#------#");
        Console.WriteLine("#----#---------------####-#-------#-#--#-#---#-#----#-#--#-#--#-#-------#-#-------#------#--#-#####--#");
        Console.WriteLine("#----######################-------###---##---###----###--###--###-------###-------########--#######--#");
        Console.WriteLine("#----------------------------------------------------------------------------------------------------#");
        Console.WriteLine("######################################################################################################");
        System.Console.ReadLine();
        Console.WriteLine("");
        Console.WriteLine("Waiting for players to connect!");
        System.Console.ReadLine();
        //EndStatscreen

        tcpClient = tcpListener.AcceptTcpClient();
        networkStream = tcpClient.GetStream();
        
        data = "Welcome to the Kniffel Server!".ToByteArray();
        networkStream.Write(data, 0, data.Length);

        var listener = new Thread(Listen);
        listener.Priority = ThreadPriority.Lowest;
        listener.Start();
    }

    static void Listen()
    {
        var arrOK = "OK".ToByteArray();

        while (true)
        {
            data = new byte[1024];
            received = networkStream.Read(data, 0, data.Length);
            if (received == 0)
                break;

            Console.WriteLine("Received from TCP Client: " + Encoding.ASCII.GetString(data, 0, received));
            networkStream.Write(arrOK, 0, arrOK.Length);
        }
        networkStream.Close();
        tcpClient.Close();
        tcpListener.Stop();
    }
}