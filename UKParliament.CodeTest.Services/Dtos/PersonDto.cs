namespace UKParliament.CodeTest.Services.Dtos;
public class PersonDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public DateOnly DateOfBirth { get; set; }
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = "";
    public string Email { get; set; } = "";
}
