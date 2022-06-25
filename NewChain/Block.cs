using System;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
public class Block
{
	public long Index { get; set; }
	public DateTime TimeStamp { get; set; }
	public string PrevHash { get; set; }
	public string Hash { get; set; }
	public IList<Transaction> Transactions { get; set; }
	public long Nonce { get; set; }
	public Block(DateTime timestamp, IList<Transaction> transactions)
    {
		TimeStamp = timestamp;
		Transactions = transactions;
    }
	public void mineBlock(int difficulty)
	{
		var leadingZeros = new string('0', difficulty);  
		while (this.Hash == null || this.Hash.Substring(0, difficulty) != leadingZeros)  
		{  
			this.Nonce++;  
			this.Hash = this.CalculateHash();  
		}  

	}
	public string CalculateHash()  
	{  
		SHA256 sha256 = SHA256.Create();  
	
		byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PrevHash ?? ""}-{JsonConvert.SerializeObject(Transactions)}-{Nonce}");  
		byte[] outputBytes = sha256.ComputeHash(inputBytes);  
	
		return Convert.ToHexString(outputBytes);  
	}  

}