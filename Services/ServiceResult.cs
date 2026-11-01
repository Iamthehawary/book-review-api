namespace BookReviewApi.Services;

public enum ResultStatus
{
    Success,
    NotFound,
    Forbidden
}

public record ServiceResult<T>(ResultStatus Status, T? Data = default)
{
    public bool Succeeded => Status == ResultStatus.Success;

    public static ServiceResult<T> Ok(T data) => new(ResultStatus.Success, data);
    public static ServiceResult<T> NotFound() => new(ResultStatus.NotFound);
    public static ServiceResult<T> Forbidden() => new(ResultStatus.Forbidden);
}