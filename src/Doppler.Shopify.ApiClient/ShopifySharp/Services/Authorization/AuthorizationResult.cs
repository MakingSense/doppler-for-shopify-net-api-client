using System;
using System.Collections.Generic;
using System.Text;

namespace ShopifySharp
{
    public class AuthorizationResult
    {
        public string AccessToken { get; private set; }
        public string[] GrantedScopes { get; private set; }

        internal AuthorizationResult(string accessToken, string[] grantedScopes)
        {
            this.AccessToken = accessToken;
            this.GrantedScopes = grantedScopes;
        }
    }
}
