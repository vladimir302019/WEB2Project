using WebShop.DTO;

namespace WebShop.Interfaces
{
    public interface IArticleService
    {
        List<ArticleDTO> GetAllArticles();
        ArticleDTO GetArticle(long id);
        ArticleDTO AddNewArticle(ArticleDTO newArticle);
        bool DeleteArticle(long id);
        ArticleDTO UpdateArticle(long id, ArticleDTO newArticle);
    }
}
