
using Microsoft.EntityFrameworkCore;
using Webshop.Interface;
using Webshop.Models;

namespace Webshop.Extensions.JWT
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly MyDbContext _context;
        private readonly IJwtExtension _jwtExtension;

        public JwtMiddleware(RequestDelegate next, IJwtExtension jwtExt, MyDbContext context)
        {
            _next = next;
            _jwtExtension = jwtExt;
            _context = context;
        }

        public async Task Invoke(HttpContext context)
        {
            string? token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtExtension.ValidateToken(token);
            if (userId != null)
            {
                // attach user to context on successful jwt validation
                context.Items["Customer"] = await _jwtExtension.GetCustomerByID(userId.Value);
            }

            await _next(context);
        }
    }
}
