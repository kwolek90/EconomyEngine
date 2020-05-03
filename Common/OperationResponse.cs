using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    
    public class OperationResponse
    {
        public OperationResponse()
        {
            Errors = new Dictionary<string, List<string>>();
        }
        public Dictionary<string,List<string>> Errors { get; set; }
        
        public bool Success
        {
            get { return !Errors.Any(); }
        }

        public void AddError(string key, string message)
        {
            if (!Errors.ContainsKey(key))
            {
                Errors[key] = new List<string>();
            }
            Errors[key].Add(message);
        }

        public void Merge(OperationResponse secondResponse)
        {
            foreach (var keyValue in secondResponse.Errors)
            {
                var key = keyValue.Key;
                var messages = keyValue.Value;
                foreach (var message in messages)
                {
                    AddError(key,message);
                }

            }
        }
    }
    
    public class OperationResponse<T> : OperationResponse where T:class 
    {
        public T Response { get; set; }

        public void Merge(OperationResponse<T> response)
        {
            base.Merge(response);
            Response = response.Response;
        }
    }
}