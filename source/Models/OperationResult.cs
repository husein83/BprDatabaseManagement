using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseManagement.Models
{
    public class OperationResult
    {
        public readonly bool IsSuccess;
        public string Message { get; set; } = string.Empty;

        private OperationResult(bool isSuccess, string message)
        {
            this.Message = message;
            this.IsSuccess = isSuccess;
        }

        public static OperationResult Success(string message)
        {
            return new OperationResult(true, message);
        }

        public static OperationResult Failure(string message)
        {
            return new OperationResult(false, message);
        }
    }
}
