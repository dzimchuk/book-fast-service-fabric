using System;
using System.Threading.Tasks;
using BookFast.Web.Contracts;
using BookFast.Web.Contracts.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookFast.Web.Features.Facility.ViewModels;

namespace BookFast.Web.Features.Facility
{
    [Authorize(Policy = "FacilityProviderOnly")]
    public class AccommodationController : Controller
    {
        private readonly IAccommodationProxy service;
        private readonly AccommodationMapper mapper;

        public AccommodationController(IAccommodationProxy service, AccommodationMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
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
                var accommodation = await service.FindAsync(id.Value);
                return View(mapper.MapFrom(accommodation));
            }
            catch (AccommodationNotFoundException)
            {
                return View("NotFound");
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            try
            {
                var accommodation = await service.FindAsync(id.Value);
                return View(mapper.MapFrom(accommodation));
            }
            catch (AccommodationNotFoundException)
            {
                return View("NotFound");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AccommodationViewModel accommodationViewModel)
        {
            if (ModelState.IsValid)
            {
                var details = mapper.MapFrom(accommodationViewModel);
                try
                {
                    await service.UpdateAsync(accommodationViewModel.Id, details);
                    return RedirectToAction("details", "facility", new { id = accommodationViewModel.FacilityId });
                }
                catch (AccommodationNotFoundException)
                {
                    return View("NotFound");
                }
                catch (FacilityNotFoundException)
                {
                    return View("NotFound");
                }
            }

            return View(accommodationViewModel);
        }

        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            ViewBag.FacilityId = id.Value;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id, AccommodationViewModel accommodationViewModel)
        {
            if (ModelState.IsValid)
            {
                var details = mapper.MapFrom(accommodationViewModel);
                try
                {
                    await service.CreateAsync(accommodationViewModel.FacilityId, details);
                    return RedirectToAction("details", "facility", new { id = accommodationViewModel.FacilityId });
                }
                catch (FacilityNotFoundException)
                {
                    return View("NotFound");
                }
            }

            ViewBag.FacilityId = accommodationViewModel.FacilityId;
            return View(accommodationViewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            try
            {
                var accommodation = await service.FindAsync(id.Value);
                return View(mapper.MapFrom(accommodation));
            }
            catch (AccommodationNotFoundException)
            {
                return View("NotFound");
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, [FromForm]int facilityId)
        {
            try
            {
                await service.DeleteAsync(id);
                return RedirectToAction("details", "facility", new { id = facilityId });
            }
            catch (AccommodationNotFoundException)
            {
                return View("NotFound");
            }
        }
    }
}