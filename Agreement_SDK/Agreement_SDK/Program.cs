using Neo;
using Neo.IO.Json;
using Neo.Network.P2P.Payloads;
using Neo.Network.RPC;
using Neo.Network.RPC.Models;
using Neo.SmartContract;
using Neo.SmartContract.Native;
using Neo.VM;
using Neo.Wallets;
using System;
using System.Linq;
using System.Numerics;
using System.Security.Policy;
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


            /// Instruction: https://docs.neo.org/v3/docs/en-us/tooldev/sdk/transaction.html#transaction-construction-process
            // choose a neo node with rpc opened
            RpcClient client = new RpcClient("http://seed1t.neo.org:20332");



            // construct the script
            ReadOnlySpan<byte> myContractSpan = Encoding.Default.GetBytes(myContractScriptHash);
            UInt160 scriptHash = new UInt160(myContractSpan);
            byte[] script = scriptHash.MakeScript("addAgText", "String for test");


            // get ScriptHash of KeyPair account
            KeyPair sendKey = Neo.Network.RPC.Utility.GetKeyPair(privateKey);
            UInt160 sender = Contract.CreateSignatureContract(sendKey.PublicKey).ScriptHash;


            // add Cosigners, which is a collection of scripthashs that need to be signed
            Cosigner[] cosigners = new[] { new Cosigner { Scopes = WitnessScope.CalledByEntry, Account = sender } };

            // initialize the TransactionManager with rpc client and sender scripthash 
            TransactionManager txManager = new TransactionManager(client, sender);

            // fill the script, attributes and cosigners 
            txManager.MakeTransaction(script, null, cosigners);

            // add signature for the transaction with sendKey 
            txManager.AddSignature(sendKey);

            // sign transaction with the added signature 
            txManager.Sign();

            Transaction tx = txManager.Tx;


            // broadcasts the transaction over the Neo network 
            client.SendRawTransaction(tx);

            Console.WriteLine("Done");

            // print a message after the transaction is on chain 
            WalletAPI neoAPI = new WalletAPI(client);
            neoAPI.WaitTransaction(tx)
                .ContinueWith(async (p) => Console.WriteLine($"Transaction vm state is  {(await p).VMState}"));

            Console.ReadKey();
        }
    }
}
