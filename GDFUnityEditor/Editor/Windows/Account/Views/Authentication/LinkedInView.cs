using GDFFoundation;

namespace GDFUnity.Editor.ServiceProviders
{
    public class LinkedInView : OAuthView
    {
        public override string Name => "LinkedIn";
        public override string Title => "LinkedIn authentication";
        public override string Help => "/unity/editor-windows/account/authentication/linkedin";

        public LinkedInView(AccountView view) : base(view)
        {
        }

        protected override IJob Login(Country country, string clientId, string token)
        {
            return GDFEditor.Account.Authentication.LinkedIn.Login(country, clientId, token, true);
        }
    }
}
