using DMS_M306.Enums;
using DMS_M306.Interfaces;
using DMS_M306.Interfaces.Repositories;
using DMS_M306.Models;
using DMS_M306.Services;
using DMS_M306.Services.Interfaces;
using DMS_M306.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DMS_M306.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IFileRepository _fileRepository;
        private IUnitOfWork _unitOfWork;
        private ICodeService _codeService;

        public HomeController(IFileRepository fileRepo, IUnitOfWork unityOfWork, ICodeService codeService)
        {
            _fileRepository = fileRepo;
            _unitOfWork = unityOfWork;
            _codeService = codeService;
        }


        public ActionResult Index()
        {
            HomeViewModel vm = new HomeViewModel();
            return View(vm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}