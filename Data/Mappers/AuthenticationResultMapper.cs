namespace RazorBlog.Data.Mappers
{
    using RazorBlog.Services.Communications;
    public static class AuthenticationResultMapper
    {
        public static Result Error(ServiceCode code, string error = "An error occured.")
        {
            return new Result()
            {
                Code = code,
                ErrorMessage = error,
            };
        }

        public static Result Success()
        {
            return new Result()
            {
                Code = ServiceCode.Success,
            };
        }
    }
}
