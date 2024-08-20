using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoProject.models;
using TodoProject.Models;
using TodoProject.Services;

namespace TodoProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommentController : Controller
    {
        public IOptions<TodoDatabaseSettings> dbSettings { get; }

        private CommentService commentService;

        public CommentController(IOptions<TodoDatabaseSettings> dbSettings, AppDbService appDb, CommentService commentService)
        {
            this.dbSettings = dbSettings;
            this.commentService = commentService;
        }

        [HttpPut]
        [Route("{cardId}")]
        public async Task<IActionResult> PostComment([FromRoute] string cardId, string content)
        {
            return Ok(await commentService.PostComment(cardId, content));
        }
    }
}
