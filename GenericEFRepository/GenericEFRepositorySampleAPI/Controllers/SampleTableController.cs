using EFDomainModel;
using GenericEFRepositoryBLL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GenericEFRepositorySampleAPI.Controllers
{
    [Route("api/sample")]
    [ApiController]
    public class SampleTableController : ControllerBase
    {        
        [HttpGet]
        [Route("allsample")]
        public async Task<List<SampleTableDTO>> GetAllSample(string searchText)
        {
            return await SampleManager.GetAllSampleTable(searchText);
        }

        [HttpGet]
        [Route("samplebyid")]
        public async Task<SampleTableDTO> GetSampleTableWithDetailById(long id)
        {
            return await SampleManager.GetSampleTableWithDetailById(id);
        }
    }
}
