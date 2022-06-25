using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

public class P2PServer: WebSocketBehavior
{
    bool chainSynched = false;
    WebSocketServer wss = null;

    public void Start()
    {
        wss = new WebSocketServer($"ws://127.0.0.1:{Program.Port}");
        wss.AddWebSocketService<P2PServer>("/BlockChain");
        wss.Start();
        Console.WriteLine($"Started server at ws://127.0.0.1:{Program.Port}");
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        if (e.Data.Contains("Hi Server"))
        {
            Console.WriteLine(e.Data);
            Send($"From {Program.Port}: Hi Client");
        }
        else
        {
            Blockchain newChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);

            if (newChain.IsValid() && newChain.Chain.Count > Program.PhillyCoin.Chain.Count)
            {
                Program.PhillyCoin.Chain = newChain.Chain;
            }

            if (!chainSynched)
            {
                Send(JsonConvert.SerializeObject(Program.PhillyCoin));
                chainSynched = true;
            }
        }
    }
}
