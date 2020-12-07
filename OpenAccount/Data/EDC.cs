using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;
using System.Threading.Tasks;

namespace OpenAccount.Data
{
    public class EDC
    {
        public SerialPort serialPort;
        string dataRespond = string.Empty;
        private string _statusCode = string.Empty;
        private string _respondCode = string.Empty;
        private string _ecr = string.Empty;
        private string _approvalCode = string.Empty;

        public string statusCode => _statusCode;
        public string respondCode => _respondCode;
        public string ecr => _ecr;
        public string approvalCode => _approvalCode;
        string strTransType = string.Empty;
        string dataSplit = string.Empty;
        public AutoResetEvent mre = new AutoResetEvent(false);

        public string EDCStatus = string.Empty;

        public int intTry = 0;

        public void Clear()
        {
            _statusCode = string.Empty;
            _respondCode = string.Empty;
            _approvalCode = string.Empty;
            dataRespond = string.Empty;
            strTransType = string.Empty;
            dataSplit = string.Empty;
            intTry = 0;
        }

        public static string StringToByteString(string p_str)
        {
            string result = string.Empty;

            try
            {
                string byte_ = string.Empty;

                int j = 0;
                int byte_count = 0;

                for (int i = 0; i < p_str.Length; i++)
                {
                    j++;
                    byte_ += p_str[i];

                    if (j >= 2)
                    {
                        byte_count++;
                        if (byte_count > 1) result += ", ";

                        result += "0x" + byte_;

                        if ((byte_count % 8) == 0)
                        {
                            result += System.Environment.NewLine;
                        }

                        byte_ = string.Empty;
                        j = 0;
                    }
                }
            }

            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        public byte[] StringToByteArray(string p_str)
        {
            byte[] result = null;

            try
            {
                if (!p_str.Contains(","))
                {
                    p_str = StringToByteString(p_str);
                }

                p_str = p_str.Replace(Environment.NewLine, "");
                p_str = p_str.Replace("0x", "");
                p_str = p_str.Replace(" ", "");

                string[] x_bytes = p_str.Split(',');

                result = new byte[x_bytes.Length];
                int i = 0;
                foreach (string x_byte in x_bytes)
                {
                    result[i] = byte.Parse(x_byte, System.Globalization.NumberStyles.HexNumber);
                    i++;
                }
            }
            catch
            {
                result = null;
            }

            return result;
        }

        public byte[] LRC(byte[] p_data)
        {
            try
            {
                int length = p_data.Length;

                byte x = p_data[1];

                for (int i = 2; i <= length - 2; i++)
                {
                    x ^= p_data[i];
                }
                p_data[length - 1] = x;
            }
            catch { }

            return p_data;
        }
        public void SendCommand(SerialPort port, string ecrmsg)
        {
            string stx = "02";
            string ecr = "424E49"; // BNI;
            string ecr_messsage = string.Empty;
            string etx = "03";
            string lrc = "00";

            dataRespond = string.Empty;

            serialPort.Close();
            string request_BankFiller = string.Empty;

            request_BankFiller = HexaBankFiller("");

            ecr_messsage = ecrmsg
                            + request_BankFiller;

            string data = stx
                + ecr
                + ecr_messsage
                + etx
                + lrc;

            byte[] data2 = StringToByteArray(data);
            byte[] data2_with_lrc = LRC(data2);

            serialPort.PortName = port.PortName;
            serialPort.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);

            Console.WriteLine("1");
            serialPort.Open();

            serialPort.Write(data2_with_lrc, 0, data2_with_lrc.Length);
            Console.WriteLine("2");

            //serialPort.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
        }

        public string HexaBankFiller(string data_filler)
        {
            return "202020202020202020202020202020202020202020202020202020202020202020202020202020202020202020202020";
        }

        public void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            dataSplit = string.Empty;
            _respondCode = string.Empty;
            _statusCode = string.Empty;
            _approvalCode = string.Empty;
            Console.WriteLine(dataRespond);
            if (serialPort.IsOpen)
            {
                string dataCurrent = serialPort.ReadExisting();
                dataRespond += dataCurrent;
                Console.WriteLine("Data Current: " + dataCurrent);
                Console.WriteLine("Data Respond: " + dataRespond);
                if (dataRespond.Contains("\x06"))
                {
                    if (dataRespond.Length > 10)
                    {
                        serialPort.Close();
                        EDCStatus = "EDC ACK";
                        Console.WriteLine(EDCStatus);
                        dataSplit = dataRespond.Substring(dataRespond.IndexOf("BNI"));
                        Console.WriteLine("Data Split: " + dataSplit);
                        _statusCode = dataSplit.Substring(4, 2);
                        Console.WriteLine("Status : " + _statusCode);
                    }
                }
                else if (dataRespond.Contains("\x15"))
                {
                    serialPort.Close();
                    EDCStatus = "EDC NAK";
                    Console.WriteLine(EDCStatus);

                }
            }
        }

        public void port_DataReceivedLoop(object sender, SerialDataReceivedEventArgs e)
        {
            dataSplit = string.Empty;
            _respondCode = string.Empty;
            _statusCode = string.Empty;
            _approvalCode = string.Empty;
            bool exitLoop = false;
            Console.WriteLine(dataRespond);
            if (serialPort.IsOpen)
            {
                
                    string dataCurrent = serialPort.ReadExisting();
                    dataRespond += dataCurrent;
                    Console.WriteLine("Data Current: " + dataCurrent);
                    Console.WriteLine("Data Respond: " + dataRespond);
                    if (dataRespond.Contains("\x06"))
                    {
                        if (dataRespond.Length > 10)
                        {
                            //serialPort.Close();
                            Console.WriteLine("EDC ACK");
                            dataSplit = dataRespond.Substring(dataRespond.IndexOf("BNI"));
                            Console.WriteLine("Data Split: " + dataSplit);
                            _statusCode = dataSplit.Substring(4, 2);
                            Console.WriteLine("Status : " + _statusCode);
                        }
                    }
                    else if (dataRespond.Contains("\x15"))
                    {
                        serialPort.Close();
                        Console.WriteLine("EDC NAK");
                    }
            }
        }

        public void SendCommandLoop(SerialPort port, string ecrmsg)
        {
            _statusCode = string.Empty;
            string stx = "02";
            string ecr = "424E49"; // BNI;
            string ecr_messsage = string.Empty;
            string etx = "03";
            string lrc = "53";

            dataRespond = string.Empty;

            serialPort.Close();

            string request_BankFiller = string.Empty;

            request_BankFiller = HexaBankFiller("");

            ecr_messsage = ecrmsg
                + request_BankFiller;

            string data = stx
                + ecr
                + ecr_messsage
                + etx
                + lrc;

            byte[] data2 = StringToByteArray(data);
            byte[] data2_with_lrc = LRC(data2);
            //data2[56] = 0x77;
            serialPort.PortName = port.PortName;
            serialPort.BaudRate = 115200;
            //serialPort.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);

            Console.WriteLine("1");
            try
            {
                serialPort.Open();
            }
            catch
            {
                EDCStatus = "EDC NAK";
                return;
            }

            serialPort.Write(data2_with_lrc, 0, data2_with_lrc.Length);
            Console.WriteLine("2");

            //serialPort.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);

            dataSplit = string.Empty;
            _respondCode = string.Empty;
            _statusCode = string.Empty;
            _approvalCode = string.Empty;
            Console.WriteLine(dataRespond);
            if (serialPort.IsOpen)
            {
                string dataCurrent = serialPort.ReadExisting();
                dataRespond += dataCurrent;
                Console.WriteLine("Data Current: " + dataCurrent);
                Console.WriteLine("Data Respond: " + dataRespond);
                if (dataRespond.Contains("\x06"))
                {
                    if (dataRespond.Length > 10)
                    {
                        serialPort.Close();
                        EDCStatus = "EDC ACK";
                        Console.WriteLine(EDCStatus);
                        dataSplit = dataRespond.Substring(dataRespond.IndexOf("BNI"));
                        Console.WriteLine("Data Split: " + dataSplit);
                        _statusCode = dataSplit.Substring(4, 2);
                        Console.WriteLine("Status : " + _statusCode);
                    }
                }
                else if (dataRespond.Contains("\x15"))
                {
                    serialPort.Close();
                    EDCStatus = "EDC NAK";
                    Console.WriteLine(EDCStatus);

                }
            }
        }
    }
}
