using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.Numerics;

namespace Agreement6
{

    [ManifestExtra("Author", "Matthew Shelby")]
    [ManifestExtra("Email", "Shahabi110@gmail.com")]
    [ManifestExtra("Website", "noren.ir")]
    [ManifestExtra("Description", " This is an Agreement Registration contract.")]
    [Features(ContractFeatures.HasStorage)]
    public class Contract1 : SmartContract
    {
        public static string Main(string operation, object[] args)
        {
            if (operation == "name")
                return Name();

            if (operation == "index")
                return Index();


            if (args.Length > 0)
            {
                //---- Deploy Contract
                if (operation == "deploy")
                    return Deploy((byte[])args[0]);


                //---- ADD Agreement Text
                if (operation == "addAgText")
                    return AddAgText((string)args[0]);


                //---- READ Agreement Text
                if (operation == "readAgText")
                    return ReadAgText((string)args[0]);

            }
            return "Error @ Main. Could not detect the request.";
        }

        #region Dapp Methods

        /// <summary>
        /// Adds the Agreement Text to Storage and return its index
        /// </summary>
        /// <param name="AgText">Agreement Text</param>
        /// <returns></returns>
        public static string AddAgText(string AgText)
        {
            BigInteger Index = Storage.Get(Storage.CurrentContext, "Index").ToBigInteger();
            if (Index > 0)
            {
                Storage.Put(Storage.CurrentContext, Key("AgText_", Index), AgText);
                Storage.Put(Storage.CurrentContext, "Index", Index + 1);
                return Storage.Get(Storage.CurrentContext, "Index").AsString();
            }
            return "Failed";
        }


        /// <summary>
        /// Read the Agreement Text from storage.
        /// </summary>
        /// <param name="Index">Index of agreement</param>
        /// <returns></returns>
        public static string ReadAgText(string Index)
        {
            return Storage.Get(Storage.CurrentContext, Key("AgText_", Index)).AsString();
        }



        /// <summary>
        /// Returns The leatest Index of Storaage
        /// </summary>
        /// <returns></returns>
        public static string Index()
        {
            return Storage.Get(Storage.CurrentContext, "Index").AsString();
        }
        #endregion

        #region Helpers
        private static string Key(string prefix, BigInteger id)
        {
            return string.Concat(prefix, id.ToByteArray().AsString());
        }
        private static string Key(string prefix, byte[] id)
        {
            return string.Concat(prefix, id.AsString());
        }
        private static string Key(string prefix, string id)
        {
            return string.Concat(prefix, id);
        }

        #endregion

        #region  Deploy
        public static string Deploy(byte[] account)
        {
            byte[] ownerCheck = Storage.Get(Storage.CurrentContext, "owner");

            if (ownerCheck == null)
            {
                Storage.Put(Storage.CurrentContext, "owner", account);
                Storage.Put(Storage.CurrentContext, "Index", 1);
                byte[] owner = Storage.Get(Storage.CurrentContext, "owner");
                return "True @ Deploy";
            }
            return "Seems  Deployed Befor!!!";
        }
        public static string Name()
        { return "AgreementContract"; }
        #endregion

    }
}
