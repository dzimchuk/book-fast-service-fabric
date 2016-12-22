using BookFast.Web.Contracts;
using BookFast.Web.Contracts.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BookFast.Web.Controllers
{
    [Authorize(Policy = "FacilityProviderOnly")]
    public class FileAccessController : Controller
    {
        private readonly IFileAccessProxy proxy;
        private readonly IFileAccessMapper mapper;

        public FileAccessController(IFileAccessProxy proxy, IFileAccessMapper mapper)
        {
            this.proxy = proxy;
            this.mapper = mapper;
        }
        
        [HttpGet("/api/facilities/{id}/image-token")]
        public async Task<IActionResult> GetFacilityImageUploadToken(Guid id, [FromQuery]string originalFileName)
        {
            try
            {
                if (string.IsNullOrEmpty(originalFileName))
                    throw new ArgumentNullException(nameof(originalFileName));

                var token = await proxy.IssueFacilityImageUploadTokenAsync(id, originalFileName);
                return Ok(mapper.MapFrom(token));
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
            catch (FacilityNotFoundException)
            {
                return NotFound();
            }
        }
        
        [HttpGet("/api/accommodations/{id}/image-token")]
        public async Task<IActionResult> GetAccommodationImageUploadToken(Guid id, [FromQuery]string originalFileName)
        {
            try
            {
                if (string.IsNullOrEmpty(originalFileName))
                    throw new ArgumentNullException(nameof(originalFileName));

                var token = await proxy.IssueAccommodationImageUploadTokenAsync(id, originalFileName);
                return Ok(token);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
            catch (AccommodationNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
