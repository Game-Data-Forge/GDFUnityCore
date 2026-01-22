using GDFFoundation;

namespace GDFUnity.Editor.ServiceProviders
{
    public class FacebookView : OAuthView
    {
        public override string Name => "Facebook";
        public override string Title => "Facebook authentication";
        public override string Help => "/unity/editor-windows/account/authentication/facebook";

        public FacebookView(AccountView view) : base(view)
        {
        }

        protected override IJob Login(Country country, string clientId, string token)
        {
            return GDFEditor.Account.Authentication.Facebook.Login(country, clientId, token, true);
        }
    }
}
