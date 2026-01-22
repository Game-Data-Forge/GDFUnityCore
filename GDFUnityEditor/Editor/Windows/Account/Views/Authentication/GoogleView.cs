using GDFFoundation;

namespace GDFUnity.Editor.ServiceProviders
{
    public class GoogleView : OAuthView
    {
        public override string Name => "Google";
        public override string Title => "Google authentication";
        public override string Help => "/unity/editor-windows/account/authentication/google";

        public GoogleView(AccountView view) : base(view)
        {
        }

        protected override IJob Login(Country country, string clientId, string token)
        {
            return GDFEditor.Account.Authentication.Google.Login(country, clientId, token, true);
        }
    }
}
