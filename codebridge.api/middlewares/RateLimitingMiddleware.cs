namespace codebridge.api.middlewares;

public class RateLimitingMiddleware
{
    //private const int RequestLimit = 3; // for testing
    private const int RequestLimit = 10;
    private const int ToManyRequestsStatusCode = 429;

    private readonly SemaphoreSlim _semaphore;
    private readonly RequestDelegate _next;

    public RateLimitingMiddleware(RequestDelegate next)
    {
        _next = next;
        _semaphore = new SemaphoreSlim(RequestLimit);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (await _semaphore.WaitAsync(0))
        {
            await _next.Invoke(context);

            _semaphore.Release();
        }
        else
        {
            await RespondForTooManyRequestsMessage(context);
        }
    }

    private static async Task RespondForTooManyRequestsMessage(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = ToManyRequestsStatusCode;

        await context.Response.WriteAsync("Too many requests");
    }
}
