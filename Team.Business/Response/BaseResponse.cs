using System;
using System.Collections.Generic;
using System.Text;

namespace Team.Business.Response
{
    public class BaseResponse
    {
        public bool Succeeded { get; set; } = false; // ŞU an olumsuz olarak varsayılan değerini atadık.
        public ResultInfo Info { get; set; }
    }

    public class ResultInfo
    {
        public ResponseType ResponseType { get; set; }
        public string Message { get; set; }
    }

    public enum ResponseType // Enum sayılabilen demek. Tekrar tekrar durumlar döndürmemiz gerektiğinde kulllanılır ki sürekli hatırlamak zorunda kalmayalım.
    {
        OK = 1,
        EmptyValue,
        NoDataFound,
        SqlException,
        ExceptionError,
        CreateError,
        ReadError,
        UpdateError,
        DeleteError,
        Error
    }
}
