using EVCharging.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVCharging.Services
{
    public interface IUserServices
    {
        Users GetById(string Id);
        FeedBackMdel List<GetByFeedId>(string Id);
        Users AuthUser(string userName, string passWord);
        List<Users> GetListUser();
        object GetListUserAsync(UserMapParams user);
        Users Create(Users users);
        void Update(Users users, string Id);
        void Delete(string Id);
        Users GetUsersDetailes(string EmailId);        
        List<MSTModel> GetManufactureModelDetailes(string Name);
        List<MSTManufacture> GetManufactureDetailes();
        List<MstChargingAbout> GetByContentDetailes();
        FeedBackMdel feedBackMdels();

        bool ForgetPassword(Users user);
        
        
    }
}
