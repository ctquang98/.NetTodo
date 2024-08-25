using Microsoft.AspNetCore.Mvc;
using TodoProject.Attributes;
using TodoProject.Helpers;
using TodoProject.Models.DTOs;
using TodoProject.Services;

namespace TodoProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [CardLogAttribute]
    public class CardController : Controller
    {
        private readonly CardLabelSerivce cardLabelSerivce;
        private readonly CommentService commentService;
        private readonly CardService cardService;

        public CardController(CardService cardService, CardLabelSerivce cardLabelSerivce, CommentService commentService)
        {
            this.cardService = cardService;
            this.cardLabelSerivce = cardLabelSerivce;
            this.commentService = commentService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var card = await cardService.GetById(id);
            if (card == null) return Ok("NotFound");
            card.labels = await cardLabelSerivce.GetListLabelByCard(id);
            card.comments = await commentService.GetListCommentByCardId(id);
            return Ok(card);
        }

        [HttpPost]
        [Route("get_list")]
        public async Task<IActionResult> GetList(FilterParams _params)
        {
            return Ok(await cardService.GetList(_params));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCardRequest body)
        {
            return Ok(await cardService.Create(body));
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, UpdateCardRequest body)
        {
            return Ok(await cardService.Update(id, body));
        }

        [HttpPut]
        [Route("{id}/add_label")]
        public async Task<IActionResult> AddLabel([FromRoute] string id, CardAddLabelRequest body)
        {
            return Ok(await cardLabelSerivce.AddLabel(id, body));
        }

        [HttpPut]
        [Route("{id}/remove_label")]
        public async Task<IActionResult> AddLabel([FromRoute] string id, string labelId)
        {
            return Ok(await cardLabelSerivce.RemoveLabel(id, labelId));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            return Ok(await cardService.Delete(id));
        }
    }
}
