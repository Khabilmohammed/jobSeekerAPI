namespace jobSeeker.Middlware
{
    public class SimpleCustommiddlware
    {
        private readonly RequestDelegate _next;

        public SimpleCustommiddlware(RequestDelegate requestDelegate)
        {
            _next = requestDelegate;    
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.Request.Method=="post")
            {
                httpContext.Response.StatusCode = 401; 
                await httpContext.Response.WriteAsync("Unauthorized request");
                return;
            }

            _next(httpContext);
        }
    }
}
