using Microsoft.JSInterop;
using DynamicFormsApp.Shared.Services;

namespace DynamicFormsApp.Client.Services
{
    public class CookieHelper
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly IUserService _userService;

        // Constructor to initialize dependencies
        public CookieHelper(IJSRuntime jsRuntime, IUserService userService)
        {
            _jsRuntime = jsRuntime;
            _userService = userService;
        }

        // Retrieves the value of a cookie by name
        public async Task<string> LoginStatus()
        {
            var user = await _jsRuntime.InvokeAsync<string>("cookieHelper.getCookie");
            if (string.IsNullOrEmpty(user))
            {
                var current = await _userService.GetCurrentUser();
                if (!string.IsNullOrEmpty(current))
                {
                    await LoginUser(current);
                    user = current;
                }
            }
            return user;
        }

        // Sets a cookie with the specified name and value
        public async Task LoginUser(string value)
        {
            await _jsRuntime.InvokeVoidAsync("cookieHelper.setCookie", value);
        }

        // Deletes a cookie by name
        public async Task LogoutAsync()
        {
            await _jsRuntime.InvokeVoidAsync("cookieHelper.deleteCookie");
        }
    }
}
