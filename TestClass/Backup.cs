//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace TestClass {
//    class Program {
//        public static void Pause(String myStr) {
//            Console.Write(myStr + "\n\nPress any key to continue . . . ");
//            Console.ReadKey(true);
//        }

//        public delegate String sayInfoEventHandler(String myName);

//        public class Person {
//            public String Name { set; get; }
//            public Person (String Name) {
//                this.Name = Name;
//            }

//            public static void Blank(String myName) { }

//            public virtual String Welcome(String myName) { return String.Format("My Name is: {0}, Person Self-introduction: {1}.", this.Name, myName); } 
//        }

//        public class Student : Person {
//            public Student(String Name) : base(Name) { }
//            public override String Welcome(String myName) { return String.Format("My Name is: {0}, Student Self-introduction: {1}.", this.Name, myName); }
//        }

//        public class Teacher : Person {
//            public Teacher(String Name) : base(Name) { }
//            public override String Welcome(String myName) { return String.Format("My Name is: {0}, Teacher Self-introduction: {1}.", this.Name, myName); }
//        }

//        static void Main(string[] args) {
//            Student student = new Student("Tianhao Wu_S");
//            Teacher teacher = new Teacher("Tianhao Wu_T");

//            sayInfoEventHandler myDelegate = (String str) => { Console.Write(""); return str; };
//            myDelegate += student.Welcome;
//            myDelegate += teacher.Welcome;

//            String myStr = myDelegate("Delegat");

//            Pause(myStr);
//            return;
//        }
//    }
//}


//// A set of classes for handling a bookstore:
//namespace Bookstore {
//    using System.Collections;

//    // Describes a book in the book list:
//    public struct Book {
//        public string Title { set; get; }   // Title of the book.
//        public string Author { set; get; }  // Author of the book.
//        public decimal Price { set; get; }  // Price of the book.
//        public bool Paperback { set; get; } // Is it paperback?

//        public Book(string title, string author, decimal price, bool paperBack) {
//            Title = title;
//            Author = author;
//            Price = price;
//            Paperback = paperBack;
//        }
//    }

//    // Declare a delegate type for processing a book:
//    public delegate void ProcessBookDelegate(Book book);

//    // Maintains a book database.
//    public class BookDB {
//        // List of all books in the database:
//        ArrayList list = new ArrayList();

//        // Add a book to the database:
//        public void AddBook(string title, string author, decimal price, bool paperBack) {
//            list.Add(new Book(title, author, price, paperBack));
//        }

//        // Get a book from the database:
//        public Book GetBook(int index) {
//            Book tempBookInfo = (Book)list[index];
//            return tempBookInfo;
//        }

//        // Call a passed-in delegate on each paperback book to process it: 
//        public void ProcessPaperbackBooks(ProcessBookDelegate processBook, bool mode = false) {
//            foreach (Book b in list) { if (b.Paperback) { if (mode) { System.Console.Write("-#"); } processBook(b); } }
//        }
//    }
//}


//// Using the Bookstore classes:
//namespace BookTestClient {
//    using Bookstore;

//    // Class to total and average prices of books:
//    class PriceTotaller {
//        int countBooks = 0;
//        decimal priceBooks = 0.0m;

//        internal void AddBookToTotal(Book book) {
//            countBooks += 1; priceBooks += book.Price;
//        }

//        internal decimal AveragePrice() { return priceBooks / countBooks; }
//    }

//    // Class to test the book database:
//    class TestBookDB {
//        // Print the title of the book.
//        static void PrintBookInfo(Book b) { System.Console.WriteLine("\tTitle: {0}\n\t\tPaperback: {1}\tAuthor: {2}", b.Title, b.Paperback ? "FALSE": "TRUE", b.Author); }
//        static void Pause() {
//            System.Console.Write("\nPress any key to continue..."); System.Console.ReadKey(true);
//        }

//        // Execution starts here.
//        static void Main() {
//            BookDB bookDB = new BookDB();

//            // Initialize the database with some books:
//            AddBooks(bookDB);

//            // Print all the titles of paperbacks:
//            System.Console.WriteLine("Paperback Book Titles:");

//            // Create a new delegate object associated with the static 
//            // method Test.PrintTitle:
//            bookDB.ProcessPaperbackBooks(PrintBookInfo, true);

//            // Get the average price of a paperback by using
//            // a PriceTotaller object:
//            PriceTotaller totaller = new PriceTotaller();

//            // Create a new delegate object associated with the nonstatic 
//            // method AddBookToTotal on the object totaller:
//            bookDB.ProcessPaperbackBooks(totaller.AddBookToTotal);

//            System.Console.WriteLine("Average Paperback Book Price: ${0:#.##}", totaller.AveragePrice());

//            Pause();
//        }

//        // Initialize the book database with some test books:
//        static void AddBooks(BookDB bookDB) {
//            bookDB.AddBook("The C Programming Language", "Brian W. Kernighan and Dennis M. Ritchie", 19.95m, true);
//            bookDB.AddBook("The Unicode Standard 2.0", "The Unicode Consortium", 39.95m, true);
//            bookDB.AddBook("The MS-DOS Encyclopedia", "Ray Duncan", 129.95m, false);
//            bookDB.AddBook("Dogbert's Clues for the Clueless", "Scott Adams", 12.00m, true);
//        }
//    }
//}

//using System;
//namespace Programming {
//    public delegate void PrintEventDelegate(Object sender, EventSample.PrintEventArgs e);
//    public class EventSample {
//        // public event PrintEventDelegate PrintComplete;

//        public static void Pause(String myStr = "") { Console.Write(myStr + "\nPress any key to continue... "); Console.ReadKey(true); }

//        public static void makePrinter(int indexID = 0x00) {
//            Printer printer = new LaserPrinter(indexID);
//            printer.Subscribe();
//            printer.onPrint(new PrintEventArgs("Ready"));
//            printer.onPrint(new PrintEventArgs("Printing"));
//            System.Console.WriteLine(printer);
//            printer.onPrint(new PrintEventArgs("Completed"));
//        }

//        public static int Main() {
//            try { makePrinter(0x00); } catch (NotImplementedException) { Pause(""); }

//            return 0x00;
//        }

//        public class PrintEventArgs : EventArgs {
//            public String PrintState { get; set; }
//            public PrintEventArgs (String myStr) { this.PrintState = myStr; }
//        }

//        public interface PrinterMachine {
//            void onPrint(PrintEventArgs eventContent);
//            void Subscribe();
//            void UnSubscribe();
//            void Printer_PrintComplete(Object sender, PrintEventArgs e);
//        }

//        public class Printer : PrinterMachine {
//            internal protected int indexID { get; set; }
//            internal protected PrintEventArgs PrintState { get; set; }
//            internal protected PrintEventDelegate TestEventHandlers;

//            public Printer(int indexID = 0x00) { this.indexID = indexID; PrintState = new PrintEventArgs(""); }

//            private event PrintEventDelegate PrintComplete {
//                add { lock (TestEventHandlers) { TestEventHandlers += value; } }
//                remove { lock (TestEventHandlers) { TestEventHandlers -= value; } }
//            }

//            public virtual void onPrint(PrintEventArgs eventContent) {
//                PrintEventDelegate temp = TestEventHandlers;
//                if (temp != null) { temp(this, eventContent); }
//            }

//            public void Subscribe() { TestEventHandlers += Printer_PrintComplete; }
//            public void UnSubscribe() { TestEventHandlers -= Printer_PrintComplete; }

//            public override string ToString() {
//                return this.GetType() + " : index -#" + this.indexID;
//            }

//            public virtual void Printer_PrintComplete(Object sender, PrintEventArgs e) {
//                Console.WriteLine("Print State: " + e.PrintState + ", Send by: " + sender.GetType() + "[" + e.GetHashCode() + "@" + e.GetType() + "]");
//                if (e.PrintState == "Completed") { throw new NotImplementedException(); }
//            }
//        }

//        public class LaserPrinter : Printer {
//            internal protected String Technology { get; }
//            public LaserPrinter(InkjetPrinter v) : base(v.indexID) { this.Technology = v.Technology; }
//            public LaserPrinter(int indexID = 0x00, String Technology = "Laser") : base(indexID) { this.Technology = Technology; }

//            public override string ToString() {
//                return base.ToString() + ", Sataus: " + this.PrintState.PrintState;
//            }

//            public override void onPrint(PrintEventArgs eventContent) {
//                PrintState = eventContent;

//                PrintEventDelegate temp = TestEventHandlers;
//                if (temp != null) { temp(this, eventContent); }
//            }

//            public override void Printer_PrintComplete(object sender, PrintEventArgs e) {
//                Console.WriteLine(this.Technology + " Print(" + this.indexID + ") State: " + this.PrintState + ", Send by: " + sender.GetType() + "[" + e.GetHashCode() + "@" + e.GetType() + "]");
//                if (this.PrintState.PrintState == "Completed") { throw new NotImplementedException(); }
//            }

//            public static explicit operator LaserPrinter(InkjetPrinter v) {
//                return new LaserPrinter(v);
//            }
//        }

//        public class InkjetPrinter : Printer {
//            internal protected String Technology { get; }
//            public InkjetPrinter(LaserPrinter v) : base(v.indexID) { this.Technology = v.Technology; }
//            public InkjetPrinter(int indexID = 0x00, String Technology = "Inkjet") : base(indexID) { this.Technology = Technology; }

//            public override string ToString() {
//                return base.ToString() + ", Sataus: " + this.PrintState.PrintState;
//            }

//            public override void onPrint(PrintEventArgs eventContent) {
//                PrintState = eventContent;

//                PrintEventDelegate temp = TestEventHandlers;
//                if (temp != null) { temp(this, eventContent); }
//            }

//            public override void Printer_PrintComplete(object sender, PrintEventArgs e) {
//                Console.WriteLine(this.Technology + " Print(" + this.indexID + ") State: " + this.PrintState + ", Send by: " + sender.GetType() + "[" + e.GetHashCode() + "@" + e.GetType() + "]");
//                if (this.PrintState.PrintState == "Completed") { throw new NotImplementedException(); }
//            }

//            public static explicit operator InkjetPrinter(LaserPrinter v) {
//                return new InkjetPrinter(v);
//            }
//        }
//    }
//}

//namespace m {
//    public class myException : System.Exception {
//        internal protected bool flag { get; set; }

//        public myException(bool flag = false) { this.flag = flag; }
//        public myException(string message, bool flag = false) : base(message) { this.flag = flag; }
//        public myException(string message, System.Exception InnerException, bool flag = false) : base(message, InnerException) { this.flag = flag; }
//    }

//    public class my {
//        public delegate string go(int x);
//        public static string my1(int x) {
//            int threadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
//            System.Console.Write("[" + threadID.ToString() + "]delegate out>>> " + x.ToString());
//            System.Threading.Thread.Sleep(2000);
//            return string.Empty;
//        }
//        public static string my2(int x) { System.Threading.Thread.Sleep(2000); return x.ToString(); }

//        private static void CallbackHandler(System.IAsyncResult ar) { System.Console.WriteLine(" ***END"); }
//        public static string mymy(go a, int x) {
//            if (x == 0x80) {
//                throw new myException("Bad Message", new System.ArgumentOutOfRangeException(), true);
//            } else {
//                System.IAsyncResult result = a.BeginInvoke(x, new System.AsyncCallback(CallbackHandler), null);
//                do {
//                    System.Threading.Thread.Sleep(200);
//                    System.Console.Write(".");
//                } while (!result.IsCompleted);
//                a.EndInvoke(result);
//            }
//            return string.Empty;
//        }

//        public enum Mouse {
//            Left = 0x01,
//            Midd = 0x02,
//            Righ = 0x04
//        };

//        public static int Main() {
//            string ret = string.Empty;
//            int[] temp = new int[] { 0x80, 0xFF, 0x11, 0x22, 0x33, 0x00, 0x80, 0x08 };

//            go myGo = new go(my1);
//            foreach (int tempInt in temp) {
//                if ( tempInt != 0x00 ) {
//                    System.Exception my = null;
//                    try { mymy(myGo, tempInt); } catch (myException e) {
//                        my = e; System.Console.Write("\t" + e.Message + " -#" + e.flag + "@" + e.GetType() + "<<<");
//                    } catch (System.Exception e) {
//                        my = e; System.Console.Write(e.Message + " -# Null@" + e.GetType() + "<<<");
//                    } finally { if (my != null) { System.Console.WriteLine("Exception Catched..."); System.Threading.Thread.Sleep(500); } }
//                }
//            } System.Console.WriteLine(ret);

//            int control = (int)(Mouse.Left | Mouse.Righ);
//            string res = string.Empty;
//            if (control == (int)Mouse.Left) {
//                res = "Mouse.Left";
//            } else if (control == (int)Mouse.Midd) {
//                res = "Mouse.Midd";
//            } else if (control == (int)Mouse.Righ) {
//                res = "Mouse.Righ";
//            } else if (control == (int)(Mouse.Left | Mouse.Midd)) {
//                res = "Mouse.Left + Mouse.Midd";
//            } else if (control == (int)(Mouse.Left | Mouse.Righ)) {
//                res = "Mouse.Left + Mouse.Righ";
//            } else if (control == (int)(Mouse.Midd | Mouse.Righ)) {
//                res = "Mouse.Midd + Mouse.Righ";
//            } else if (control == (int)(Mouse.Left | Mouse.Midd | Mouse.Righ)) {
//                res = "Mouse.Left + Mouse.Midd + Mouse.Righ";
//            } else { res = "BadMouseInput"; }
//            System.Console.Write(res);
//            System.Console.ReadKey();

//            return 0;
//        }
//    }
//}

//namespace thread {
//    using threading = System.Threading;
//    using thread = System.Threading.Thread;
//    public class threadTest {
//        public class Studio {
//            public int count { get; set; }
//            public Studio(int countINT = 0x00) { this.count = countINT; }
//        }

//        static thread Mission_1, Mission_2;
//        static Studio obj = new Studio(0x00);

//        public static void output(object data) {
//            for (int loop = 0x00; loop <= 0x0F; ++ loop) {
//                lock (obj) {
//                    try {
//                        System.Console.WriteLine("\tThread Name: {0}, Thread ID: {1}, Output -#{2}, Prop -#{3}", thread.CurrentThread.Name, thread.CurrentThread.ManagedThreadId, loop, ++obj.count);
//                    } catch (System.Exception) { } finally { }
//                }
//                if (loop == 0x08) {
//                    if (thread.CurrentThread.Name == "Mission_1" && Mission_2.ThreadState != threading.ThreadState.Unstarted) { Mission_2.Join(); }
//                } else if (loop == 0x0B) {
//                    if (thread.CurrentThread.Name == "Mission_2") { thread.CurrentThread.Abort(); }
//                }
//                thread.Sleep(1000);
//            }
//        }

//        public static void Main() {
//            Mission_1 = new thread(new threading.ParameterizedThreadStart(output));
//            Mission_2 = new thread(new threading.ParameterizedThreadStart(output));

//            Mission_1.Start(null); Mission_1.Name = "Mission_1"; Mission_1.Priority = threading.ThreadPriority.Lowest;
//            Mission_2.Start(null); Mission_2.Name = "Mission_2"; Mission_2.Priority = threading.ThreadPriority.BelowNormal;

//            for (int loop = 0x00; loop <= 0x0F; ++loop) {
//                lock (obj) {
//                    try {
//                        System.Console.WriteLine("Thread Name: {0}, Thread ID: {1}, Output -#{2}, Prop -#{3}", thread.CurrentThread.Name, thread.CurrentThread.ManagedThreadId, loop, ++obj.count);
//                    } catch (System.Exception) { } finally { }
//                }
//                thread.Sleep(1000);
//            }
//        }
//    }
//}

//using System.Linq;

//namespace file {
//    public class fileList {
//        public static void Main() {
//            string dirPath = @"F:\Download\";
//            try {
//                string[] lengthList = new string[] { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB", "BB" };
//                System.Collections.Generic.IEnumerable<string> allFileCollections = from file in System.IO.Directory.EnumerateFiles(dirPath, "*.*") select file;

//                foreach (string file in allFileCollections) {
//                    string tempName = new System.IO.FileInfo(file).Name;
//                    int tempNameLength = System.Text.Encoding.GetEncoding("GB2312").GetByteCount(tempName) / 0x08;
//                    for (int loop = 0x0C - tempNameLength; loop >= 0x00; --loop) { tempName += "\t"; }

//                    double fileLengthMui = 0x00; double fileLengthMax = 1024;
//                    double fileLength = new System.IO.FileInfo(file).Length;
//                    while (fileLength >= fileLengthMax) {
//                        fileLengthMax = System.Math.Pow(fileLengthMax, ++fileLengthMui);
//                    } fileLengthMax = 1024; fileLengthMui -= 0x01;
//                    double tempLength = System.Math.Round(fileLength / System.Math.Pow(fileLengthMax, fileLengthMui), 2);

//                    System.Console.WriteLine(tempName + " -#Size: " + (tempLength + lengthList[(int)fileLengthMui]).PadLeft(10));
//                }
//                System.Console.ReadKey();
//            } catch (System.Exception) { } finally { }
//        }
//    }
//}

//namespace tcp {
//    public abstract class TCPBase {
//        public string setIPAddress { set; get; }
//        public int setListenPort { set; get; }

//        public TCPBase(string setIPAddress, int setListenPort) {
//            this.setIPAddress = setIPAddress;
//            this.setListenPort = setListenPort;
//        }

//        public abstract string toString();
//    }

//    public class Client : TCPBase {
//        public string ClientName { set; get; }
//        public Client(string setIPAddress, int setListenPort, string ClientName) : base(setIPAddress, setListenPort) { this.ClientName = ClientName; }
//        public override string toString() { return ""; }

//        public void Connect() {
//            System.Net.Sockets.TcpClient TCPClient = null;
//            System.Net.Sockets.NetworkStream NETStream = null;
//            System.IO.StreamWriter StreamWriter = null;

//            try {
//                TCPClient = new System.Net.Sockets.TcpClient();

//                TCPClient.Connect(new System.Net.IPEndPoint(System.Net.IPAddress.Parse(this.setIPAddress), this.setListenPort));
//                NETStream = TCPClient.GetStream();
//                StreamWriter = new System.IO.StreamWriter(NETStream, System.Text.UTF8Encoding.UTF8);
//                StreamWriter.AutoFlush = true;

//                string msg = "";
//                while ((msg = System.Console.ReadLine()) != "EOF") {
//                    StreamWriter.WriteLine(msg);
//                    System.Threading.Thread.Sleep(100);
//                } StreamWriter.WriteLine("END THE SOCKET");
//            } catch (System.Exception e) { System.Console.WriteLine(e.Message); } finally { StreamWriter.Close(); NETStream.Close(); TCPClient.Close(); }
//        }
//    }

//    public class Server : TCPBase {
//        public Server(string setIPAddress, int setListenPort) : base(setIPAddress, setListenPort) { }
//        public override string toString() { return ""; }

//        public void Listen() {
//            System.Net.Sockets.TcpListener TCPServer = new System.Net.Sockets.TcpListener(new System.Net.IPEndPoint(System.Net.IPAddress.Parse(this.setIPAddress), this.setListenPort));
//            TCPServer.Start();
//            System.Net.Sockets.TcpClient TCPClient = new System.Net.Sockets.TcpClient();

//            System.Net.Sockets.NetworkStream NETStream = null;
//            System.IO.StreamReader StreamReader = null;
//            bool isListen = true;

//            try {
//                do {
//                    if (!TCPClient.Connected) {
//                        TCPClient = TCPServer.AcceptTcpClient();
//                        if (TCPClient.Connected) { System.Console.WriteLine("Connected..."); } else { System.Console.WriteLine("Connect Failed... [Retry]"); }
//                        System.Threading.Thread.Sleep(100);
//                    }

//                    NETStream = TCPClient.GetStream();
//                    if (NETStream != null && NETStream.CanRead) {
//                        StreamReader = new System.IO.StreamReader(NETStream, System.Text.UTF8Encoding.UTF8);
//                        try {
//                            string msg = StreamReader.ReadLine();
//                            if (!string.IsNullOrEmpty(msg)) {
//                                System.Console.WriteLine("Listen Message: {0}", msg);
//                            } else { isListen = false; }
//                        } catch (System.Exception e) { System.Console.WriteLine(e.Message); } finally { }
//                    }
//                } while (isListen);
//            } catch (System.Exception e) { System.Console.WriteLine(e.Message); } finally { StreamReader.Close(); NETStream.Close(); TCPClient.Close(); TCPServer.Stop(); }
//        }
//    }

//    public class TCPProgram {
//        public static void getProgramHelp() {
//            System.Console.WriteLine("*.exe -Mode -IP:5074");
//            System.Console.ReadKey();
//            System.Environment.Exit(0xFF);
//        }

//        public static void Main(string[] args) {
//            try {
//                if (args.Length != 2 || args.Length != 3) { throw new System.NotSupportedException("Error Args Length..."); }
//            } catch (System.Exception e) { getProgramHelp(); }

//            string launcherMode = args[0];
//            string setIPAddress = args[1];
//            int setListenPort = 5074;

//            switch (launcherMode) {
//                case "Client":
//                    Client myClient = new Client(setIPAddress, setListenPort, "args[2]");
//                    myClient.Connect(); break;
//                case "Server":
//                    Server myServer = new Server(setIPAddress, setListenPort);
//                    myServer.Listen(); break;
//                default:
//                    getProgramHelp(); break;
//            }
//        }
//    }
//}

//namespace Task {
//    public class Tasker {
//        public static void Main() {
//            System.Threading.Tasks.Task<string>[] taskArray = new System.Threading.Tasks.Task<string>[] {
//                System.Threading.Tasks.Task<string>.Factory.StartNew(() => work1("arg work1")),
//                System.Threading.Tasks.Task<string>.Factory.StartNew(() => work2("arg work2")),
//                System.Threading.Tasks.Task<string>.Factory.StartNew(() => work3("arg work3")),
//            }; System.Console.WriteLine("".PadLeft(30, '-'));

//            string[] resultSet = new string[taskArray.Length];
//            for (int loop = 0x00; loop < resultSet.Length; ++loop) { resultSet[loop] = taskArray[loop].Result; }
//            foreach (string result in resultSet) { System.Console.WriteLine(result); }
//            System.Console.WriteLine("".PadLeft(30, '-')); System.Console.ReadKey();
//        }

//        private static string work1(string msg = "") { string str = msg + "@" + System.Threading.Thread.CurrentThread.ManagedThreadId; System.Threading.Thread.Sleep(1000); return str; }
//        private static string work2(string msg = "") { string str = msg + "@" + System.Threading.Thread.CurrentThread.ManagedThreadId; System.Threading.Thread.Sleep(1000); return str; }
//        private static string work3(string msg = "") { string str = msg + "@" + System.Threading.Thread.CurrentThread.ManagedThreadId; System.Threading.Thread.Sleep(1000); return str; }
//    }
//}

//namespace webTest {
//    public class web {
//        public static void Main() {
//            try {
//                System.Net.WebClient MyWebClient = new System.Net.WebClient();
//                MyWebClient.Credentials = System.Net.CredentialCache.DefaultCredentials;

//                System.Uri url = new System.Uri("http://baidu.com/s?wd=233");
//                byte[] pageData = MyWebClient.DownloadData(url);
//                // System.Text.StringBuilder pageHtml = new System.Text.StringBuilder(System.Text.Encoding.UTF8.GetString(pageData));
//                string pageHtml = System.Text.Encoding.UTF8.GetString(pageData).Replace("<em>", "").Replace("</em>", "");

//                for (int index = 0x00; index <= pageHtml.Length;) {
//                    int offsetBefore = "data-tools= ".Length;
//                    int offsetAfter = " ".Length;
//                    int start = pageHtml.IndexOf("data-tools=", index);
//                    if (start >= 0x00) {
//                        start += offsetBefore;
//                        int end = pageHtml.IndexOf("}", start);
//                        string set = pageHtml.Substring(start, end - start + offsetAfter);
//                        string setBefore = set;
//                        string setAfter = set;

//                        int setStart = setBefore.IndexOf(":") + 0x02;
//                        int setEnd = setBefore.IndexOf(",", setStart) - 0x01;
//                        string before = setBefore.Substring(setStart, setEnd - setStart);

//                        setStart = setAfter.IndexOf(":", setEnd) + 0x02;
//                        setEnd = setAfter.IndexOf("}") - 0x01;
//                        string after = setAfter.Substring(setStart, setEnd - setStart);

//                        System.Uri urlInner = new System.Uri(after);
//                        System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(urlInner);
//                        System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
//                        var stream = response.GetResponseStream();
//                        System.IO.StreamReader reader = new System.IO.StreamReader(stream, System.Text.Encoding.GetEncoding("gb2312"));
//                        var str = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.GetEncoding("utf-8").GetBytes(reader.ReadToEnd()));

//                        System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"<title>([^<]+)</title>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
//                        System.Text.RegularExpressions.MatchCollection m = reg.Matches(str);
//                        foreach (System.Text.RegularExpressions.Match match in m) {
//                            before = match.Value.Replace("<title>", "").Replace("</title>", "");
//                        } System.Console.WriteLine("Start: " + start + ", End: " + end + "\n\t-# Title: " + before + "\n\t-# URL: " + after);

//                        index = end;
//                    } else { break; }
//                } System.Console.ReadLine();
//            } catch (System.Net.WebException webEx) { System.Console.WriteLine(webEx.Message.ToString()); }
//        }
//    }
//}

//using System.Linq;

//namespace plinq {
//    public class plinqTest {
//        public static void Main() {
//            int count = 0x00;
//            foreach (int x in new int[] { 0, 0, 0, 0 }) {
//                System.Collections.Generic.IEnumerable<int> source = Enumerable.Range(1, 10000);
//                ParallelQuery<int> nums = from num in source.AsParallel() where Abs(num) >= 0x00 select num;
//                count = NewMethod(count, nums);
//            }
//            System.Console.ReadKey();
//        }

//        private static int NewMethod(int count, ParallelQuery<int> nums) {
//            foreach (int num in nums) { ++count; }
//            System.Console.WriteLine(count);
//            count = 0x00;
//            return count;
//        }

//        public static int Abs(int num = 0x00) {
//            System.Random ran = new System.Random();
//            int RandKey = ran.Next(0, 20);
//            if (RandKey % 0x02 == 0x00) { return -1; } else { return 1; }
//        }
//    }
//}

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
        private static System.Net.Sockets.Socket ClientSocket = null;
        public string ClientName { set; get; }
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
                recMess.Abort();
            } catch (System.ObjectDisposedException e) {
            } catch (System.Exception e) {
                /*
                    System.Console.WriteLine(e.GetType() + ": " + e.Message);
                */
                myClientSocket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                ClientSocket.Close();
            } finally { }
        }

        internal protected bool SendMessage() {
            try {
                string msg = string.Empty;
                System.Console.WriteLine("Send Message to SET Name: {0}", this.ClientName);
                ClientSocket.Send(System.Text.Encoding.UTF8.GetBytes("SET NAME: " + this.ClientName));
                while ((msg = System.Console.ReadLine()) != "EOF") {
                    if (!isConnect()) { break; }

                    if (msg.Length >= 0x00) {
                        byte[] sMessage = System.Text.Encoding.UTF8.GetBytes(msg);
                        ClientSocket.Send(sMessage, sMessage.Length, 0);
                        System.Console.WriteLine("Send Message to [{0}]: {1}", ClientSocket.RemoteEndPoint.ToString(), msg);
                    }
                    System.Threading.Thread.Sleep(100);
                }
                if (isConnect()) { ClientSocket.Send(System.Text.Encoding.UTF8.GetBytes("END OF THE SOCKET")); }
            } catch (System.Exception e) { System.Console.WriteLine(e.GetType() + ": " + e.Message); } finally {
                ClientSocket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                ClientSocket.Close();
            }
            return false;
        }

        internal protected bool isConnect() { return ClientSocket.Connected; }
    }

    public class Server : SocketBase {
        // private static System.Collections.Generic.Dictionary<string, string> ChatLog = new System.Collections.Generic.Dictionary<string, string> { };
        private static System.Collections.ArrayList ChatLog = new System.Collections.ArrayList();
        private static System.Net.Sockets.Socket ServerSocket = null;

        public Server(string setSocketIPAddress, int setSocketPort) : base(setSocketIPAddress, setSocketPort) { }
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
                                        System.Threading.Thread.Sleep(500);
                                        do {
                                            if (currentPoint < ChatLog.Count) {
                                                myClientSocket.Send(System.Text.Encoding.UTF8.GetBytes(string.Format("\tChat Log: {0}", ChatLog[currentPoint++])));
                                            }
                                            System.Threading.Thread.Sleep(500);
                                        } while (!myClientSocket.Poll(10, System.Net.Sockets.SelectMode.SelectRead));
                                    } catch (System.Exception e) { System.Console.WriteLine("{0}: {1}", e.GetType(), e.Message); } finally { }
                                });
                                SendTimer.Start();

                                int receiveMessageLength = myClientSocket.Receive(result);
                                string ClientName = System.Text.Encoding.UTF8.GetString(result, 0, receiveMessageLength);
                                if (ClientName.IndexOf("SET NAME: ") >= 0x00) {
                                    ClientName = ClientName.Substring("SET NAME: ".Length);
                                } else { ClientName = string.Empty; }
                                System.Console.WriteLine("Socket STARTED by {0}...", ClientName);
                                do {
                                    receiveMessageLength = myClientSocket.Receive(result);
                                    lock (ChatLog) { ChatLog.Add(ClientName + ": " + System.Text.Encoding.UTF8.GetString(result, 0, receiveMessageLength)); }
                                    System.Console.WriteLine("Received Message [0] by [{1} @ -#{2}]: {3}", System.Threading.Thread.CurrentThread.ManagedThreadId, myClientSocket.RemoteEndPoint.ToString(), ClientName, System.Text.Encoding.UTF8.GetString(result, 0, receiveMessageLength));
                                    System.Threading.Thread.Sleep(200);
                                } while (true);
                            } catch (System.Exception) { } finally { };
                        }));
                        PrintMessage.Start();
                        do {
                            System.Threading.Thread.Sleep(500);
                        } while (!myClientSocket.Poll(10, System.Net.Sockets.SelectMode.SelectRead));
                        System.Console.WriteLine("Socket END...");
                        PrintMessage.Abort();
                    } catch (System.Exception) { } finally { myClientSocket.Shutdown(System.Net.Sockets.SocketShutdown.Both); myClientSocket.Close(); }
                }));
                receiveThread.Start();
            } while (true);
        }
    }

    public class SocketProgram {
        public static void getProgramHelp() {
            System.Console.WriteLine("*.exe -Mode -IP:50740");
            System.Console.ReadKey();
            System.Environment.Exit(0xFF);
        }

        public static void Main(string[] args) {
            string launcherMode = string.Empty;
            string setIPAddress = string.Empty;
            string ClientName = string.Empty;
            int setListenPort = 50740;
            bool flag = true;

            try {
                if (args.Length != 2 && args.Length != 3) { throw new System.NotSupportedException("Error Args Length..."); } else if (args.Length == 3) {
                    launcherMode = args[0];
                    setIPAddress = args[1];
                    ClientName = args[2];
                } else { launcherMode = args[0]; setIPAddress = args[1]; ClientName = "[Client Name]"; }
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
                    Server myServer = new Server(setIPAddress, setListenPort);
                    myServer.Listen();
                    break;
                default:
                    getProgramHelp();
                    break;
            }
        }
    }
}