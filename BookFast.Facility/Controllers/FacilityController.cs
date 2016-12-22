using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Facility.Models;
using BookFast.Facility.Models.Representations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using BookFast.Facility.Contracts;
using BookFast.Facility.Contracts.Exceptions;

namespace BookFast.Facility.Controllers
{
    [Authorize(Policy = "Facility.Write")]
    [SwaggerResponseRemoveDefaults]
    public class FacilityController : Controller
    {
        private readonly IFacilityService service;
        private readonly IFacilityMapper mapper;

        public FacilityController(IFacilityService service, IFacilityMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        /// <summary>
        /// List facilities by owner
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/facilities")]
        [SwaggerOperation("list-facilities")]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, Type = typeof(IEnumerable<FacilityRepresentation>))]
        public async Task<IEnumerable<FacilityRepresentation>> List()
        {
            var facilities = await service.ListAsync();
            return mapper.MapFrom(facilities);
        }
        
        /// <summary>
        /// Find facility by ID
        /// </summary>
        /// <param name="id">Facility ID</param>
        /// <returns></returns>
        [HttpGet("/api/facilities/{id}")]
        [SwaggerOperation("find-facility")]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, Type = typeof(FacilityRepresentation))]
        [SwaggerResponse(System.Net.HttpStatusCode.NotFound, Description = "Facility not found")]
        [AllowAnonymous]
        public async Task<IActionResult> Find(Guid id)
        {
            try
            {
                var facility = await service.FindAsync(id);
                return Ok(mapper.MapFrom(facility));
            }
            catch (FacilityNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new facility
        /// </summary>
        /// <param name="facilityData">Facility details</param>
        /// <returns></returns>
        [HttpPost("api/facilities")]
        [SwaggerOperation("create-facility")]
        [SwaggerResponse(System.Net.HttpStatusCode.Created, Type = typeof(FacilityRepresentation))]
        [SwaggerResponse(System.Net.HttpStatusCode.BadRequest, Description = "Invalid parameters")]
        public async Task<IActionResult> Create([FromBody]FacilityData facilityData)
        {
            if (ModelState.IsValid)
            {
                var facility = await service.CreateAsync(mapper.MapFrom(facilityData));
                return CreatedAtAction("Find", new { id = facility.Id }, mapper.MapFrom(facility));
            }

            return BadRequest();
        }
        
        /// <summary>
        /// Update facility
        /// </summary>
        /// <param name="id">Facility ID</param>
        /// <param name="facilityData">Facility details</param>
        /// <returns></returns>
        [HttpPut("api/facilities/{id}")]
        [SwaggerOperation("update-facility")]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, Type = typeof(FacilityRepresentation))]
        [SwaggerResponse(System.Net.HttpStatusCode.BadRequest, Description = "Invalid parameters")]
        [SwaggerResponse(System.Net.HttpStatusCode.NotFound, Description = "Facility not found")]
        public async Task<IActionResult> Update(Guid id, [FromBody]FacilityData facilityData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var facility = await service.UpdateAsync(id, mapper.MapFrom(facilityData));
                    return Ok(mapper.MapFrom(facility));
                }

                return BadRequest();
            }
            catch (FacilityNotFoundException)
            {
                return NotFound();
            }
        }
        
        /// <summary>
        /// Delete facility
        /// </summary>
        /// <param name="id">Facility ID</param>
        /// <returns></returns>
        [HttpDelete("api/facilities/{id}")]
        [SwaggerOperation("delete-facility")]
        [SwaggerResponse(System.Net.HttpStatusCode.NoContent)]
        [SwaggerResponse(System.Net.HttpStatusCode.NotFound, Description = "Facility not found")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await service.DeleteAsync(id);
                return new NoContentResult();
            }
            catch (FacilityNotFoundException)
            {
                return NotFound();
            }
        }
    }
}