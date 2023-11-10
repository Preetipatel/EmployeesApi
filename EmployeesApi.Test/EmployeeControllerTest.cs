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
            _employeeProviderMock.Setup(x => x.GetAllEmployeesAsync()).ReturnsAsync(new List<EmployeeEntity>());
            EmployeesController controller = new EmployeesController(_employeeProviderMock.Object);
            var result = await controller.GetEmployeesAsync();

            //Assert
            Assert.NotNull(result);
            _employeeProviderMock.Verify(x => x.GetAllEmployeesAsync(), Times.Once);
        }

        [Fact]
        public async Task AddEmployee_ReturnsSuccessful()
        {
            EmployeeEntity emp = new EmployeeEntity()
            {
                Id = 123,
                FirstName = "test",
                LastName = "test",
                Email = "test",
                Age = 30
            };
            _employeeProviderMock.Setup(x => x.AddEmployeeAsync(It.IsAny<EmployeeEntity>())).ReturnsAsync(emp);
            EmployeesController controller = new EmployeesController(_employeeProviderMock.Object);

            var result = await controller.AddEmployeeAsync(emp);

            //Assert
            Assert.NotNull(result);
             _employeeProviderMock.Verify(x => x.AddEmployeeAsync(It.IsAny<EmployeeEntity>()), Times.Once);
        }
    }
}