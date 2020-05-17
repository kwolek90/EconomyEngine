using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Common.MVC
{
    public class BaseController:Controller
    { 
        protected ActionResult PrepareResponse<T>(OperationResponse<T> response) where T : class
        {
            if (response.Success)
            {
                if (Request.Method == "DELETE") Response.StatusCode = 201;
                if (Request.Method == "POST") Response.StatusCode = 201;
                if (Request.Method == "GET") Response.StatusCode = 200;
                return Json(response.Response);
            }
            else
            {
                var modelState = new ModelStateDictionary();
                foreach (var keyValuePair in response.Errors)
                {
                    var key = keyValuePair.Key;
                    var messages = keyValuePair.Value;
                    foreach (var message in messages)
                    {
                        modelState.AddModelError(key, message);
                    }
                }
                return BadRequest(modelState);
            }
        }
        
        protected ActionResult PrepareResponse(OperationResponse response)
        {
            var newResponse = new OperationResponse<string>(){Errors = response.Errors, Response = "OK"};
            return PrepareResponse(newResponse);
        }
    }
}