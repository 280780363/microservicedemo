using System.Linq;
using System.Threading.Tasks;
using Identity.ViewModels;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;

namespace Identity.Controllers
{
    public class ConsentController : Controller
    {
        private readonly IClientStore clientStore;
        private readonly IResourceStore resourceStore;
        private readonly IIdentityServerInteractionService interactionService;
        private readonly IEventService _event;

        public ConsentController(
            IClientStore clientStore,
            IResourceStore resourceStore,
            IEventService _event,
            IIdentityServerInteractionService interactionService)
        {
            this.clientStore = clientStore;
            this.resourceStore = resourceStore;
            this.interactionService = interactionService;
            this._event = _event;
        }

        // GET
        public async Task<IActionResult> Index(string returnUrl)
        {
            var request = await interactionService.GetAuthorizationContextAsync(returnUrl);
            if (request != null)
            {
                var client = await clientStore.FindEnabledClientByIdAsync(request.ClientId);
                if (client != null)
                {
                    var resources = await resourceStore.FindResourcesByScopeAsync(request.ScopesRequested);
                    var model = new ConsentModel
                    {
                        ClientId = request.ClientId,
                        ClientName = client.ClientName,
                        ClientLogoUrl = client.LogoUri,
                        AllowRememberConsent = client.AllowRememberConsent,
                        ReturnUrl = returnUrl,
                        ClientUrl = client.ClientUri
                    };


                    model.IdentityScopes = resources.IdentityResources.Select(r => new ScopeModel
                    {
                        Name = r.Name,
                        DisplayName = r.DisplayName,
                        Checked = r.Required,
                        Description = r.Description,
                        Emphasize = r.Emphasize,
                        Required = r.Required
                    }).ToArray();

                    model.ResourceScopes = resources.ApiResources.SelectMany(r => r.Scopes).Select(r => new ScopeModel
                    {
                        Name = r.Name,
                        DisplayName = r.DisplayName,
                        Checked = r.Required,
                        Description = r.Description,
                        Emphasize = r.Emphasize,
                        Required = r.Required
                    }).ToArray();

                    return View(model);
                }

                return View(null);
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ConsentInputModel model)
        {
            var request = await interactionService.GetAuthorizationContextAsync(model.ReturnUrl);
            if (request == null)
                return null;
            if (model.Button == "no")
            {
                await _event.RaiseAsync(
                    new ConsentDeniedEvent(User.GetSubjectId(), request.ClientId, request.ScopesRequested));
                // 拒绝授权 返回原来的页面
                return Redirect(model.ReturnUrl);
            }
            else
            {
                // 同意授权
                await _event.RaiseAsync(new ConsentGrantedEvent(User.GetSubjectId(), request.ClientId,
                    request.ScopesRequested, model.Scopes, model.RememberConsent));
                await interactionService.GrantConsentAsync(request, new ConsentResponse
                {
                    RememberConsent = model.RememberConsent,
                    ScopesConsented = model.Scopes
                });
                return Redirect(model.ReturnUrl);
            }
        }
    }
}