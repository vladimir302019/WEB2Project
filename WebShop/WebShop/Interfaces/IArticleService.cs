using WebShop.DTO.ArticleDTOs;

namespace WebShop.Interfaces
{
    public interface IArticleService
    {
        Task<List<ArticleDTO>> GetAllArticles();
        Task<List<ArticleDTO>> GetSellerArticles(long id);
        Task<ArticleDTO> GetArticle(long id, long userId);
        Task<ArticleDTO> AddNewArticle(ArticleDTO newArticle, long sellerId);
        Task<bool> DeleteArticle(long id);
        Task<ArticleUpdateDTO> UpdateArticle(long id, ArticleUpdateDTO newArticle);
        Task UploadImage(long id, IFormFile file);
    }
}
