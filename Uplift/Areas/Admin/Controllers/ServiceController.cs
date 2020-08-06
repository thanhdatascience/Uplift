using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;
using Uplift.Models.ViewModel;

namespace Uplift.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        [BindProperty]
        public ServiceViewModel serviceViewModel { get; set; }
        public ServiceController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            serviceViewModel = new ServiceViewModel()
            {
                Service = new Models.Service(),
                CategoryList = _unitOfWork.Category.GetCategoryListForDropDown(),
                FrequencyList = _unitOfWork.Frequency.GetFrequencyListForDropDown(),
            };
            if (id != null)
            {
                serviceViewModel.Service = _unitOfWork.Service.Get(id.GetValueOrDefault());
            }

            return View(serviceViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (serviceViewModel.Service.Id == 0)
                {
                    //New Service
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\services");
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    serviceViewModel.Service.ImageUrl = @"\images\services\" + fileName + extension;

                    _unitOfWork.Service.Add(serviceViewModel.Service);
                }
                else
                {
                    //Edit Service
                    var serviceFromDb = _unitOfWork.Service.Get(serviceViewModel.Service.Id);
                    if (files.Count > 0)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(webRootPath, @"images\services");
                        var extension_new = Path.GetExtension(files[0].FileName);

                        var imagePath = Path.Combine(webRootPath, serviceFromDb.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }

                        using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension_new), FileMode.Create))
                        {
                            files[0].CopyTo(fileStreams);
                        }
                        serviceViewModel.Service.ImageUrl = @"\images\services\" + fileName + extension_new;
                    }
                    else
                    {
                        serviceViewModel.Service.ImageUrl = serviceFromDb.ImageUrl;
                    }

                    _unitOfWork.Service.Update(serviceViewModel.Service);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                serviceViewModel.CategoryList = _unitOfWork.Category.GetCategoryListForDropDown();
                serviceViewModel.FrequencyList = _unitOfWork.Frequency.GetFrequencyListForDropDown();
                return View(serviceViewModel);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var serviceFromDb = _unitOfWork.Service.Get(id);
            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, serviceFromDb.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            if (serviceFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting!" });
            }
            _unitOfWork.Service.Remove(serviceFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successfully!" });
        }

        #region API CALL
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Service.GetAll(includeProperties:"Category,Frequency") });
        }
        #endregion
    }
}