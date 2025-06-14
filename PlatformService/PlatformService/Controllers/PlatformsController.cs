using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _platform;
        private readonly IMapper _mapper;
        private readonly ICommanDataClient _commanDataClient;

        public PlatformsController(
            IPlatformRepo platform,
            IMapper mapper,
            ICommanDataClient commanDataClient
            )
        {
            _platform = platform;
            _mapper = mapper;
            _commanDataClient = commanDataClient;
        }


        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            var pllatformItems = _platform.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(pllatformItems));
        }


        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int Id)
        {
            var pllatformItem = _platform.GetPlatformById(Id);
            if (pllatformItem == null) return NotFound();
            return Ok(_mapper.Map<PlatformReadDto>(pllatformItem));
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid Request");
            var platformModel = _mapper.Map<Platform>(platformCreateDto);

            _platform.CreatePlatform(platformModel);
            _platform.SaveChanges();

            var platformToReturn = _mapper.Map<PlatformReadDto>(platformModel);

            try
            {
                await _commanDataClient.SendPlatformToCommand(platformToReturn);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"---> Could not send synchronously : {ex.Message}");
            }
            return CreatedAtRoute(nameof(GetPlatformById),new {id = platformToReturn.Id}, platformToReturn);
        }

    }
}
