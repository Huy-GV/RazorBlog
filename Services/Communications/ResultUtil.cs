namespace RazorBlog.Services.Communications
{
    public static class ResultUtil
    {
        /// <summary>
        /// Returns a success result with a default error object and data of type <typeparamref name="Empty"></typeparamref>
        /// </summary>
        /// <returns></returns>
        public static Result<Empty, Error> Success()
        {
            return new Result<Empty, Error>
            {
                Code = ServiceCode.Success,
                Data = new(),
            };
        }

        /// <summary>
        /// Returns a success result with a default empty error object and data of type <typeparamref name="TData">TData</typeparamref>
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Result<TData, Error> Success<TData>(TData data)
        {
            return new Result<TData, Error>
            {
                Code = ServiceCode.Success,
                Data = data,
            };
        }

        public static Result<Empty, Error> Failure(ServiceCode code, string message = "An error occurred.")
        {
            return new Result<Empty, Error>
            {
                Code = code,
                Data = new(),
                Error = new Error(message),
            };
        }

        /// <summary>
        /// Return an error result with the default Error object.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Result<TData, Error> Failure<TData>(ServiceCode code, string message = "An error occurred.")
        {
            return new Result<TData, Error>
            {
                Code = code,
                Data = default!,
                Error = new Error(message),
            };
        }

        /// <summary>
        /// Return an error result with a custom Error object.
        /// </summary>
        /// <typeparam name="TError"></typeparam>
        /// <param name="code"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static Result<TData, TError> Failure<TData, TError>(ServiceCode code, TError error)
        {
            return new Result<TData, TError>
            {
                Code = code,
                Data = default!,
                Error = error,
            };
        }
    }
}