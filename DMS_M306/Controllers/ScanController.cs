using DMS_M306.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DMS_M306.Controllers
{
    public class ScanController : Controller
    {
        private IFileRepository _fileRepository;

        public ScanController(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        // GET: Scan
        public ActionResult Index(string ScanString)
        {
            if (ScanString == "" && ScanString != null) return RedirectToAction("Index","Home");
            var spilted = ScanString.Split('-');
            if(spilted.Count() != 2) return RedirectToAction("Index", "Home");

            int FileID = 0;
            int ReleaseNumber = 0;


            if(int.TryParse(spilted[0], out FileID)&& int.TryParse(spilted[1],out ReleaseNumber))
            {
                var file = _fileRepository.Get(x => x.Id == FileID).FirstOrDefault();
                if(file != null)
                {
                    if(ReleaseNumber == 0)
                    {
                        return RedirectToAction("Details", "File", new { @Id = FileID });
                    }
                    if(file.Releases != null)
                    {
                        var release = file.Releases.Where(x => x.ReleaseNumber == ReleaseNumber).FirstOrDefault();
                        if(release != null)
                        {
                            return RedirectToAction("Details", "FileRelease", new { @Id = release.Id });
                        }
                    }

                }
            }
                return RedirectToAction("Index", "Home");
        }
    }
}