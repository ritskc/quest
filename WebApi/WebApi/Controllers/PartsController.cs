using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using WebApi.IServices;

namespace WebApi.Controllers
{
    //[Authorize]
    [Route("[controller]")]
    [ApiController]
    public class PartsController : ControllerBase
    {
        private readonly IPartService _partService;

        public PartsController(IPartService partService)
        {
            this._partService = partService;
        }

        // GET: api/Todo
        [HttpGet("{companyId}/{warehouseId}")]
        public async Task<ActionResult<IEnumerable<Part>>> GetParts(int companyId, int warehouseId)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                int userId = Convert.ToInt32(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value);
                if (userId == 0)
                    return StatusCode(401, "user is unautorized");

                var result = await this._partService.GetAllPartsAsync(companyId,warehouseId,userId);

                if (result == null)
                {
                    return NotFound();
                }

                return result.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }

        }

        // GET api/values/5
        [HttpGet("{companyId}/{warehouseId}/{id}")]
        public async Task<ActionResult<Part>> Get(int companyId,int warehouseId, int id)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                int userId = Convert.ToInt32(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value);
                if (userId == 0)
                    return StatusCode(401, "user is unautorized");

                var result = await this._partService.GetPartAsync(id,warehouseId);

                if (result == null)
                {
                    return NotFound();
                }

                return result;
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }

        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Part part)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                int userId = Convert.ToInt32(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value);
                if (userId == 0)
                    return StatusCode(401, "user is unautorized");


                if (part == null)
                    return StatusCode(500,"invalid part");
                if (string.IsNullOrEmpty(part.Code) || string.IsNullOrEmpty(part.Description))
                    return StatusCode(500, "invalid partcode / description");
                
                var parts = await this._partService.GetAllPartsAsync(part.CompanyId,part.WarehouseId,userId);
                if (parts.Where(x => x.Code == part.Code).Count() > 0)
                    return StatusCode(302,"partcode already exist");                

                await this._partService.AddPartAsync(part);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Part part)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                int userId = Convert.ToInt32(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value);
                if (userId == 0)
                    return StatusCode(401, "user is unautorized");

                if (id != part.Id)
                {
                    return BadRequest();
                }

                if (part == null)
                    return StatusCode(500, "invalid part");
                if (string.IsNullOrEmpty(part.Code) || string.IsNullOrEmpty(part.Description))
                    return StatusCode(500, "invalid partcode / description");                

                part.Id = id;
                await this._partService.UpdatePartAsync(part);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }       
        
       
        // PUT api/values/5
        [HttpPost("{companyId}/{warehouseId}/{partId}/{direction}/{qty}/{note}")]
        public async Task<IActionResult> Put(int companyId,int warehouseId, string type, int partId, int qty,string direction, string note)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                int userId = Convert.ToInt32(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value);
                if (userId == 0)
                    return StatusCode(401, "user is unautorized");

                if (direction.ToLower() == BusinessConstants.DIRECTION.IN.ToString().ToLower() || direction.ToLower() == BusinessConstants.DIRECTION.OUT.ToString().ToLower())
                    await this._partService.UpdateQtyInHandByPartIdAsync(companyId, warehouseId, partId, qty,direction,note);
                else
                    return BadRequest();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

      
        //DELETE: api/Todo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Part>> Delete(int id)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                int userId = Convert.ToInt32(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value);
                if (userId == 0)
                    return StatusCode(401, "user is unautorized");

                await this._partService.DeletePartAsync(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }
    }
}