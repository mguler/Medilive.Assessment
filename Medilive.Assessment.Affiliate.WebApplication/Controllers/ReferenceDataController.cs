using Medilive.Assessment.Affiliate.Business.ReferenceDataManagement;
using Medilive.Assessment.Affiliate.Dto;
using Medilive.Assessment.Affiliate.Dto.ReferenceDataManagement;
using Microsoft.AspNetCore.Mvc;

namespace Medilive.Assessment.Affiliate.WebApplication.Controllers
{
    public class ReferenceDataController : Controller
    {
        private readonly ReferenceDataManager _referenceDataManager;
        public ReferenceDataController(ReferenceDataManager referenceDataManager)
        {
            _referenceDataManager= referenceDataManager;
        }
        public IActionResult GetGenderList()
        {
            var response = new Response<ReferenceDataDto[]>();
            response.Data = _referenceDataManager.GetGenderList();
            response.IsSuccessful = true;
            return Json(response);
        }
    }
}
