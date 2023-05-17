using WebShop.Models;

namespace WebShop.Repositories.IRepositories
{
    public interface IArticleRepository
    {
        Task<List<Article>> GetAll();
        Task<Article> GetById(long id);
        Task<Article> InsertArticle(Article article);
        void DeleteArticle(long id);
        void UpdateArticle(Article article);
    }
}
