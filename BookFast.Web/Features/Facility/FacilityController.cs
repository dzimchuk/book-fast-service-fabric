using System.Threading.Tasks;
using BookFast.Web.Contracts;
using BookFast.Web.Contracts.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BookFast.Web.Features.Facility.ViewModels;

namespace BookFast.Web.Features.Facility
{
    [Authorize(Policy = "FacilityProviderOnly")]
    public class FacilityController : Controller
    {
        private readonly IFacilityProxy facilityService;
        private readonly FacilityMapper facilityMapper;

        private readonly IAccommodationProxy accommodationService;
        private readonly AccommodationMapper accommodationMapper;

        public FacilityController(IFacilityProxy facilityService, FacilityMapper facilityMapper, 
            IAccommodationProxy accommodationService, AccommodationMapper accommodationMapper)
        {
            this.facilityService = facilityService;
            this.facilityMapper = facilityMapper;
            this.accommodationService = accommodationService;
            this.accommodationMapper = accommodationMapper;
        }

        public async Task<IActionResult> Index()
        {
            var facilities = await facilityService.ListAsync();
            return View(facilityMapper.MapFrom(facilities));
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            try
            {
                var facility = await facilityService.FindAsync(id.Value);
                var accommodations = await accommodationService.ListAsync(facility.Id);
                return View(new FacilityDetailsViewModel
                            {
                                Facility = facilityMapper.MapFrom(facility),
                                Accommodations = accommodationMapper.MapFrom(accommodations)
                            });
            }
            catch (FacilityNotFoundException)
            {
                return View("NotFound");
            }
        }
        
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FacilityViewModel facilityViewModel)
        {
            if (ModelState.IsValid)
            {
                var details = facilityMapper.MapFrom(facilityViewModel);

                await facilityService.CreateAsync(details);
                return RedirectToAction("Index");
            }
            return View(facilityViewModel);
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            try
            {
                var facility = await facilityService.FindAsync(id.Value);
                return View(facilityMapper.MapFrom(facility));
            }
            catch (FacilityNotFoundException)
            {
                return View("NotFound");
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FacilityViewModel facilityViewModel)
        {
            if (ModelState.IsValid)
            {
                var details = facilityMapper.MapFrom(facilityViewModel);

                try
                {
                    await facilityService.UpdateAsync(facilityViewModel.Id, details);
                    return RedirectToAction("Index");
                }
                catch (FacilityNotFoundException)
                {
                    return View("NotFound");
                }
            }

            return View(facilityViewModel);
        }
        
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            try
            {
                var facility = await facilityService.FindAsync(id.Value);
                return View(facilityMapper.MapFrom(facility));
            }
            catch (FacilityNotFoundException)
            {
                return View("NotFound");
            }
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await facilityService.DeleteAsync(id);
                return RedirectToAction("Index");
            }
            catch (FacilityNotFoundException)
            {
                return View("NotFound");
            }
        }
    }
}
