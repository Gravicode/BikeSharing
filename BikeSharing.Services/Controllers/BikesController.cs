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
    public class BikesController : Controller
    {
        private readonly BikeSharingDB _context;

        public BikesController(BikeSharingDB context)
        {
            _context = context;
        }

        // GET: api/AppLogs
        [HttpGet("[action]")]
        //[Route("GetBikes")]
        public async Task<IActionResult> GetBikes()
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                hasil.Data = _context.Bikes.ToList();
            }
            catch (Exception ex)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = ex.Message;
            }
            return Ok(hasil);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBikes([FromRoute] long id)
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

                    var bikes = await _context.Bikes.FindAsync(id);

                    if (bikes == null)
                    {
                        hasil.IsSucceed = false;
                        hasil.ErrorMessage = "not found";
                    }
                    else
                    {
                        hasil.Data = bikes;
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
        public async Task<IActionResult> PutBikes([FromRoute] long id, [FromBody] Bike Bikes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Bikes.Id)
            {
                return BadRequest();
            }

            _context.Entry(Bikes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BikesExists(id))
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
        public async Task<IActionResult> PostBikes([FromBody] Bike Bikes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Bikes.Add(Bikes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBike", new { id = Bikes.Id }, Bikes);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBike([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Bikes = await _context.Bikes.FindAsync(id);
            if (Bikes == null)
            {
                return NotFound();
            }

            _context.Bikes.Remove(Bikes);
            await _context.SaveChangesAsync();

            return Ok(Bikes);
        }

        private bool BikesExists(long id)
        {
            return _context.Bikes.Any(e => e.Id == id);
        }
    }
}
