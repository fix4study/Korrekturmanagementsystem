namespace Korrekturmanagementsystem.Shared;

public class RoleResolutionResult
{
    public bool IsSuccess { get; set; }
    public Guid? SystemRoleId { get; set; }
    public string? ErrorMessage { get; set; }

    public static RoleResolutionResult Success(Guid id) => new() { IsSuccess = true, SystemRoleId = id };
    public static RoleResolutionResult Failure(string error) => new() { IsSuccess = false, ErrorMessage = error };
}
