using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;


public partial class IpInfoClass
{
    public static List<TcpConnectionInformation> conns = new List<TcpConnectionInformation>();
 
    public static string timeStamp;
    public static bool logging;
    public static bool gdprint;
    public static string dateformat;
 

    //constructor
    public IpInfoClass(bool l, bool c, string d)
    {
        logging = l;
        gdprint = c;
        dateformat = d;
    }


    public void startinformation()
	{
        DateTime timestamp = DateTime.Now;
        WriteLog(timestamp + " ipinformation-class");
        string[] localaddr = IP.GetLocalAddresses();
        WriteLog("Godot information from IP-class local addresses: ");
        foreach (string s in localaddr)
        {
            WriteLog(s);
        }

        WriteLog("\n");

        var localif = new Godot.Collections.Array<Godot.Collections.Dictionary>();
        localif = IP.GetLocalInterfaces();
        WriteLog("Godot information from IP-class local interfaces: ");
        for (int i = 0; i < localif.Count; i++)
        {
            WriteLog(localif[i].ToString());
        }

        WriteLog("\n");
        WriteLog("All NetworkInterfaces: ");
        NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface adapter in adapters.Where(a => a.OperationalStatus == OperationalStatus.Up))
        {
            WriteLog("Description: " + adapter.Description + "\n" +
                "Id: " + adapter.Id + "\n" +
                "IsReceiveOnly: " + adapter.IsReceiveOnly + "\n" +
                "Id: " + adapter.Id + "\n" +
                "Name: " + adapter.Name + "\n" +
                "NetworkInterfaceType: " + adapter.NetworkInterfaceType + "\n" +
                "OperationalStatus: " + adapter.OperationalStatus + "\n" +
                "Speed (bits per second): " + adapter.Speed + "\n" +
                "SupportsMulticast: " + adapter.SupportsMulticast);


            var ipv4Info = adapter.GetIPv4Statistics();
            WriteLog("OutputQueueLength: " + ipv4Info.OutputQueueLength);
            WriteLog("BytesReceived: " + ipv4Info.BytesReceived);
            WriteLog("BytesSent: " + ipv4Info.BytesSent);

            if (adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
            {
                WriteLog("Ethernet or WiFi Network - Speed (bits per seconde): " + adapter.Speed);
            }
            WriteLog("\n");
        }

        WriteLog("Active TCP connections:\n");
        ShowActiveTcpConnections();

        WriteLog("\nNew TCP connections:");
    }


    // https://learn.microsoft.com/en-us/dotnet/api/system.net.networkinformation.ipglobalproperties.getactivetcpconnections?view=net-7.0
    public void ShowActiveTcpConnections()
    {
        IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
        TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
        DateTime timestamp = DateTime.Now;
        foreach (TcpConnectionInformation c in connections)
        {
            WriteLog(timestamp.ToString() + " from " + c.LocalEndPoint.ToString() +
                " <---> to " + c.RemoteEndPoint.ToString() + " --- state: " + c.State.ToString());
            conns.Add(c);
        }
    }


    public void ChangedConnections()
    {
        bool found = false;
        //GD.Print("Active TCP Connections");
        IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
        TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
        DateTime timestamp = DateTime.Now;
        foreach (TcpConnectionInformation c in connections)
        {
            found = false; //search for known connections
            for (int i = 0; i < conns.Count; i++)
            {
                if (conns[i].LocalEndPoint.ToString() == c.LocalEndPoint.ToString() &&
                    conns[i].RemoteEndPoint.ToString() == c.RemoteEndPoint.ToString() &&
                    conns[i].State.ToString() == c.State.ToString())
                {
                    found = true;
                }
            }
            if (!found)
            {
                conns.Add(c);
                WriteLog(timestamp.ToString() + " from " + c.LocalEndPoint.ToString() +
                " <---> to " + c.RemoteEndPoint.ToString() + " --- state: " + c.State.ToString());
            }
        }
        for (int i = 0; i < conns.Count; i++)
        {
            found = false; //search for dead connections
            foreach (TcpConnectionInformation c in connections)
            {
                if (conns[i].LocalEndPoint.ToString() == c.LocalEndPoint.ToString() &&
                conns[i].RemoteEndPoint.ToString() == c.RemoteEndPoint.ToString() &&
                conns[i].State.ToString() == c.State.ToString())
                {
                    found = true;
                }
            }
            if (!found)
            {
                WriteLog(timestamp.ToString() + " removed connection from list" + " from " + conns[i].LocalEndPoint.ToString() +
                " <---> to " + conns[i].RemoteEndPoint.ToString());
                conns.Remove(conns[i]);
            }
        }
    }


    // https://stackoverflow.com/questions/20185015/how-to-write-log-file-in-c
    public void WriteLog(string strLog)
    {
        if (gdprint) GD.Print(strLog);
        if (!logging) return;

        StreamWriter log;
        FileStream fileStream = null;
        DirectoryInfo logDirInfo = null;
        FileInfo logFileInfo;

        string logFilePath = "./logs/";
        logFilePath = logFilePath + "Log-" + System.DateTime.Today.ToString(dateformat) + "." + "txt";
        logFileInfo = new FileInfo(logFilePath);
        logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
        if (!logDirInfo.Exists) logDirInfo.Create();
        if (!logFileInfo.Exists)
        {
            fileStream = logFileInfo.Create();
        }
        else
        {
            fileStream = new FileStream(logFilePath, FileMode.Append);
        }
        log = new StreamWriter(fileStream);
        log.WriteLine(strLog);
        log.Close();
    }

}
