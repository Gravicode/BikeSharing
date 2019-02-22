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
    public class UserProfileController :Controller
    {
        private readonly BikeSharingDB _context;

        public UserProfileController(BikeSharingDB context)
        {
            _context = context;
        }

        // GET: api/AppLogs
        [HttpGet("[action]")]
       //[Route("GetUserProfiles")]
        public async Task<IActionResult> GetUserProfiles()
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                hasil.Data = _context.UserProfiles.ToList();
            }
            catch (Exception ex)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = ex.Message;
            }
            return Ok(hasil);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserProfiles([FromRoute] long id)
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

                    var userprofile = await _context.UserProfiles.FindAsync(id);

                    if (userprofile == null)
                    {
                        hasil.IsSucceed = false;
                        hasil.ErrorMessage = "not found";
                    }
                    else
                    {
                        hasil.Data = userprofile;
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
        public async Task<IActionResult> PutUserProfiles([FromRoute] long id, [FromBody] UserProfile UserProfiles)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != UserProfiles.Id)
            {
                return BadRequest();
            }

            _context.Entry(UserProfiles).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserProfilesExists(id))
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
        [Route("PostUserProfile")]
        public async Task<IActionResult> PostUserProfiles([FromBody] UserProfile UserProfiles)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.UserProfiles.Add(UserProfiles);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserProfile", new { id = UserProfiles.Id }, UserProfiles);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMarket([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var UserProfiles = await _context.UserProfiles.FindAsync(id);
            if (UserProfiles == null)
            {
                return NotFound();
            }

            _context.UserProfiles.Remove(UserProfiles);
            await _context.SaveChangesAsync();

            return Ok(UserProfiles);
        }

        private bool UserProfilesExists(long id)
        {
            return _context.UserProfiles.Any(e => e.Id == id);
        }
    }
}
