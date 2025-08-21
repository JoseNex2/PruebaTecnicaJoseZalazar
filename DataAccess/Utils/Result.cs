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
        public default(T)? Response { get; set; } = null;
        public int Code { get; set; }

        public static Result<T> Ok(T response = null, string message = "")
        {
            return new Result<T>() { Success = true, Response = response, Message = message };
        }

        public static Result<T> Fail(string message)
        {
            return new Result<T>() { Success = false, Message = message };
        }
    }
}
