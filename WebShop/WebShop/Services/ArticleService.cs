using AutoMapper;
using WebShop.DBConfiguration;
using WebShop.DTO;
using WebShop.Interfaces;
using WebShop.Models;

namespace WebShop.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IMapper _mapper;
        private readonly WebShopDbContext _webShopDbContext;

        public ArticleService(IMapper mapper, WebShopDbContext webShopDbContext)
        {
            _mapper = mapper;
            _webShopDbContext = webShopDbContext;
        }

        public ArticleDTO AddNewArticle(ArticleDTO newArticle)
        {
            var article = _webShopDbContext.Articles.FirstOrDefault(a => a.Name == newArticle.Name);
            if (article != null) { return null; }

            _webShopDbContext.Articles.Add(_mapper.Map<Article>(newArticle));
            _webShopDbContext.SaveChanges();
            return _mapper.Map<ArticleDTO>(newArticle);
        }

        public bool DeleteArticle(long id)
        {
            var article = _webShopDbContext.Articles.Find(id);
            if (article == null) { return false; }
            else
            {
                _webShopDbContext.Articles.Remove(article);
                _webShopDbContext.SaveChanges();
                return true;
            }
        }

        public List<ArticleDTO> GetAllArticles()
        {
            return _mapper.Map<List<ArticleDTO>>(_webShopDbContext.Articles);
        }

        public ArticleDTO GetArticle(long id)
        {
            var article = _webShopDbContext.Articles.Find(id);
            if(article == null) { return null; }
            else return _mapper.Map<ArticleDTO>(article);
        }

        public ArticleDTO UpdateArticle(long id, ArticleDTO newArticle)
        {
            var article = _webShopDbContext.Articles.Find(id);
            if (article == null) { return null; }
            else
            {
                article.Name = newArticle.Name;
                article.Description = newArticle.Description;
                article.Price = newArticle.Price;
                article.MaxQuantity = newArticle.MaxQuantity;
                article.PhotoUrl = newArticle.PhotoUrl;

                _webShopDbContext.SaveChanges();
                return _mapper.Map<ArticleDTO>(article);
            }

        }
    }
}
