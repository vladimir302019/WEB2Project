using AutoMapper;
using WebShop.DTO.ArticleDTOs;
using WebShop.Interfaces;
using WebShop.Models;
using WebShop.Repositories.IRepositories;

namespace WebShop.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ArticleService(IMapper mapper, IUnitOfWork unit)
        {
            _mapper = mapper;
            _unitOfWork = unit;
        }

        public async Task<ArticleDTO> AddNewArticle(ArticleDTO newArticle, long sellerId)
        {
            List<Article> list = await _unitOfWork.ArticleRepository.GetAll();
            var article = list.Find(a => a.Name == newArticle.Name);
            if (article != null) { return null; }

            var seller = await _unitOfWork.UserRepository.GetById(sellerId);
            if (seller == null) { return null; }

            article = _mapper.Map<Article>(newArticle);
            article.Seller = seller;
            article.SellerId = sellerId;
            //seller.Articles.Add(article);

            _unitOfWork.UserRepository.UpdateUser(seller);
            await _unitOfWork.ArticleRepository.InsertArticle(article);
            await _unitOfWork.Save();
            return _mapper.Map<ArticleDTO>(article);
        }

        public async Task<bool> DeleteArticle(long id)
        {
            var article = await _unitOfWork.ArticleRepository.GetById(id);
            if (article == null) { return false; }
            else
            {
                var seller = await _unitOfWork.UserRepository.GetById(article.SellerId);
                //seller.Articles.Remove(article);
                _unitOfWork.UserRepository.UpdateUser(seller);

                _unitOfWork.ArticleRepository.DeleteArticle(article.Id);
                await _unitOfWork.Save();
                return true;
            }
        }

        public async Task<List<ArticleDTO>> GetAllArticles()
        {
            return _mapper.Map<List<ArticleDTO>>(await _unitOfWork.ArticleRepository.GetAll());
        }

        public async Task<ArticleDTO> GetArticle(long id)
        {
            var article = await _unitOfWork.ArticleRepository.GetById(id);
            if (article == null) { return null; }
            else return _mapper.Map<ArticleDTO>(article);
        }

        public async Task<List<ArticleDTO>> GetSellerArticles(long id)
        {
            var seller = await _unitOfWork.UserRepository.GetById(id);
            if (seller == null) { return null; }
            else return _mapper.Map<List<ArticleDTO>>(await _unitOfWork.ArticleRepository.GetSellerArticles(id));
        }

        public async Task<ArticleUpdateDTO> UpdateArticle(long id, ArticleUpdateDTO newArticle)
        {
            var article = await _unitOfWork.ArticleRepository.GetById(id);
            if (article == null) { return null; }

            string name = article.Name;
            long ids = article.SellerId;
            article = _mapper.Map<Article>(newArticle);
            article.Name = name;
            article.SellerId = ids;
            
            _unitOfWork.ArticleRepository.UpdateArticle(article);
            await _unitOfWork.Save();
            return _mapper.Map<ArticleUpdateDTO>(article);
        }
    }
}
