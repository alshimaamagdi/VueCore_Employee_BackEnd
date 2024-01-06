namespace VueCore_Employee_BackEnd.CustomConfiguration
{
    public class GlobalRoleMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalRoleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var user = context.User;

            // Check if the user has the "Admin" role
            if (user.IsInRole("Admin"))
            {
                context.Response.Headers.Add("IsAdmin", "true");
            }
            else 
            {
                context.Response.Headers.Add("IsAdmin", "false");

            }

            await _next(context);
        }

    }
}
