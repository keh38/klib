using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Desktop;
//using Microsoft.Identity.Client.Utils.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLib.MSGraph
{
    public class GraphApp
    {
        public static void CreateApplication(bool useWam)
        {
            //if (WindowsNativeUtils.IsElevatedUser())
            //{
            //    WindowsNativeUtils.InitializeProcessSecurity();
            //}

            if (_clientApp != null) return;

            var builder = PublicClientApplicationBuilder.Create(ClientId)
                .WithAuthority($"{Instance}{Tenant}")
                .WithDefaultRedirectUri();

            if (useWam)
            {
                builder.WithExperimentalFeatures();
                builder.WithWindowsBroker(true);  // Requires redirect URI "ms-appx-web://microsoft.aad.brokerplugin/{client_id}" in app registration
            }
            _clientApp = builder.Build();
            TokenCacheHelper.EnableSerialization(_clientApp.UserTokenCache);
        }

        // Below are the clientId (Application Id) of your app registration and the tenant information. 
        // You have to replace:
        // - the content of ClientID with the Application Id for your app registration
        // - The content of Tenant by the information about the accounts allowed to sign-in in your application:
        //   - For Work or School account in your org, use your tenant ID, or domain
        //   - for any Work or School accounts, use organizations
        //   - for any Work or School accounts, or Microsoft personal account, use 720edb1f-5c4e-4043-8141-214a63a7ead5
        //   - for Microsoft Personal account, use consumers
        private static string ClientId = "08a42019-0538-4d18-bfd7-b6befa1e33a6";

        // Note: Tenant is important for the quickstart.
        private static string Tenant = "720edb1f-5c4e-4043-8141-214a63a7ead5";
        private static string Instance = "https://login.microsoftonline.com/";
        private static IPublicClientApplication _clientApp = null;

        public static IPublicClientApplication PublicClientApp { get { return _clientApp; } }
    }
}
