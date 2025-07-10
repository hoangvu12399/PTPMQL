namespace DemoMVC.Models.Entities
{
    public class Employee : Person
    {
        public required string EmployeeId { get; set; }
        public required int Age { get; set; }
    }
}
