using EVCharging.Models;
using EVCharging.Services;
using EVCharging.Utilities;
using EVCharging.Utilities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EVCharging.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserServices userServices;
        private readonly Authentication authentication;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly FileHelper fileHelper;
        private readonly LogHelper logHelper;
        private readonly CryptoHelper cryptoHelper;
        public UsersController(IUserServices _userServices, IWebHostEnvironment _hostingEnvironment)
        {
            this.userServices = _userServices;
            this.authentication = FoundationObject.FoundationObj.authentication;
            this.hostingEnvironment = _hostingEnvironment;
            this.fileHelper = FoundationObject.FoundationObj.fileHelper;
            this.logHelper = FoundationObject.FoundationObj.logHelper;
            this.cryptoHelper = FoundationObject.FoundationObj.CryptoHelper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            try
            {
                var user = userServices.AuthUser(loginViewModel.UserName, loginViewModel.Password);
                if (user == null)
                    return Ok(new { token = "", Message = "User Inavild" });
                return Ok(new { token = authentication.GenerateJSONWebToken(user), Message = "User Valid", user });
            }
            catch (Exception ex)
            {
                logHelper.ErrorLogs("UsersController", "Login", "", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Registration")]
        public ActionResult Registration([FromBody] Users users)
        {
            try
            {
                var res = userServices.Create(users);
                if (res == null)
                {
                    return NotFound();
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                logHelper.ErrorLogs("UsersController", "Registration", "", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("UpdateUsersProfile")]
        public ActionResult UpdateUsersProfile(UsersModel model)
        {
            try
            {
                var res = userServices.GetById(model.UserId);
                if (res != null)
                {
                    res.UpdatedAt = DateTime.Now;
                    res.MobileNumber = model.MobileNumber;
                    res.UserName = model.UserName;
                    res.EmailId = model.EmailId;
                    if ((model.ProfileImage != null) && model.ProfileImage != "")
                    {
                        res.ProfileImage = string.Format(PathUtils.UserProfile, "EVCharging", res.Id.ToString(), "Photos", res.Id.ToString() + ".jpg");
                        string path = Path.Combine(hostingEnvironment.ContentRootPath, res.ProfileImage);
                        fileHelper.CreateDirectoryIfNotExists(Path.GetDirectoryName(Path.GetFullPath(path)));
                        string imageName = res.ProfileImage + ".jpg";
                        string imgPath = Path.Combine(path, imageName);
                        try
                        {
                            byte[] imageBytes = Convert.FromBase64String(model.ProfileImage);
                            System.IO.File.WriteAllBytes(path, imageBytes);
                            model.ProfileImage = res.ProfileImage;
                        }
                        catch (Exception ex)
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                        }
                    }
                    userServices.Update(res, res.Id);
                    model.ProfileImage = res.ProfileImage;
                    model.ProfileImage.Replace(@"\\", @"/");

                    return StatusCode(StatusCodes.Status201Created, model);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
            }
            catch (Exception ex)
            {
                logHelper.ErrorLogs("UsersController", "UpdateUsersProfile", "", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{Id:length(24)}")]
        public ActionResult GetById(string Id)
        {
            try
            {
                var res = userServices.GetById(Id);
                if (res == null)
                {
                    return NotFound();
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                logHelper.ErrorLogs("UsersController", "GetById", "", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetUsers")]
        public ActionResult<List<Users>> GetUsers()
        {
            try
            {
                var res = userServices.GetListUser();
                if (res == null)
                {
                    return NotFound();
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                logHelper.ErrorLogs("UsersController", "GetUsers", "", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetByContentDetailes")]
        public ActionResult GetByContentDetailes()
        {
            try
            {
                var res = userServices.GetByContentDetailes();
                if (res == null)
                {
                    return NotFound();
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                logHelper.ErrorLogs("UsersController", "GetByContentDetailes", "", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetManufactureDetailes")]
        public ActionResult GetManufactureDetailes()
        {
            try
            {
                var res = userServices.GetManufactureDetailes();
                if (res == null)
                {
                    return NotFound();
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                logHelper.ErrorLogs("UsersController", "GetManufactureDetailes", "", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetManufactureModelDetailes/{Name}")]
        public ActionResult GetManufactureModelDetailes(string Name)
        {
            try
            {
                var res = userServices.GetManufactureModelDetailes(Name);
                if (res == null)
                {
                    return NotFound();
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                logHelper.ErrorLogs("UsersController", "GetManufactureDetailes", "", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id:length(24)}")]
        public ActionResult Deleteuser(string Id)
        {
            try
            {
                var user = userServices.GetById(Id);

                if (user == null)
                {
                    return NotFound();
                }
                userServices.Delete(user.Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                logHelper.ErrorLogs("UsersController", "Deleteuser", "", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userIn"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Users userIn)
        {
            try
            {
                var user = userServices.GetById(id);
                if (user == null)
                {
                    return NotFound();
                }
                userServices.Update(userIn, id);
                return NoContent();
            }
            catch (Exception ex)
            {
                logHelper.ErrorLogs("UsersController", "Update", "", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("ForgetPassword")]
        public IActionResult ForgetPassword(ForgetPasswsordModel model)
        {
            try
            {
                bool Emailid = model.Email.Contains("@");
                Users users;
                if (Emailid)
                {

                    users = userServices.GetUsersDetailes(model.Email);
                    if (users != null) // multiple Emails Ids will store
                    {
                        bool re = userServices.ForgetPassword(users);
                        if (re) // User Email is avaialble in DB
                        {
                            return Ok(re);
                        }
                        else // User Email is not registered in DB
                        {

                            return StatusCode(StatusCodes.Status500InternalServerError, "Email Not Found");

                        }
                    }

                    else // From DB not receive any Email
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, "No Emails are not received from DB ");
                    }

                }
                else // Given Email is not following Email Format....
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Email ID is improper");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
      

        [Authorize]
        [HttpGet("GetFeedBackId")]
        public ActionResult GetFeedBackId(string UserId)             
        {
            try
            {
                var res = userServices.GetFeedBackId(UserId);
                if (res == null)
                {
                    return NotFound();
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                logHelper.ErrorLogs("UsersController", "GetFeedBackId", "", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

      
        [Authorize]
        [HttpGet("GetAllFeedBack")]
        public ActionResult GetAllFeedBack(string StationId)
        {
            try
            {
                var user = userServices.GetAllFeedBack(StationId);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                logHelper.ErrorLogs("UsersController", "GetAllFeedBack", "", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpPost("PostFeedBack")]
        public ActionResult PostFeedBack([FromBody] FeedBackMdel feedBack)
        {
           
            try
            {
                var CheckUser = userServices.GetById(feedBack.UserId);
                if (CheckUser!=null)
                {
                     userServices.Create(feedBack);
                     return Ok();
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, "UserNotFound");
                }
               
               
               
            }
            catch (Exception ex)
            {
                logHelper.ErrorLogs("UsersController", "PostFeedBack", "", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpGet("GetChargingHistory")]
        public ActionResult<List<ChargingHistoryModel>> GetChargingHistory()
        {
            try
            {
                var res = userServices.chargingHistoryModels();
                if (res == null)
                {
                    return NotFound();
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                logHelper.ErrorLogs("UsersController", "GetChargingHistory", "", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [Authorize]
        [HttpGet("GetChargingHistoryId")]
        public ActionResult GetChargingHistoryId(string UserId)
        {
            try
            {
                var res = userServices.GetChargingHistoryId(UserId);
                if (res == null)
                {
                    return NotFound();
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                logHelper.ErrorLogs("UsersController", "GetChargingHistoryId", "", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpPost("PostChargingHistory")]
        public ActionResult PostChargingHistory([FromBody] ChargingHistoryModel chargingHistory)
        {
            try
            {
                var res = userServices.postchargingHistory(chargingHistory);
                if (res == null)
                {
                    return NotFound();
                }
                if (res.StatusCode=="208")
                {
                    return NotFound(res);
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                logHelper.ErrorLogs("UsersController", "PostChargingHistory", "", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("postfav")]
        public ActionResult postfav(FavModel FavMod)
        {
            try
            {
                var user = userServices.postfav(FavMod);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                logHelper.ErrorLogs("UsersController", "postfav", "", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetFav")]
        public ActionResult GetFav(string UserId)
        {
            try
            {
                var user = userServices.GetFav(UserId);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {

                logHelper.ErrorLogs("UsersController", "GetFav", "", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetCheckChargingHistoryId")]
        public ActionResult GetCheckChargingHistoryId(string UserId ,string StationID)
        {
            try
            {
                var user = userServices.GetCheckChargingHistoryId(UserId,StationID);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {

                logHelper.ErrorLogs("UsersController", "GetCheckChargingHistoryId", "", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("DeleteHistoryByID")]
        public ActionResult deletehistorybyUserid(string UserId, string StationID)
        {
            try
            {
                 userServices.deletehistorybyUserid(UserId, StationID);
                 return Ok();

            }
            catch (Exception ex)
            {
                logHelper.ErrorLogs("UsersController", "deletehistorybyUserid", "", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


    }




}

