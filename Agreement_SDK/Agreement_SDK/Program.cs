using Neo;
using Neo.IO.Json;
using Neo.Network.P2P.Payloads;
using Neo.Network.RPC;
using Neo.Network.RPC.Models;
using Neo.SmartContract;
using Neo.VM;
using System;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Agreement_SDK
{
    class Program
    {
        private static string myContractScriptHash = "0xb868d0ee4a6153f511fcb0be6967e2924be2d40f";
        private static string privateKey = "2b7cc67d8e9a0915bb05907dfa09e353062d6e31e728b8a824620893066d24ac";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            RpcClient client = new RpcClient("http://seed1t.neo.org:20332");
            Neo.Wallets.KeyPair myKey = Neo.Network.RPC.Utility.GetKeyPair(privateKey);

            // get ScriptHash of KeyPair account
            UInt160 scriptHash = Contract.CreateSignatureContract(myKey.PublicKey).ScriptHash;
            Cosigner[] cosigners = new[] { new Cosigner { Scopes = WitnessScope.CalledByEntry, Account = scriptHash } };
            byte[] script = scriptHash.MakeScript("addAgText", "String for test");
            // initialize the TransactionManager with rpc client and sender scripthash 
            TransactionManager txManager = new TransactionManager(client, scriptHash);
            // fill the script, attributes and cosigners 
            txManager.MakeTransaction(script, null, cosigners);
            // add signature for the transaction with sendKey 
            var ss = txManager.AddSignature(myKey);
            // sign transaction with the added signature 
            txManager.Sign();
            Transaction tx = txManager.Tx;
            Console.WriteLine("Done");


            // broadcasts the transaction over the Neo network 
            client.SendRawTransaction(tx);

            // print a message after the transaction is on chain 
            WalletAPI neoAPI = new WalletAPI(client);
            neoAPI.WaitTransaction(tx)
                .ContinueWith(async (p) => Console.WriteLine($"Transaction vm state is  {(await p).VMState}"));

            Console.ReadKey();
        }
    }
}
