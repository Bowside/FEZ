using System;
using System.IO.Ports;
using System.Text;
using Microsoft.SPOT;
using GHIElectronics.NETMF.FEZ;
using GHIElectronics.NETMF.Hardware;
using System.Threading;

namespace GPRS
{
    class GPRS
    {
        static SerialPort serialPort;

        const int bufferMax = 1024;
        static byte[] buffer = new Byte[bufferMax];
        public static string output = "";
        public GPRS(string portName = "COM1", int baudRate = 19200, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            serialPort.ReadTimeout = -1;
            serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
            serialPort.Open();
        }
        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            byte[] bufferData = new byte[20];
            serialPort.Read(bufferData, 0, 20);
            char[] charArray = System.Text.UTF8Encoding.UTF8.GetChars(bufferData);
            for (int i = 0; i < charArray.Length - 1; i++)
            {
                if (charArray[i].ToString() == "\r")
                {
                    //output += charArray[i];
                    Debug.Print(output);
                    output = "";
                }
                else
                {
                    output += charArray[i];
                }
            }

        }
        private void Print(string line)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            byte[] bytesToSend = encoder.GetBytes(line);
            serialPort.Write(bytesToSend, 0, bytesToSend.Length);
        }

        private void PrintLine(string line)
        {
            Print(line + "\r");
        }
        private void PrintEnd()
        {
            byte[] bytesToSend = new byte[1];
            bytesToSend[0] = 26;
            serialPort.Write(bytesToSend, 0, 1);
            Thread.Sleep(200);
        }
        public void SendSMS(string msisdn, string message)
        {
            PrintLine("AT+CMGF=1");
            Thread.Sleep(100);
            PrintLine("AT+CMGS=\"" + msisdn + "\"");
            Thread.Sleep(100);
            PrintLine(message);
            Thread.Sleep(100);
            PrintEnd();
            Thread.Sleep(100);
            Debug.Print("SMS Sent!");
            serialPort.Close();

        }
        public void placeCall(string msisdn)
        {

            PrintLine("ATD + " + msisdn + ";");
            Thread.Sleep(100);
            Debug.Print("Calling.....");
        }
    }
}
