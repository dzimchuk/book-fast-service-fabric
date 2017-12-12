using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Facility.Models;
using BookFast.Facility.Models.Representations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookFast.Facility.Contracts;
using BookFast.Facility.Contracts.Exceptions;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BookFast.Facility.Controllers
{
    [Authorize(Policy = "Facility.Write")]
    public class AccommodationController : Controller
    {
        private readonly IAccommodationService service;
        private readonly IAccommodationMapper mapper;

        public AccommodationController(IAccommodationService service, IAccommodationMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        /// <summary>
        /// List accommodations by facility
        /// </summary>
        /// <param name="facilityId">Facility ID</param>
        /// <returns></returns>
        [HttpGet("api/facilities/{facilityId}/accommodations")]
        [SwaggerOperation("list-accommodations")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(IEnumerable<AccommodationRepresentation>))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NotFound, Description = "Facility not found")]
        [AllowAnonymous]
        public async Task<IActionResult> List(Guid facilityId)
        {
            try
            {
                var accommodations = await service.ListAsync(facilityId);
                return Ok(mapper.MapFrom(accommodations));
            }
            catch (FacilityNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Find accommodation by ID
        /// </summary>
        /// <param name="id">Accommodation ID</param>
        /// <returns></returns>
        [HttpGet("api/accommodations/{id}")]
        [SwaggerOperation("find-accommodation")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(AccommodationRepresentation))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NotFound, Description = "Accommodation not found")]
        [AllowAnonymous]
        public async Task<IActionResult> Find(Guid id)
        {
            try
            {
                var accommodation = await service.FindAsync(id);
                return Ok(mapper.MapFrom(accommodation));
            }
            catch (AccommodationNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new accommodation
        /// </summary>
        /// <param name="facilityId">Facility ID</param>
        /// <param name="accommodationData">Accommodation details</param>
        /// <returns></returns>
        [HttpPost("api/facilities/{facilityId}/accommodations")]
        [SwaggerOperation("create-accommodation")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.Created, Type = typeof(AccommodationRepresentation))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.BadRequest, Description = "Invalid parameters")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NotFound, Description = "Facility not found")]
        public async Task<IActionResult> Create([FromRoute]Guid facilityId, [FromBody]AccommodationData accommodationData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var accommodation = await service.CreateAsync(facilityId, mapper.MapFrom(accommodationData));
                    return CreatedAtAction("Find", new { id = accommodation.Id }, mapper.MapFrom(accommodation));
                }

                return BadRequest();
            }
            catch (FacilityNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Update accommodation
        /// </summary>
        /// <param name="id">Accommodation ID</param>
        /// <param name="accommodationData">Accommodation details</param>
        /// <returns></returns>
        [HttpPut("api/accommodations/{id}")]
        [SwaggerOperation("update-accommodation")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(AccommodationRepresentation))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.BadRequest, Description = "Invalid parameters")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NotFound, Description = "Facility not found, Accommodation not found")]
        public async Task<IActionResult> Update(Guid id, [FromBody]AccommodationData accommodationData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var accommodation = await service.UpdateAsync(id, mapper.MapFrom(accommodationData));
                    return Ok(mapper.MapFrom(accommodation));
                }

                return BadRequest();
            }
            catch (FacilityNotFoundException)
            {
                return NotFound();
            }
            catch (AccommodationNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Delete accommodation
        /// </summary>
        /// <param name="id">Accommodation ID</param>
        /// <returns></returns>
        [HttpDelete("api/accommodations/{id}")]
        [SwaggerOperation("delete-accommodation")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NoContent)]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NotFound, Description = "Accommodation not found")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await service.DeleteAsync(id);
                return new NoContentResult();
            }
            catch (AccommodationNotFoundException)
            {
                return NotFound();
            }
        }
    }
}