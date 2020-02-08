using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System;
using System.Globalization;

public class ClientControl : MonoBehaviour
{
    public Client client;
    public InputField inputField;

    public static IPEndPoint CreateIPEndPoint(string endPoint)
    {
        string[] ep = endPoint.Split(':');
        if (ep.Length < 2) throw new FormatException("Invalid endpoint format");
        IPAddress ip;
        if (ep.Length > 2)
        {
            if (!IPAddress.TryParse(string.Join(":", ep, 0, ep.Length - 1), out ip))
            {
                throw new FormatException("Invalid ip-adress");
            }
        }
        else
        {
            if (!IPAddress.TryParse(ep[0], out ip))
            {
                throw new FormatException("Invalid ip-adress");
            }
        }
        int port;
        if (!int.TryParse(ep[ep.Length - 1], NumberStyles.None, NumberFormatInfo.CurrentInfo, out port))
        {
            throw new FormatException("Invalid port");
        }
        return new IPEndPoint(ip, port);
    }

    public void PasteFromClipboard()
    {
        TextEditor textEditor = new TextEditor();
        textEditor.Paste();
        inputField.text = textEditor.text;
    }

    public void Connect()
    {
        try
        {
            IPEndPoint ip = CreateIPEndPoint(inputField.text);
            client.serverAddress = ip.Address.ToString();
            client.serverPort = ip.Port;
            client.Connect();
        } catch
        {

        }
        
    }
}
