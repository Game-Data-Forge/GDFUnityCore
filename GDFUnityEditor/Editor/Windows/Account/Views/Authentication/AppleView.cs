using GDFFoundation;

namespace GDFUnity.Editor.ServiceProviders
{
    public class AppleView : OAuthView
    {
        public override string Name => "Apple";
        public override string Title => "Apple authentication";
        public override string Help => "/unity/editor-windows/account/authentication/apple";

        public AppleView(AccountView view) : base(view)
        {
        }

        protected override IJob Login(Country country, string clientId, string token)
        {
            return GDFEditor.Account.Authentication.Apple.Login(country, clientId, token, true);
        }
    }
}
