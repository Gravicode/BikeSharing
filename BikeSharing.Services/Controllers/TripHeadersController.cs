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
    public class TripHeaderController : Controller
    {
        private readonly BikeSharingDB _context;

        public TripHeaderController(BikeSharingDB context)
        {
            _context = context;
        }

        // GET: api/AppLogs
        [HttpGet("[action]")]
        //[Route("GetTripheader")]
        public async Task<IActionResult> GetTripheader()
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                hasil.Data = _context.TripHeaders.ToList();
            }
            catch (Exception ex)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = ex.Message;
            }
            return Ok(hasil);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTripHeader([FromRoute] long id)
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

                    var headerTrip = await _context.TripHeaders.FindAsync(id);

                    if (headerTrip == null)
                    {
                        hasil.IsSucceed = false;
                        hasil.ErrorMessage = "not found";
                    }
                    else
                    {
                        hasil.Data = headerTrip;
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
        public async Task<IActionResult> PutTripHeader([FromRoute] long id, [FromBody] TripHeader tripHeader)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tripHeader.Id)
            {
                return BadRequest();
            }

            _context.Entry(tripHeader).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripHeaderExists(id))
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
        [Route("PostTripHeaders")]
        public async Task<IActionResult> PostTripHeader([FromBody] TripHeader tripHeader)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TripHeaders.Add(tripHeader);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTripHeader", new { id = tripHeader.Id }, tripHeader);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTripHeader([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tripHeader = await _context.TripHeaders.FindAsync(id);
            if (tripHeader == null)
            {
                return NotFound();
            }

            _context.TripHeaders.Remove(tripHeader);
            await _context.SaveChangesAsync();

            return Ok(tripHeader);
        }

        private bool TripHeaderExists(long id)
        {
            return _context.TripHeaders.Any(e => e.Id == id);
        }

    }
}
