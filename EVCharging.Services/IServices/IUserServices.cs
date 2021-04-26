using EVCharging.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVCharging.Services
{
    public interface IUserServices
    {      

        Users GetById(string Id);
      
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
        bool ForgetPassword(Users user);
        List<FeedBackMdel> GetFeedBackMdels();
        List<FeedBackMdel> GetFeedBackId(string userId);
        bool Create(FeedBackMdel feedBack);
        List<ChargingHistoryModel> chargingHistoryModels();
        List<ChargingHistoryModel> GetChargingHistoryId(string userId);
        ChargingHistoryModel GetCheckChargingHistoryId(string userId,string StationId);
        ResponseViewModel postchargingHistory(ChargingHistoryModel chargingHistory);
        object GetAllData();
        List<FavModel> GetFav(string userId);
        FavModel postfav(FavModel favModel);
        List<FeedBackMdel> GetAllFeedBack(string StationId);
        void deletehistorybyUserid(string userId, string StationId);
    }
}
