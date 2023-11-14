using Employees.Contracts;
using EmployeesApi.Area.V1.Controllers;
using EmployeesApi.Domain;
using EmployeesApi.Provider;
using Moq;

namespace EmployeesApi.Test
{
    public class EmployeeControllerTest
    {
        private readonly Mock<IEmployeeProvider> _employeeProviderMock;
        public EmployeeControllerTest()
        {
            _employeeProviderMock = new Mock<IEmployeeProvider>();
        }

        [Fact]
        public async Task GetAllEmployees_ReturnsEmplyoees()
        {
            _employeeProviderMock.Setup(x => x.GetAllEmployeesAsync()).ReturnsAsync(new List<EmployeeResponse>());
            EmployeesController controller = new EmployeesController(_employeeProviderMock.Object);
            var result = await controller.GetEmployeesAsync();

            //Assert
            Assert.NotNull(result);
            _employeeProviderMock.Verify(x => x.GetAllEmployeesAsync(), Times.Once);
        }

        [Fact]
        public async Task AddEmployee_ReturnsSuccessful()
        {
            var empResponse = new EmployeeResponse()
            {
                Id = Guid.NewGuid(),
                FirstName = "test",
                LastName = "test",
                Email = "test",
                Age = 30
            };
            var empRequest = new AddEmployeeRequest()
            {
                 FirstName = "test",
                LastName = "test",
                Email = "test",
                Age = 30
            };
            _employeeProviderMock.Setup(x => x.AddEmployeeAsync(It.IsAny<AddEmployeeRequest>())).ReturnsAsync(empResponse);
            EmployeesController controller = new EmployeesController(_employeeProviderMock.Object);

            var result = await controller.AddEmployeeAsync(empRequest);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(result.Value.Id, empResponse.Id);
             _employeeProviderMock.Verify(x => x.AddEmployeeAsync(It.IsAny<AddEmployeeRequest>()), Times.Once);
        }
    }
}