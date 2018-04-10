using BookFast.Files.Contracts;
using BookFast.Files.Contracts.Exceptions;
using BookFast.Files.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Threading.Tasks;

namespace BookFast.Files.Controllers
{
    [Authorize(Policy = "Facility.Write")]
    public class FileAccessController : Controller
    {
        private readonly IFileAccessService fileService;
        private readonly IFileAccessMapper mapper;

        public FileAccessController(IFileAccessService fileService, IFileAccessMapper mapper)
        {
            this.fileService = fileService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get a write access token for a new facility image
        /// </summary>
        /// <param name="id">Facility ID</param>
        /// <param name="originalFileName">Image file name</param>
        /// <returns></returns>
        [HttpGet("/api/facilities/{id}/image-token")]
        [SwaggerOperation("get-facility-image-upload-token")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(FileAccessTokenRepresentation))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.BadRequest, Description = "Invalid parameters")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NotFound, Description = "Facility not found")]
        public async Task<IActionResult> GetFacilityImageUploadToken(int id, [FromQuery]string originalFileName)
        {
            try
            {
                if (string.IsNullOrEmpty(originalFileName))
                    throw new ArgumentNullException(nameof(originalFileName));

                var token = await fileService.IssueFacilityImageUploadTokenAsync(id, originalFileName);
                return Ok(mapper.MapFrom(token));
            }
            catch(ArgumentNullException)
            {
                return BadRequest();
            }
            catch (FacilityNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get a write access token for a new accommodation image
        /// </summary>
        /// <param name="id">Accommodation ID</param>
        /// <param name="originalFileName">Image file name</param>
        /// <returns></returns>
        [HttpGet("/api/accommodations/{id}/image-token")]
        [SwaggerOperation("get-accommodation-image-upload-token")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(FileAccessTokenRepresentation))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.BadRequest, Description = "Invalid parameters")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NotFound, Description = "Accommodation not found")]
        public async Task<IActionResult> GetAccommodationImageUploadToken(int id, [FromQuery]string originalFileName)
        {
            try
            {
                if (string.IsNullOrEmpty(originalFileName))
                    throw new ArgumentNullException(nameof(originalFileName));

                var token = await fileService.IssueAccommodationImageUploadTokenAsync(id, originalFileName);
                return Ok(mapper.MapFrom(token));
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
