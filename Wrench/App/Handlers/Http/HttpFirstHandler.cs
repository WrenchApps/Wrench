using App.Contexts;

namespace App.Handlers.Http
{
    public class HttpFirstHandler : Handler
    {
        public override async Task DoAsync(WrenchHttpContext context)
        {
            await NextAsync(context);
        }
    }
}
