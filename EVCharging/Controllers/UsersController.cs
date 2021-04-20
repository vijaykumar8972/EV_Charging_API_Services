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
        [AllowAnonymous]
        [HttpPost("UpdateUsersProfile")]
        public ActionResult UpdateUsersProfile(UsersModel usersModel)
        {
            try
            {
                var res = userServices.GetById(usersModel.UserId);
                if (res != null)
                {
                    var users = new Users()
                    {
                        EmailId = usersModel.EmailId,
                        MobileNumber = usersModel.MobileNumber,
                        UserName = usersModel.UserName,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsActive = true
                    };

                    res.ProfileImage = string.Format(PathUtils.UserProfile, "EVCharging", res.Id.ToString(), "Photos", res.Id.ToString() + ".jpg");
                    string path = Path.Combine(hostingEnvironment.ContentRootPath, res.ProfileImage);
                    fileHelper.CreateDirectoryIfNotExists(Path.GetDirectoryName(Path.GetFullPath(path)));
                    string imageName = res.ProfileImage + ".jpg";
                    string imgPath = Path.Combine(path, imageName);
                    byte[] imageBytes = Convert.FromBase64String(usersModel.ProfileImage);
                    System.IO.File.WriteAllBytes(path, imageBytes);

                    userServices.Update(res, res.Id);
                    userServices.Update(users, users.Id);
                    return StatusCode(StatusCodes.Status201Created,usersModel);
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
        [Authorize]
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
        [AllowAnonymous]
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
      
        
       
    }


    
    
    }

