using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShop.DTO.ArticleDTOs;
using WebShop.DTO.UserDTOs;
using WebShop.Interfaces;
using WebShop.Repositories.IRepositories;

namespace WebShop.Controllers
{
    [Route("api/article")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IUserService _userService;

        public ArticleController(IArticleService articleService, IUserService userService)
        {
            _articleService = articleService;
            _userService = userService;
        }

        [HttpGet("get")]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery] long id)
        {
            return Ok(await _articleService.GetArticle(id, _userService.GetUserIdFromToken(User)));
        }

        [HttpGet("get-all")]
        [Authorize(Roles = "Buyer, Admin")]
        public async Task<IActionResult> GetAll()
        {
            List<ArticleDTO> articleDTOs = await _articleService.GetAllArticles();
            return Ok(articleDTOs);
        }

        [HttpPut("new-article")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> NewArticle([FromBody] ArticleDTO articleDTO)
        {
            return Ok(await _articleService.AddNewArticle(articleDTO, _userService.GetUserIdFromToken(User)));
        }

        [HttpPut("update-article")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> UpdateArticle([FromBody] ArticleUpdateDTO articleDTO)
        {
            return Ok(await _articleService.UpdateArticle(articleDTO.Id, articleDTO));
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> DeleteArticle([FromBody] long id)
        {
            return Ok(await _articleService.DeleteArticle(id));
        }

        [HttpGet("seller-get")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> GetSellerArticles()
        {
            return Ok(await _articleService.GetSellerArticles(_userService.GetUserIdFromToken(User)));
        }

        [HttpPut("upload-image")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> UploadImage(long id,IFormFile file)
        {
            await _articleService.UploadImage(id, file);
            return Ok();
        }
    }
}
