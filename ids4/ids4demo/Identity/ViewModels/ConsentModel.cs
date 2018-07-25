using System.Collections.Generic;

namespace Identity.ViewModels
{
    public class ConsentModel
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientUrl { get; set; }
        public string ClientLogoUrl { get; set; }
        public bool AllowRememberConsent { get; set; }
        public  string ReturnUrl { get; set; }
        public IEnumerable<ScopeModel> IdentityScopes { get; set; }
        public IEnumerable<ScopeModel> ResourceScopes { get; set; }
    }

    public class ScopeModel
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Emphasize { get; set; }
        public bool Required { get; set; }
        public bool Checked { get; set; }
    }

    public class ConsentInputModel
    {
        public string Button { get; set; }
        public IEnumerable<string> Scopes { get; set; }
        public bool RememberConsent { get; set; }
        public string ReturnUrl { get; set; }
    }
}