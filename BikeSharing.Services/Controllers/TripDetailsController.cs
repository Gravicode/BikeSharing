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
    public class TripDetailsController : Controller
    {
        private readonly BikeSharingDB _context;

        public TripDetailsController(BikeSharingDB context)
        {
            _context = context;
        }

        // GET: api/AppLogs
        [HttpGet]
        [Route("GetTripDetail")]
        public async Task<IActionResult> GetTripDetail()
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                hasil.Data = _context.TripDetails.ToList();
            }
            catch (Exception ex)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = ex.Message;
            }
            return Ok(hasil);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTripDetail([FromRoute] long id)
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

                    var detailTrip = await _context.TripDetails.FindAsync(id);

                    if (detailTrip == null)
                    {
                        hasil.IsSucceed = false;
                        hasil.ErrorMessage = "not found";
                    }
                    else
                    {
                        hasil.Data = detailTrip;
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
        public async Task<IActionResult> PutTripDetail([FromRoute] long id, [FromBody] TripDetail tripdetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tripdetail.Id)
            {
                return BadRequest();
            }

            _context.Entry(tripdetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripDetailExists(id))
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
        public async Task<IActionResult> PostTripDetail([FromBody] TripDetail tripdetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TripDetails.Add(tripdetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTripDetail", new { id = tripdetail.Id }, tripdetail);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTripDetail([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tripdetail = await _context.TripDetails.FindAsync(id);
            if (tripdetail == null)
            {
                return NotFound();
            }

            _context.TripDetails.Remove(tripdetail);
            await _context.SaveChangesAsync();

            return Ok(tripdetail);
        }

        private bool TripDetailExists(long id)
        {
            return _context.TripDetails.Any(e => e.Id == id);
        }

    }
}
