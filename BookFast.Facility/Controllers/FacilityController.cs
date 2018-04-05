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
    public class FacilityController : Controller
    {
        private readonly IFacilityQueryDataSource queryDataSource;
        private readonly IMediator mediator;

        public FacilityController(IFacilityQueryDataSource queryDataSource, IMediator mediator)
        {
            this.queryDataSource = queryDataSource;
            this.mediator = mediator;
        }

        /// <summary>
        /// List facilities by owner
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/facilities")]
        [SwaggerOperation("list-facilities")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(IEnumerable<FacilityRepresentation>))]
        public Task<IEnumerable<FacilityRepresentation>> List()
        {
            return queryDataSource.ListAsync();
        }
        
        /// <summary>
        /// Find facility by ID
        /// </summary>
        /// <param name="id">Facility ID</param>
        /// <returns></returns>
        [HttpGet("/api/facilities/{id}")]
        [SwaggerOperation("find-facility")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(FacilityRepresentation))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NotFound, Description = "Facility not found")]
        [AllowAnonymous]
        public async Task<IActionResult> Find(int id)
        {
            try
            {
                var facility = await queryDataSource.FindAsync(id);
                if (facility == null)
                {
                    throw new FacilityNotFoundException(id);
                }

                return Ok(facility);
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
        [SwaggerResponse((int)System.Net.HttpStatusCode.Created)]
        [SwaggerResponse((int)System.Net.HttpStatusCode.BadRequest, Description = "Invalid parameters")]
        public async Task<IActionResult> Create([FromBody]CreateFacilityCommand facilityData)
        {
            if (ModelState.IsValid)
            {
                var facilityId = await mediator.Send(facilityData);
                return CreatedAtAction("Find", new { id = facilityId }, null);
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
        [SwaggerResponse((int)System.Net.HttpStatusCode.NoContent)]
        [SwaggerResponse((int)System.Net.HttpStatusCode.BadRequest, Description = "Invalid parameters")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NotFound, Description = "Facility not found")]
        public async Task<IActionResult> Update(int id, [FromBody]UpdateFacilityCommand facilityData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    facilityData.FacilityId = id;
                    await mediator.Send(facilityData);

                    return NoContent();
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
        [SwaggerResponse((int)System.Net.HttpStatusCode.NoContent)]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NotFound, Description = "Facility not found")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await mediator.Send(new DeleteFacilityCommand { FacilityId = id });
                return NoContent();
            }
            catch (FacilityNotFoundException)
            {
                return NotFound();
            }
        }
    }
}