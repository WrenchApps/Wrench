using App.Contexts;

namespace App.Handlers.Http
{
    public class HttpWriteResponseMockHandler : Handler
    {
        public override async Task DoAsync(StepladderHttpContext context)
        {
            if (context.RouteSetting.ResponseMock != null)
            {
                var responseMock = context.RouteSetting.ResponseMock;
                context.HttpContext.Response.StatusCode = responseMock.StatusCode;

                context.HttpContext.Response.Headers.Add("Content-Type", responseMock.ContentType);
                await context.HttpContext.Response.WriteAsync(responseMock.BodyValue);
            }

            await NextAsync(context);
        }
    }
}
