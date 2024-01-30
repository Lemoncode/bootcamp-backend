namespace SingleResponsibilityPrinciple.Entities
{
    internal class Employee
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Address { get; set; }

        public Task Save()
        {
            _employees.Add(this);
            return Task.CompletedTask;
        }

        public Task Delete()
        {
            _employees.Remove(this);
            return Task.CompletedTask;
        }

        public Task Update() => Task.CompletedTask;

        public override string ToString()
        {
            return $@"Id: {Id}.
First name: {FirstName}.
Last name: {LastName}.
Address: {Address}.";
        }

        private static List<Employee> _employees = new List<Employee>();

        internal static void Add(Employee newEmployee)
        {
            _employees.Add(newEmployee);
        }

        internal static Employee? Find(int employeeId)
        {
            return _employees.SingleOrDefault(e => e.Id == employeeId);
        }

        internal static List<Employee> GetAllEmployees() => _employees;
    }
}
