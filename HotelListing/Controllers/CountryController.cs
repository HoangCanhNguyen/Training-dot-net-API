using AutoMapper;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;

        public CountryController(IUnitOfWork unitOfWork, ILogger<CountryController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries([FromQuery] RequestParams requestParams)
        {
            try
            {
                var countries = await _unitOfWork.Countries.GetPagedList(requestParams,null); 
                var countriesDTO = _mapper.Map<List<CountryDTO>>(countries);
                return Ok(countriesDTO);
            } catch (Exception e)
            {
                _logger.LogError(e, "Some thing went wrong");
                return BadRequest();
            }
        }

        [HttpGet("{id:int}", Name = "GetCountry")]
        public async Task<IActionResult> GetCountry(int id)
        {
            try
            {
                var country = await _unitOfWork.Countries.Get(c => c.Id == id, new List<string> { "Hotels" });
                var countryDTO = _mapper.Map<CountryDTO>(country);
                return Ok(countryDTO);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Some thing went wrong");
                return BadRequest();
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDTO countryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var country = _mapper.Map<Country>(countryDTO);
                await _unitOfWork.Countries.Insert(country);
                await _unitOfWork.Save();
                return CreatedAtRoute("GetCountry", new { id = country.Id }, country);
            }
            catch (Exception)
            {
                return StatusCode(500);
                throw;
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCountry(int id, UpdateCountryDTO countryDTO ) {
            if (!ModelState.IsValid || id < 1)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var country = await _unitOfWork.Countries.Get( c => c.Id == id);
                if (country == null)
                {
                    return BadRequest();
                }

                _mapper.Map(countryDTO, country);
                _unitOfWork.Countries.Update(country);
                await _unitOfWork.Save();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCountry(int id) {
            if (id < 1)
            {
                return BadRequest();
            }
            try
            {
                var country = await _unitOfWork.Countries.Get(c => c.Id == id);
                if (country == null)
                {
                    return BadRequest();
                }
                await _unitOfWork.Countries.Delete(id);
                await _unitOfWork.Save();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
