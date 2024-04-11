namespace My.LightAuthorizationService.Dto
{
    public class IdentResponse<T>
    {
        public T? Data { get; set; }

        public bool Success { get; init; }

        public string Message { get; init; } = default!;
    }
}
