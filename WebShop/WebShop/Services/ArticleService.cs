using AutoMapper;
using WebShop.DTO.ArticleDTOs;
using WebShop.ExceptionHandler.Exceptions;
using WebShop.Interfaces;
using WebShop.Models;
using WebShop.Models.Enums;
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
            if (article != null) { throw new ConflictException($"Article with Name: {newArticle.Name} already exists."); }

            var seller = await _unitOfWork.UserRepository.GetById(sellerId);
            if (seller == null) { throw new NotFoundException("User doesn't exist."); }

            if (!seller.Approved)
            {
                throw new ConflictException("Seller isn't approved. Wait for admin approval!");
            }

            article = _mapper.Map<Article>(newArticle);
            article.SellerId = sellerId;
            
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
                _unitOfWork.ArticleRepository.DeleteArticle(article.Id);
                await _unitOfWork.Save();
                return true;
            }
        }

        public async Task<List<ArticleDTO>> GetAllArticles()
        {
            return _mapper.Map<List<ArticleDTO>>(await _unitOfWork.ArticleRepository.GetAll());
        }

        public async Task<ArticleDTO> GetArticle(long id, long userId)
        {
            var article = await _unitOfWork.ArticleRepository.GetById(id);
            if (article == null) { throw new NotFoundException("Article doesn't exist."); }

            User user = await _unitOfWork.UserRepository.GetById(userId);
            if(user.Type == UserType.Buyer)
            {
                return _mapper.Map<ArticleDTO>(article);
            }
            if(user.Type == UserType.Seller && user.Id == article.SellerId) 
            {
                return _mapper.Map<ArticleDTO>(article);
            }
            else
            {
                throw new ConflictException("Article not available.");
            }
        }

        public async Task<List<ArticleDTO>> GetSellerArticles(long id)
        {
            var seller = await _unitOfWork.UserRepository.GetById(id);
            if (seller == null) { throw new NotFoundException("User doesn't exist."); }
            if (!seller.Approved)
            {
                throw new ConflictException("Seller isn't approved. Wait for admin approval!");
            }
            else return _mapper.Map<List<ArticleDTO>>(await _unitOfWork.ArticleRepository.GetSellerArticles(id));
        }

        public async Task<ArticleUpdateDTO> UpdateArticle(long id, ArticleUpdateDTO newArticle)
        {
            var article = await _unitOfWork.ArticleRepository.GetById(id);
            if (article == null) { throw new NotFoundException("Article doesn't exist."); }

            string name = article.Name;
            long ids = article.SellerId;
            article = _mapper.Map<Article>(newArticle);
            article.Name = name;
            article.SellerId = ids;
            
            using(var ms = new MemoryStream())
            {
                newArticle.PhotoUrl.CopyTo(ms);
                var fileBytes = ms.ToArray();

                article.PhotoUrl = fileBytes;
            }

            _unitOfWork.ArticleRepository.UpdateArticle(article);
            await _unitOfWork.Save();
            return _mapper.Map<ArticleUpdateDTO>(article);
        }

        public async Task UploadImage(long id, IFormFile file)
        {
            var article = await _unitOfWork.ArticleRepository.GetById(id);
            if (article == null) { throw new NotFoundException("Article doesn't exist."); }

            using(var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();

                article.PhotoUrl = fileBytes;
                _unitOfWork.ArticleRepository.UpdateArticle(article);
            }
            await _unitOfWork.Save();
        }
    }
}
