using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.Numerics;

namespace NeoContract6
{
    [ManifestExtra("Author", "Neo")]
    [ManifestExtra("Email", "dev@neo.org")]
    [ManifestExtra("Description", "This is a contract example")]
    [Features(ContractFeatures.HasStorage)]
    public class Contract1 : SmartContract
    {
        public static bool Main(string operation, object[] args)
        {
            Storage.Put("Hello", "World");
            return true;
        }
    }
}
