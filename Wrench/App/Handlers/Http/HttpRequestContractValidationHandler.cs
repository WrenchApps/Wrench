using App.Contexts;
using App.Helpers;
using App.Helpers.Validations;
using Opentelemetry.Trace;
using System.Diagnostics;
using System.Text.Json;

namespace App.Handlers.Http
{
    public class HttpRequestContractValidationHandler : Handler
    {
        public override async Task DoAsync(StepladderHttpContext context)
        {
            if (ContractValidation != null)
            {
                var trace = context.HttpContext.RequestServices.GetService<ITraceService>();
                Activity activity = null;
                if (trace != null)
                    activity = trace.StartActivity("HttpRequestContractValidationHandler");

                var json = await context.GetHttpContextRequestBodyAsync();
                var jsonValidation = new JsonValidation(json);

                var resultValidation = ResultValidation.Create();
                ResultValidation resultPropertyValidation = null;
                ResultValidation resultArrayObjectsValidation = null;

                var task = Task.Run(() => {
                    resultPropertyValidation = jsonValidation.Validate(ContractValidation.Properties);
                    resultArrayObjectsValidation = jsonValidation.Validate(ContractValidation.ValidationArrayObjects);
                });
                
                Task.WaitAll(task);
                
                resultValidation.Concate(resultPropertyValidation);
                resultValidation.Concate(resultArrayObjectsValidation);

                if (resultValidation.Success == false)
                {
                    context.SetHttpProcessorWithError();
                    context.ResponseContext.ResponseBodyStringValue = JsonSerializer.Serialize(resultValidation);
                }

                activity?.Dispose();
            }

            await NextAsync(context);
        }
    }
}
