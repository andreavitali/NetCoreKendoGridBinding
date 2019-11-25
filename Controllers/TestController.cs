using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Artech.AspNetCore.Kendo;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace NetCoreKendoAngularGridBinding.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        readonly MyDbContext _dbContext;
        readonly IMapper _mapper;

        public TestController(MyDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet("dto")]
        public ActionResult GetDTO(KendoDataSourceRequest request)
        {
            var response = _dbContext.Set<Employee>().Include(e => e.Department).ToDataSourceResult<Employee, EmployeeDTO>(request, _mapper);
            return Ok(response);
        }

        [HttpGet]
        public ActionResult Get(KendoDataSourceRequest request)
        {
            var response = _dbContext.Set<Employee>().Include(e => e.Department).ToDataSourceResult(request);
            return Ok(response);
        }
    }
}
