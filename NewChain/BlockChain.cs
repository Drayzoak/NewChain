public class Blockchain  
{      
    public IList<Block> Chain { set;  get; }  
    private int Difficulty { set; get; } = 2;
    public IList<Transaction> PendingTransactions = new List<Transaction>();
    private int _reward = 1; 
    public Blockchain()  
    {  
        InitializeChain();  
        AddGenesisBlock();  
    }  
    public void InitializeChain()  
    {  
        Chain = new List<Block>();  
    }  
    private Block CreateGenesisBlock()
    {
        Block block = new Block(DateTime.Now, PendingTransactions);
        block.Index=0;
        block.PrevHash = null ;
        block.mineBlock(Difficulty);
        PendingTransactions = new List<Transaction>();
        return block;
    }
    private void AddGenesisBlock()
    {
        Chain.Add(CreateGenesisBlock());
    }
    public Block GetLatestBlock()  
    {  
        return Chain[Chain.Count - 1];  
    }  
    public void CreateTransaction(Transaction transaction)
    {
        PendingTransactions.Add(transaction);
    }  
    public void ProcessPendingTransactions(string minerAddress)
    {
        Block block = new Block(DateTime.Now, PendingTransactions);
        AddBlock(block);

        PendingTransactions = new List<Transaction>();
        CreateTransaction(new Transaction(null, minerAddress, _reward));
    }
    public void AddBlock(Block block)  
    {  
        Block latestBlock = GetLatestBlock();  
        block.Index = latestBlock.Index + 1; 
        block.TimeStamp = DateTime.Now; 
        block.PrevHash = latestBlock.Hash;  
        block.mineBlock(Difficulty);
        Chain.Add(block);  
        
    }
    public bool IsValid()  
    {  
        for (int i = 1; i < Chain.Count; i++)  
        {  
            Block currentBlock = Chain[i];  
            Block previousBlock = Chain[i - 1];  
    
            if (currentBlock.Hash != currentBlock.CalculateHash())  
            {  
                return false;  
            }  
    
            if (currentBlock.PrevHash != previousBlock.Hash)  
            {   
                return false;  
            }  
        }  
        return true;  
    }  
    // void ReplaceChain()
    // {
    //     if(Chain.Count<= this.Chain.Count){
    //         Console.Error.WriteLine("Chain is INVALID");
    //     }

    //     if(!IsValid()){
    //         Console.Error.WriteLine("Chain is INVALID");
    //     }
    //     Console.Error.WriteLine("Replacing Chian");
    //     this.Chain = Chain;
    // }
    public int GetBalance(string address)
    {
        int balance = 0;

        for (int i = 0; i < Chain.Count; i++)
        {
            for (int j = 0; j < Chain[i].Transactions.Count; j++)
            {
                var transaction = Chain[i].Transactions[j];

                if (transaction.FromAddress == address)
                {
                    balance -= transaction.Amount;
                }

                if (transaction.ToAddress == address)
                {
                    balance += transaction.Amount;
                }
            }
        }
        return balance;
    }
}
