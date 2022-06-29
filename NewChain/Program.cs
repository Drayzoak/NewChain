using Newtonsoft.Json;
class Program
{
    public static int Port;
    private static P2PServer _server;
    private static readonly P2PClient Client = new P2PClient();
    public static Blockchain NewChain = new Blockchain();
    private static string _name = "Unknown";
    static void Main(string[] args)
    {

        if (args.Length >= 1)
            Port = int.Parse(args[0]);
        if (args.Length >= 2)
            _name = args[1];

        if (Port > 0)
        {
            _server = new P2PServer();
            _server.Start();
        }
        if (_name != "Unkown")
        {
            Console.WriteLine($"Current user is {_name}");
        }

        Console.WriteLine("=========================");
        Console.WriteLine("1. Connect to a server");
        Console.WriteLine("2. Add a transaction");
        Console.WriteLine("3. Display Blockchain");
        Console.WriteLine("4. Start Mining");
        Console.WriteLine("5. Get Balance Of Account");
        Console.WriteLine("6. Exit");
        Console.WriteLine("=========================");

        int selection = 0;
        while (selection != 6)
        {
            switch (selection)
            {
                case 1:
                    Console.WriteLine("Please enter the server URL");
                    string serverUrl = Console.ReadLine();
                    
                    Client.Connect($"{serverUrl}/BlockChain");
                    break;
                case 2:
                    Console.WriteLine("Please enter the receiver name");
                    string receiverName = Console.ReadLine();
                    Console.WriteLine("Please enter the amount");
                    string amount = Console.ReadLine();
                    NewChain.CreateTransaction(new Transaction(_name, receiverName, int.Parse(amount)));
                    Client.Broadcast(JsonConvert.SerializeObject(NewChain));
                    break;
                case 3:
                    Console.WriteLine("Blockchain");
                    Console.WriteLine(JsonConvert.SerializeObject(NewChain, Formatting.Indented));
                    break;
                case 4:
                    NewChain.ProcessPendingTransactions(_name);
                    break;
                case 5:
                    string Balanceof = Console.ReadLine();
                    Console.WriteLine(NewChain.GetBalance(Balanceof));
                    break;
            }

            Console.WriteLine("Please select an action");
            string action = Console.ReadLine();
            selection = int.Parse(action);
        }

        Client.Close();
    }
}
