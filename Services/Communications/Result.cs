namespace RazorBlog.Services.Communications
{
    /// <summary>
    /// Result object containing object of type <typeparamref name="TData"/> on success and error of type <typeparamref name="TError"/> on failure
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TError"></typeparam>
    public record Result<TData, TError>
    {
        public bool Succeeded => Code == ServiceCode.Success;
        public ServiceCode Code { get; init; }
        /// <summary>
        /// Data returned from the service.
        /// </summary>
        public TData Data { get; init; }
        public TError Error { get; init; }
    }
}