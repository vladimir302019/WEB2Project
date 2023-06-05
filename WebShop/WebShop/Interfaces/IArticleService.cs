using WebShop.DTO.ArticleDTOs;
using WebShop.DTO.UserDTOs;

namespace WebShop.Interfaces
{
    public interface IArticleService
    {
        Task<List<ArticleGetDTO>> GetAllArticles();
        Task<List<ArticleGetDTO>> GetSellerArticles(long id);
        Task<ArticleDTO> GetArticle(long id, long userId);
        Task<ArticleDTO> AddNewArticle(ArticleDTO newArticle, long sellerId);
        Task<bool> DeleteArticle(long id);
        Task<ArticleUpdateDTO> UpdateArticle(long id, ArticleUpdateDTO newArticle);
        Task UploadImage(long id, IFormFile file);
        Task<ArticleImageDTO> GetArticleImage(long id);
    }
}
