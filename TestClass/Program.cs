namespace Socket {
    public abstract class SocketBase {
        public static byte[] result = new byte[1024];
        public System.Net.IPAddress SocketAddress { set; get; }
        public int SocketPort { set; get; }

        public SocketBase(string setSocketIPAddress, int setSocketPort) {
            this.SocketAddress = System.Net.IPAddress.Parse(setSocketIPAddress);
            this.SocketPort = setSocketPort;
        }

        public abstract string toString();
    }

    public class Client : SocketBase {
        public string ClientName { set; get; }
        private static System.Net.Sockets.Socket ClientSocket = null;

        public Client(string setSocketIPAddress, int setSocketPort, string setClientName) : base(setSocketIPAddress, setSocketPort) { this.ClientName = setClientName; }
        public override string toString() { return ""; }

        public bool Connect() {
            ClientSocket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);

            try {
                ClientSocket.Connect(new System.Net.IPEndPoint(this.SocketAddress, this.SocketPort));
                System.Console.WriteLine("Handshaking [{0}] was {1}...", ClientSocket.LocalEndPoint.ToString(), ClientSocket.Connected ? "Succeeded" : "Failed");
            } catch (System.Exception) { ClientSocket = null; return false; } finally { }

            return true;
        }

        internal protected void ReceiveMessage() {
            System.Net.Sockets.Socket myClientSocket = ClientSocket;
            try {
                System.Threading.Thread recMess = new System.Threading.Thread(new System.Threading.ThreadStart(() => {
                    int receiveMessageLength = 0x00;
                    try {
                        do {
                            if (isConnect()) { receiveMessageLength = myClientSocket.Receive(result); } else { return; }
                            if (receiveMessageLength >= 0x00) { System.Console.WriteLine("Received Message: {0}", System.Text.Encoding.UTF8.GetString(result, 0, receiveMessageLength)); }
                            System.Threading.Thread.Sleep(100);
                        } while (true);
                    } catch (System.Exception) { } finally { }
                }));
                recMess.Start();

                do { System.Threading.Thread.Sleep(100); } while (!myClientSocket.Poll(10, System.Net.Sockets.SelectMode.SelectRead));
                System.Console.WriteLine("Socket END...");
                if (recMess.ThreadState == System.Threading.ThreadState.WaitSleepJoin) {
                    recMess.Interrupt();
                } else { recMess.Abort(); }

                System.Environment.Exit(0x00);
            } catch (System.ObjectDisposedException) { } finally { }
        }

        internal protected void SetClientName() {
            System.Console.WriteLine("Send Message to SET Name: {0}", this.ClientName);
            ClientSocket.Send(System.Text.Encoding.UTF8.GetBytes("SET NAME: " + this.ClientName));
        }

        internal protected bool SendMessage() {
            try {
                string msg = string.Empty;
                while ((msg = System.Console.ReadLine()) != "EOF") {
                    if (!isConnect()) { break; }

                    if (msg.Length >= 0x00) {
                        byte[] sMessage = System.Text.Encoding.UTF8.GetBytes(msg);
                        ClientSocket.Send(sMessage, sMessage.Length, 0);
                        System.Console.WriteLine("Send Message to [{0}]: {1}", ClientSocket.RemoteEndPoint.ToString(), msg);
                    } System.Threading.Thread.Sleep(100);
                } if (isConnect()) { ClientSocket.Send(System.Text.Encoding.UTF8.GetBytes("END OF THE SOCKET")); }
            } finally { ClientSocket.Shutdown(System.Net.Sockets.SocketShutdown.Both); ClientSocket.Close(); }
            return false;
        }

        internal protected bool isConnect() { return ClientSocket.Connected; }
    }

    public class Server : SocketBase {
        private bool commandAnalysis { set; get; }

        private static System.Collections.ArrayList ChatLog = new System.Collections.ArrayList();
        private static System.Net.Sockets.Socket ServerSocket = null;

        public Server(string setSocketIPAddress, int setSocketPort, string commandAnalysis) : base(setSocketIPAddress, setSocketPort) { if (commandAnalysis == "TRUE") { this.commandAnalysis = true; } }
        public override string toString() { return ""; }

        public void Listen() {
            ServerSocket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
            ServerSocket.Bind(new System.Net.IPEndPoint(this.SocketAddress, this.SocketPort));
            ServerSocket.Listen(5);
            System.Console.WriteLine("Listening [{0}] was Succeeded...", ServerSocket.LocalEndPoint.ToString());
            System.Threading.Thread myThread = new System.Threading.Thread(ListenClientConnect);
            myThread.Start();
        }

        private void ListenClientConnect() {
            do {
                System.Net.Sockets.Socket ClientSocket = ServerSocket.Accept();
                System.Threading.Thread receiveThread = new System.Threading.Thread(new System.Threading.ThreadStart(() => {
                    System.Net.Sockets.Socket myClientSocket = ClientSocket;
                    try {
                        System.Threading.Thread PrintMessage = new System.Threading.Thread(new System.Threading.ThreadStart(() => {
                            try {
                                System.Threading.Thread SendTimer = new System.Threading.Thread(() => {
                                    try {
                                        int currentPoint = 0x00;
                                        do {
                                            if (currentPoint < ChatLog.Count) {
                                                myClientSocket.Send(System.Text.Encoding.UTF8.GetBytes(string.Format("\t[{0}]", ChatLog[currentPoint++])));
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
                                    ClientName = ClientName.Substring("SET NAME: ".Length);
                                } else { ClientName = string.Empty; }
                                System.Threading.Thread.CurrentThread.Name = ClientName;
                                System.Console.WriteLine("Socket STARTED by {0}...", ClientName);

                                string msg = string.Empty;
                                do {
                                    if (System.Threading.Thread.CurrentThread.IsAlive) {
                                        if ((receiveMessageLength = myClientSocket.Receive(result)) > 0x00) {
                                            msg = System.Text.Encoding.UTF8.GetString(result, 0, receiveMessageLength);

                                            if (this.commandAnalysis) {
                                                switch (canAnalysis(msg)) {
                                                    case 0x01:
                                                        int strLength = msg.Length;
                                                        int startIndex = msg.IndexOf(": ") + ": ".Length;
                                                        if (startIndex >= 0x04) {
                                                            ClientName = msg.Substring(startIndex, strLength - startIndex);
                                                        } break;
                                                    case 0x02:
                                                        myClientSocket.Send(System.Text.Encoding.UTF8.GetBytes(ClientName)); break;
                                                    default:
                                                        lock (ChatLog) { ChatLog.Add(ClientName + ": " + msg); }
                                                        System.Console.WriteLine("Received Message by [{0} @ -#{1}]: {2}", myClientSocket.RemoteEndPoint.ToString(), ClientName, msg);
                                                        break;
                                                } System.Threading.Thread.Sleep(200);
                                            }
                                        }
                                    }
                                } while (!myClientSocket.Poll(10, System.Net.Sockets.SelectMode.SelectRead));
                                if (SendTimer.ThreadState == System.Threading.ThreadState.WaitSleepJoin) {
                                    SendTimer.Interrupt();
                                } else { SendTimer.Abort(); }
                            } catch (System.Net.Sockets.SocketException) {
                            } catch (System.Threading.ThreadInterruptedException) { } finally { };
                        }));
                        PrintMessage.Start();
                        do {
                            System.Threading.Thread.Sleep(500);
                        } while (!myClientSocket.Poll(10, System.Net.Sockets.SelectMode.SelectRead));
                        System.Console.WriteLine("Socket END by {0}...", PrintMessage.Name);
                        if (PrintMessage.ThreadState == System.Threading.ThreadState.WaitSleepJoin) {
                            PrintMessage.Interrupt();
                        } else { PrintMessage.Abort(); }
                    } finally { myClientSocket.Shutdown(System.Net.Sockets.SocketShutdown.Both); myClientSocket.Close(); }
                    // System.Exception e: System.Console.WriteLine("[Class 1]# {0}: {1}\n\t{2}", e.GetType(), e.Message, e.StackTrace);
                }));
                receiveThread.Start();
            } while (true);
        }

        private int canAnalysis(string msg) {
            if (msg.Length < 0x04) { return 0x00; }
            switch (msg.Substring(0, 4)) {
                case "SETN": return 0x01;
                case "GETN": return 0x02;
                default: return 0x00;
            }
        }
    }

    public class SocketProgram {
        public static void getProgramHelp() {
            System.Console.WriteLine("%0.exe -<Launcher Mode> -<Server IP>(:50740) [commandAnalysis/Name]");
            System.Console.WriteLine("");
            System.Console.WriteLine("For Example: ");
            System.Console.WriteLine("SocketChat.exe Server 127.0.0.1 [TRUE]");
            System.Console.WriteLine("SocketChat.exe Client 127.0.0.1 [My Name]");
            System.Console.ReadKey();
            System.Environment.Exit(0xFF);
        }

        public static void Main(string[] args) {
            string launcherMode = string.Empty;
            string setIPAddress = string.Empty;
            string ClientName = string.Empty;
            string CommandMode = string.Empty;
            int setListenPort = 50740;
            bool flag = true;

            try {
                if (args.Length != 2 && args.Length != 3) { throw new System.NotSupportedException("Error Args Length..."); } else if (args.Length == 3) {
                    launcherMode = args[0];
                    setIPAddress = args[1];
                    CommandMode = ClientName = args[2];
                } else { launcherMode = args[0]; setIPAddress = args[1]; ClientName = "[Client Name]"; CommandMode = "FALSE"; }
            } catch (System.Exception) { getProgramHelp(); }

            switch (launcherMode) {
                case "Client":
                    Client myClient = new Client(setIPAddress, setListenPort, ClientName);
                    try {
                        bool isConnected = false;
                        int ConnectTimes = 0x00;
                        do {
                            try { isConnected = myClient.Connect(); } catch (System.Exception) { } finally { }
                            if (isConnected) { break; } else if (++ConnectTimes > 0x03) { throw new System.NotSupportedException(); } else {
                                System.Console.WriteLine("Handshaking [{0}] was No Response...", myClient.SocketAddress.ToString());
                                System.Threading.Thread.Sleep(1000);
                            }
                        } while (!isConnected);
                    } catch (System.Exception) { myClient = null; break; } finally { }

                    try {
                        myClient.SetClientName();
                        System.Threading.Thread messageRec = new System.Threading.Thread(() => {
                            do {
                                if (myClient.isConnect()) {
                                    myClient.ReceiveMessage();
                                    System.Threading.Thread.Sleep(500);
                                } else { System.Threading.Thread.CurrentThread.Abort(); }
                            } while (flag);
                        });
                        messageRec.Start();
                        do { System.Threading.Thread.Sleep(100); } while (flag = myClient.SendMessage());
                    } catch (System.Exception) { } finally { myClient = null; }
                    break;

                case "Server":
                    Server myServer = new Server(setIPAddress, setListenPort, CommandMode);
                    myServer.Listen();
                    break;

                default:
                    getProgramHelp();
                    break;
            }
        }
    }
}
