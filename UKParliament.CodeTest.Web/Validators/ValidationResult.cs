namespace UKParliament.CodeTest.Web.Validators;

public class ValidationResult
{
    public bool IsValid => Errors.Count == 0;
    public List<string> Errors { get; } = [];
}
