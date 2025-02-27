namespace ChatEx.Helpers
{
    /// <summary>
    /// 调用函数返回数据或者异常结果
    /// </summary>
    public class ReturnResult<T>
    {
        /// <summary>
        /// 返回的数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 错误信息集合
        /// </summary>
        public IEnumerable<string> Errors { get; protected set; }
        //public IEnumerable<WebApiError> Errors { get; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; protected set; } = string.Empty;

        /// <summary>
        /// 是否异常结果
        /// </summary>
        public bool IsErrorResult => !string.IsNullOrWhiteSpace(ErrMsg);

        /// <summary>
        /// 是否异常结果或者返回空值
        /// </summary>
        public bool IsErrorOrNullResult => IsErrorResult || (Data == null);

        public ReturnResult() { }

        public ReturnResult(T data)
        {
            this.Data = data;
        }

        /// <summary>
        /// 返回异常结果
        /// 注意，不能定义为构造函数，因为WebApiResult(T data)的T可能是string
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static ReturnResult<T> ErrResult(string errMsg)
        {
            var result = new ReturnResult<T>()
            {
                Errors = new List<string>() { errMsg },
                ErrMsg = errMsg,
            };

            return result;
        }

        public static ReturnResult<T> ErrResult(IEnumerable<string> errors)
        {
            var result = new ReturnResult<T>()
            {
                Errors = errors,
                ErrMsg = string.Join(Environment.NewLine, errors),
            };

            return result;
        }
    }
}
