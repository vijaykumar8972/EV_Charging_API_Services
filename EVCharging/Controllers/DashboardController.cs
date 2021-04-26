using EVCharging.Models;
using EVCharging.Services;
using EVCharging.Utilities;
using EVCharging.Utilities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVCharging.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IUserServices userServices;

        private readonly Authentication authentication;
        public DashboardController(IUserServices _userServices)
        {
            this.userServices = _userServices;
            this.authentication = FoundationObject.FoundationObj.authentication;
        }
        [Authorize]
        [HttpPost("NearbyStations")]
        public ActionResult GeAllData(UserMapParams user)
        {
            try
            {
                var res = userServices.GetListUserAsync(user);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
      


    }
}
