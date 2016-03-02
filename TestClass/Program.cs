namespace Socket {
    public abstract class SocketBase {
        public static byte[] result = new byte[4096];
        public System.Net.IPAddress SocketAddress { set; get; }
        public int SocketPort { set; get; }

        public SocketBase(System.Net.IPAddress setSocketIPAddress, int setSocketPort) {
            this.SocketAddress = setSocketIPAddress;
            this.SocketPort = setSocketPort;
        }

        public abstract string toString();
    }

    public class Client : SocketBase {
        public string ClientName { set; get; }
        private static System.Net.Sockets.Socket ClientSocket = null;

        public Client(System.Net.IPAddress setSocketIPAddress, int setSocketPort, string setClientName) : base(setSocketIPAddress, setSocketPort) { this.ClientName = setClientName; }
        public override string toString() { return ""; }

        public void Connect(int reTryTimes = 0x00) {
            bool isConnected = false; int ConnectTimes = 0x00; reTryTimes = reTryTimes < 0x03 ? 0x03 : reTryTimes;
            ClientSocket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);

            do {
                if (isConnected = ConnectToServer()) { break; } else if (ConnectTimes >= 0x03) { ClientSocket = null; throw new System.NotSupportedException(); } else {
                    System.Console.WriteLine("Handshaking [{0}] - No Response...", this.SocketAddress.ToString());
                } if (++ConnectTimes < reTryTimes) { System.Threading.Thread.Sleep(1000); }
            } while (!isConnected);

            System.Threading.Thread watch = new System.Threading.Thread(new System.Threading.ThreadStart(Client.watch)); watch.Start();
        }

        private bool ConnectToServer() {
            try {
                ClientSocket.Connect(new System.Net.IPEndPoint(this.SocketAddress, this.SocketPort));
                System.Console.WriteLine("Handshaking [{0}] was {1}...", ClientSocket.LocalEndPoint.ToString(), ClientSocket.Connected ? "Succeeded" : "Failed");
            } catch (System.Net.Sockets.SocketException) { return false; } finally { }

            return true;
        }

        internal protected void ReceiveMessage() {
            System.Net.Sockets.Socket myClientSocket = ClientSocket;
            try {
                int receiveMessageLength = 0x00;
                try {
                    do {
                        if (isConnect()) { receiveMessageLength = myClientSocket.Receive(result); } else { return; }

                        if (receiveMessageLength >= 0x00) { System.Console.WriteLine("Received Message -#{0}", System.Text.Encoding.UTF8.GetString(result, 0, receiveMessageLength)); }
                        System.Threading.Thread.Sleep(100);
                    } while (true);
                } catch (System.Net.Sockets.SocketException) { } finally { }
            } catch (System.ObjectDisposedException) { } finally { }
        }

        internal protected bool SendMessage() {
            string msg = string.Empty;
            try {
                System.IO.Stream inputStream = System.Console.OpenStandardInput();
                System.Console.SetIn(new System.IO.StreamReader(inputStream, System.Text.Encoding.Default, true, 4096));

                while ((msg = System.Console.ReadLine()) != "EOF") {
                    if (!isConnect()) { break; }

                    if (msg.Length > 0x00) {
                        byte[] sMessage = System.Text.Encoding.UTF8.GetBytes(msg); ClientSocket.Send(sMessage, sMessage.Length, 0);
                        if (msg[0] == '#') { System.Console.WriteLine("Send Message to [{0}]: {1}", ClientSocket.RemoteEndPoint.ToString(), msg.Substring(1)); }
                    } System.Threading.Thread.Sleep(100);
                } if (isConnect()) { ClientSocket.Send(System.Text.Encoding.UTF8.GetBytes("END OF THE SESSION")); }
            } finally { ClientSocket.Shutdown(System.Net.Sockets.SocketShutdown.Both); ClientSocket.Close(); }
            return false;
        }

        private static void watch() {
            do { System.Threading.Thread.Sleep(300); } while (isConnect());
            System.Console.WriteLine("Socket END..."); System.Environment.Exit(0x00);
        }

        internal protected void SetClientName() {
            System.Console.WriteLine("Send Message to SET Name: {0}", this.ClientName);
            ClientSocket.Send(System.Text.Encoding.UTF8.GetBytes("SET NAME: " + this.ClientName));
        }

        internal protected static bool isConnect() { return ClientSocket.Connected; }
    }

    public class Server : SocketBase {
        private bool commandAnalysis { set; get; }

        private static System.Collections.ArrayList ChatLog = new System.Collections.ArrayList();
        private static System.Net.Sockets.Socket ServerSocket = null;

        public Server(System.Net.IPAddress setSocketIPAddress, int setSocketPort, string commandAnalysis) : base(setSocketIPAddress, setSocketPort) { if (commandAnalysis == "TRUE") { this.commandAnalysis = true; } }
        public override string toString() { return ""; }

        public void Listen() {
            ServerSocket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
            try {
                ServerSocket.Bind(new System.Net.IPEndPoint(this.SocketAddress, this.SocketPort)); ServerSocket.Listen(5);
                System.Console.WriteLine("Listening [{0}] was Succeeded...", ServerSocket.LocalEndPoint.ToString());
                System.Threading.Thread myThread = new System.Threading.Thread(ListenClientConnect); myThread.Start();
            } catch (System.Net.Sockets.SocketException) { System.Console.WriteLine("IP or Port was used..."); } finally { }
        }

        private void ListenClientConnect() {
            do {
                System.Net.Sockets.Socket ClientSocket = ServerSocket.Accept();
                System.Threading.Thread receiveThread = new System.Threading.Thread(new System.Threading.ThreadStart(() => {
                    string aliveClientName = string.Empty;
                    System.Net.Sockets.Socket myClientSocket = ClientSocket;
                    try {
                        System.Threading.Thread SendTimer = new System.Threading.Thread(() => {
                            try {
                                int currentPoint = 0x00;
                                do {
                                    if (currentPoint < ChatLog.Count) {
                                        myClientSocket.Send(System.Text.Encoding.UTF8.GetBytes(string.Format("{0}", ChatLog[currentPoint++])));
                                        System.Threading.Thread.Sleep(500);
                                    }
                                } while (!myClientSocket.Poll(10, System.Net.Sockets.SelectMode.SelectRead));
                            } catch (System.ObjectDisposedException) {
                            } catch (System.Threading.ThreadInterruptedException) { } finally { }
                        });
                        SendTimer.Start();

                        int receiveMessageLength = myClientSocket.Receive(result);
                        string ClientName = System.Text.Encoding.UTF8.GetString(result, 0, receiveMessageLength);
                        if (ClientName.IndexOf("SET NAME: ") >= 0x00) {
                            aliveClientName = ClientName = ClientName.Substring("SET NAME: ".Length);
                        } else { ClientName = string.Empty; }
                        System.Threading.Thread.CurrentThread.Name = ClientName;
                        System.Console.WriteLine("Socket STARTED by {0}...", ClientName);

                        string msg = string.Empty; int selectIndex = 0x00;
                        do {
                            if (System.Threading.Thread.CurrentThread.IsAlive) {
                                if ((receiveMessageLength = myClientSocket.Receive(result)) > 0x00) {
                                    msg = System.Text.Encoding.UTF8.GetString(result, 0, receiveMessageLength);

                                    if (this.commandAnalysis) { selectIndex = canAnalysis(msg); } else { selectIndex = 0x00; }
                                    switch (selectIndex) {
                                        case 0x01:
                                            int startIndex = msg.IndexOf(": ") + ": ".Length; ClientName = msg.Substring(startIndex, msg.Length - startIndex);
                                            lock (ChatLog) { ChatLog.Add("[" + aliveClientName + "] was Change Name to [" + ClientName + "]"); }
                                            aliveClientName = ClientName; break;
                                        case 0x02: myClientSocket.Send(System.Text.Encoding.UTF8.GetBytes(ClientName)); break;
                                        case 0x04: this.commandAnalysis = !this.commandAnalysis; break;
                                        case 0x08: myClientSocket.Shutdown(System.Net.Sockets.SocketShutdown.Both); myClientSocket.Close(); System.Environment.Exit(0); break;
                                        case 0x0F: myClientSocket.Send(System.Text.Encoding.UTF8.GetBytes(System.DateTime.UtcNow.ToLocalTime().ToString(new System.Globalization.CultureInfo("en-US")))); break;
                                        default:
                                            System.Console.WriteLine("Received Message by [{0} @ -#{1}]: {2}", myClientSocket.RemoteEndPoint.ToString(), ClientName, msg);
                                            lock (ChatLog) { ChatLog.Add(ClientName + ": " + msg); } break;
                                    } System.Threading.Thread.Sleep(200);
                                }
                            }
                        } while (!myClientSocket.Poll(10, System.Net.Sockets.SelectMode.SelectRead));
                        if (SendTimer.ThreadState == System.Threading.ThreadState.WaitSleepJoin) {
                            SendTimer.Interrupt();
                        } else { SendTimer.Abort(); }
                    } catch (System.Net.Sockets.SocketException) {
                    } catch (System.Threading.ThreadInterruptedException) { } finally { };
                    do {
                        System.Threading.Thread.Sleep(500);
                    } while (!myClientSocket.Poll(10, System.Net.Sockets.SelectMode.SelectRead));
                    System.Console.WriteLine("Socket END by [{0}/{1}]...", System.Threading.Thread.CurrentThread.Name, aliveClientName);
                    myClientSocket.Shutdown(System.Net.Sockets.SocketShutdown.Both); myClientSocket.Close();
                    // System.Exception e: System.Console.WriteLine("[Class 1]# {0}: {1}\n\t{2}", e.GetType(), e.Message, e.StackTrace);
                }));
                receiveThread.Start();
            } while (true);
        }

        private int canAnalysis(string msg) {
            if (msg.Length < 0x04) { return 0x00; }

            switch (msg.Substring(0, 4)) {
                case "SETN": if (msg.IndexOf(": ") >= 0x00) { return 0x01; } else { return 0x00; };
                case "GETN": return 0x02;
                case "DISA": return 0x04;
                case "SHUT": return 0x08;
                case "TIME": return 0x0F;
                default: return 0x00;
            }
        }
    }

    public class SocketProgram {
        public static void getProgramHelp() {
            System.Console.WriteLine("%0.exe -<Launcher Mode> -<Server IP>(:50740) [commandAnalysis/Name]\n");
            System.Console.WriteLine("For Example: ");
            System.Console.WriteLine("SocketChat.exe Server 127.0.0.1 [TRUE]");
            System.Console.WriteLine("SocketChat.exe Client 127.0.0.1 [My Name]");
        }

        public static void Main(string[] args) {
            string launcherMode = string.Empty; System.Net.IPAddress setIPAddress = System.Net.IPAddress.None;
            string ClientName = string.Empty; string CommandMode = string.Empty; int setListenPort = 50740;

            try {
                if (args.Length != 2 && args.Length != 3) { throw new System.NotSupportedException("Error Args Length..."); }
                else if (args.Length == 3) { CommandMode = ClientName = args[2]; } else { ClientName = "[Client Name]"; CommandMode = "FALSE"; }
                if (System.Net.IPAddress.TryParse(args[1], out setIPAddress)) { launcherMode = args[0]; System.Net.IPAddress.Parse(args[1]); } else { getProgramHelp(); }
            } catch (System.NotSupportedException) { getProgramHelp(); } finally { }

            switch (launcherMode) {
                case "Client":
                    Client myClient = new Client(setIPAddress, setListenPort, ClientName);
                    try { myClient.Connect(); } catch (System.NotSupportedException) { System.Console.WriteLine("Bad Server Address"); break; } finally { }

                    System.Threading.Thread messageRec = new System.Threading.Thread(() => { myClient.ReceiveMessage(); }); messageRec.Start();
                    myClient.SetClientName(); myClient.SendMessage();
                    break;

                case "Server":
                    Server myServer = new Server(setIPAddress, setListenPort, CommandMode); myServer.Listen();
                    break;

                default:
                    break;
            }
            System.Console.ReadKey();
        }
    }
}
