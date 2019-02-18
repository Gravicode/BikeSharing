using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BikeSharing.Models;
using BikeSharing.Service.Helpers;

namespace BikeSharing.Service.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    //[Authorize]
    public class BikeStationsController : Controller
    {
        private readonly BikeSharingDB _context;

        public BikeStationsController(BikeSharingDB context)
        {
            _context = context;
        }

        // GET: api/AppLogs
        [HttpGet("action")]
        //[Route("GetBikeStations")]
        public async Task<IActionResult> GetBikeStations()
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                hasil.Data = _context.BikeStations.ToList();
            }
            catch (Exception ex)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = ex.Message;
            }
            return Ok(hasil);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBikeStations([FromRoute] long id)
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                if (!ModelState.IsValid)
                {
                    hasil.IsSucceed = false;
                    hasil.ErrorMessage = "state is not valid";
                }
                else
                {

                    var bikestation = await _context.BikeStations.FindAsync(id);

                    if (bikestation == null)
                    {
                        hasil.IsSucceed = false;
                        hasil.ErrorMessage = "not found";
                    }
                    else
                    {
                        hasil.Data = bikestation;
                    }
                }
            }
            catch (Exception ex)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = ex.Message;
            }
            return Ok(hasil);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBikeStations([FromRoute] long id, [FromBody] BikeStation BikeStations)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != BikeStations.Id)
            {
                return BadRequest();
            }

            _context.Entry(BikeStations).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BikeStationsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostBikeStations([FromBody] BikeStation BikeStations)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.BikeStations.Add(BikeStations);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBikeStation", new { id = BikeStations.Id }, BikeStations);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMarket([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var BikeStations = await _context.BikeStations.FindAsync(id);
            if (BikeStations == null)
            {
                return NotFound();
            }

            _context.BikeStations.Remove(BikeStations);
            await _context.SaveChangesAsync();

            return Ok(BikeStations);
        }

        private bool BikeStationsExists(long id)
        {
            return _context.BikeStations.Any(e => e.Id == id);
        }
    }
}
