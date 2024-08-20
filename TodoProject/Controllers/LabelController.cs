using Microsoft.AspNetCore.Mvc;
using TodoProject.Models.DTOs;
using TodoProject.Services;

namespace TodoProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LabelController : Controller
    {
        private readonly LabelService labelService;

        public LabelController(LabelService labelService)
        {
            this.labelService = labelService;
        }

        [HttpPost]
        [Route("get_list")]
        public async Task<IActionResult> GetList(FilterParams _params)
        {
            return Ok(await labelService.GetList(_params));
        }

        [HttpPost]
        public async Task<IActionResult> Create(UpdateLabelRequest body)
        {
            return Ok(await labelService.Create(body));
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, UpdateLabelRequest body)
        {
            return Ok(await labelService.Update(id, body));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            return Ok(await labelService.Delete(id));
        }
    }
}
