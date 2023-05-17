using WebShop.DTO;

namespace WebShop.Interfaces
{
    public interface IArticleService
    {
        Task<List<ArticleDTO>> GetAllArticles();
        Task<ArticleDTO> GetArticle(long id);
        Task<ArticleDTO> AddNewArticle(ArticleDTO newArticle, long sellerId);
        Task<bool> DeleteArticle(long id);
        Task<ArticleDTO> UpdateArticle(long id, ArticleDTO newArticle);
    }
}
