using DMS_M306.Attributes;
using DMS_M306.Constants;
using DMS_M306.Interfaces;
using DMS_M306.Interfaces.Repositories;
using DMS_M306.Models;
using DMS_M306.ViewModels.Category;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DMS_M306.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly IFileCategoryRepository _fileCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private bool _isDeleteEnabled;

        public CategoryController(IFileCategoryRepository fileCategoryRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _fileCategoryRepository = fileCategoryRepository;
            _isDeleteEnabled = false;
        }

        [HttpGet]
        public ActionResult Index()
        {
            CategoriesViewModel vm = new CategoriesViewModel()
            {
                Categories = _fileCategoryRepository.Get().Select(x => new CategoryViewModel {Id = x.Id , Name = x.Name, Description = x.Description}).ToList()
            };
            return View(vm);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CategoryViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            FileCategory fileCategory = new FileCategory()
            {
                Name = vm.Name,
                Description = vm.Description
            };
            try
            {
                _fileCategoryRepository.Add(fileCategory);
                _unitOfWork.SaveChanges();
                TempData[GlobalConstants.AlertSuccessMessageKey] = GlobalConstants.CategoryCreateSuccess;
            }
            catch
            {
                TempData[GlobalConstants.AlertErrorMessageKey] = GlobalConstants.CategoryCreateError;
            }
            return RedirectToAction("index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (_isDeleteEnabled == false)
            {
                TempData[GlobalConstants.AlertErrorMessageKey] = GlobalConstants.CategoryDeleteDisabledMessage;
                return RedirectToAction("Index");
            }
            var category = _fileCategoryRepository.Get(x => x.Id == id).FirstOrDefault();
            if(category == null)
            {
                return RedirectToAction("Index");
            } 
            CategoryViewModel vm = new CategoryViewModel()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
            return View(vm);
        }

        [HttpPost]
        public ActionResult Delete(CategoryViewModel vm)
        {
            if(_isDeleteEnabled == false)
            {
                TempData[GlobalConstants.AlertErrorMessageKey] = GlobalConstants.CategoryDeleteDisabledMessage;
                return RedirectToAction("Index");
            }
            var category = _fileCategoryRepository.Get(x => x.Id == vm.Id).FirstOrDefault();
            if(category != null)
            {
                _fileCategoryRepository.Delete(category);
                _unitOfWork.SaveChanges();
            }
            TempData[GlobalConstants.AlertSuccessMessageKey] = GlobalConstants.CategoryDeleteSuccess;
            return RedirectToAction("Index");
        }
    }
}