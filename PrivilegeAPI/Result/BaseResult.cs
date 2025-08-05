﻿using System.Text.Json.Serialization;

namespace PrivilegeAPI.Result
{
    public class BaseResult
    {
        public bool IsSuccess => ErrorMessage == null;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string ErrorMessage { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int? ErrorCode { get; set; }
    }

    public class BaseResult<T> : BaseResult
    {
        public T Data { get; set; }

        public BaseResult(string errorMessage, int errorCode, T data)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            Data = data;
        }

        public BaseResult() { }
    }
}
