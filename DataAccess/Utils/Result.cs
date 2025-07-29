using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataAccess.Utils
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public T? Data { get; set; }

        public static Result<T> Ok(T data, string message = "")
        {
            return new Result<T>() { Success = true, Data = data, Message = message };
        }

        public static Result<T> Fail(string message)
        {
            return new Result<T>() { Success = false, Message = message };
        }
    }
}
