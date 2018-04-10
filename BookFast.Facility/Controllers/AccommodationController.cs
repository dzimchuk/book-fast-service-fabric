using BookFast.Facility.CommandStack.Commands;
using BookFast.Facility.Domain.Exceptions;
using BookFast.Facility.QueryStack;
using BookFast.Facility.QueryStack.Representations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookFast.Facility.Controllers
{
    [Authorize(Policy = "Facility.Write")]
    public class AccommodationController : Controller
    {
        private readonly IAccommodationQueryDataSource queryDataSource;
        private readonly IMediator mediator;

        public AccommodationController(IAccommodationQueryDataSource queryDataSource, IMediator mediator)
        {
            this.queryDataSource = queryDataSource;
            this.mediator = mediator;
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
        public async Task<IActionResult> List(int facilityId)
        {
            try
            {
                var accommodations = await queryDataSource.ListAsync(facilityId);
                return Ok(accommodations);
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
        public async Task<IActionResult> Find(int id)
        {
            try
            {
                var accommodation = await queryDataSource.FindAsync(id);
                if (accommodation == null)
                {
                    throw new AccommodationNotFoundException(id);
                }

                return Ok(accommodation);
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
        [SwaggerResponse((int)System.Net.HttpStatusCode.Created)]
        [SwaggerResponse((int)System.Net.HttpStatusCode.BadRequest, Description = "Invalid parameters")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NotFound, Description = "Facility not found")]
        public async Task<IActionResult> Create([FromRoute]int facilityId, [FromBody]CreateAccommodationCommand accommodationData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    accommodationData.FacilityId = facilityId;
                    var accommodationId = await mediator.Send(accommodationData);
                    return CreatedAtAction("Find", new { id = accommodationId });
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
        [SwaggerResponse((int)System.Net.HttpStatusCode.NoContent)]
        [SwaggerResponse((int)System.Net.HttpStatusCode.BadRequest, Description = "Invalid parameters")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NotFound, Description = "Facility not found, Accommodation not found")]
        public async Task<IActionResult> Update(int id, [FromBody]UpdateAccommodationCommand accommodationData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    accommodationData.AccommodationId = id;
                    await mediator.Send(accommodationData);
                    return NoContent();
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await mediator.Send(new DeleteAccommodationCommand { AccommodationId = id });
                return NoContent();
            }
            catch (AccommodationNotFoundException)
            {
                return NotFound();
            }
        }
    }
}