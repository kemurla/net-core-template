namespace CAT.Core
{
    public class Result
    {
        /// <summary>
        /// Non-success constructor with a message.
        /// </summary>
        /// <param name="message"></param>
        public Result(string message)
        {
            Message = message;
            Success = false;
        }

        /// <summary>
        /// Success constructor.
        /// </summary>
        public Result()
        {
            Success = true;
        }

        public bool Success { get; protected set; }

        public string Message { get; }
    }

    public class Result<T> : Result
    {
        /// <summary>
        /// Success constructor with data.
        /// </summary>
        /// <param name="data"></param>
        public Result(T data)
        {
            Data = data;
            Success = true;
        }

        /// <summary>
        /// Non-success constructor with a message.
        /// </summary>
        /// <param name="message"></param>
        public Result(string message) : base(message) { }

        public T Data { get; }
    }
}
