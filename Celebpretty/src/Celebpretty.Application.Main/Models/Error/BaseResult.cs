namespace Celebpretty.Application.Main.Models.Error;

public class BaseResult
{
    public ErrorCode? ErrorCode { get; init; }
    public bool IsSuccess { get => ErrorCode is null; }
}
