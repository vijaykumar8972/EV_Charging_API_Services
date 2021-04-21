using EVCharging.Data;
using EVCharging.Models;
using EVCharging.Services;
using EVCharging.Utilities;
using EVCharging.Utilities.Helpers;
using MimeKit;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace EVCharging.Services
{
    public class UserServices : IUserServices
    {
        private readonly EVChargingDBContext eVChargingDBContext = null;
        private readonly CryptoHelper cryptoHelper;
        private readonly EmailHelper emailHelper;
        private readonly IEmailConfig _emailConfig;

        public int UserId { get; private set; }

        public UserServices(IEVChargingDatabaseSettings eVChargingDatabaseSettings, IEmailConfig emailConfig)
        {
            cryptoHelper = FoundationObject.FoundationObj.CryptoHelper;
            emailHelper = FoundationObject.FoundationObj.emailHelper;
            this.eVChargingDBContext = new EVChargingDBContext(eVChargingDatabaseSettings);
            this._emailConfig = emailConfig;

        }

        public Users AuthUser(string EmailId, string passWord)
        {
            try
            {
                var user = eVChargingDBContext.users.Find<Users>(user => (user.EmailId.ToLower() == EmailId.ToLower() || user.MobileNumber == EmailId)).FirstOrDefault();
                if (user == null) return null;
                string pwdphase = cryptoHelper.CreatePasswordHash(passWord, user.SaltPassword);
                if (user.Password == pwdphase)
                {
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Users Create(Users users)
        {
            if (!string.IsNullOrEmpty(users.EmailId))
            {
                var resEmail = eVChargingDBContext.users.Find(n => n.EmailId == users.EmailId).FirstOrDefault();
                if (resEmail != null)
                {
                    throw new Exception("EmailID already Exit");
                }
            }
            var resMobileno = eVChargingDBContext.users.Find(n => n.MobileNumber == users.MobileNumber).FirstOrDefault();
            if (resMobileno != null)
            {
                throw new Exception("MobileNo already Exit");
            }
            string passed = users.Password;
            users.EmailId.ToLower();
            users.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            users.SaltPassword = cryptoHelper.CreateSalt();
            users.Password = cryptoHelper.CreatePasswordHash(users.Password, users.SaltPassword);
            eVChargingDBContext.users.InsertOne(users);
            var pathToFile = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"EmailTemplate/WelComeEmailTemplate.html")).Replace("\\", "/");
            var builder = new BodyBuilder();
            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }
            var msg = emailHelper.CreateSAEmailMessage(users.EmailId, users.Password, users.UserName);
            string messageBody = string.Format(builder.HtmlBody,
                    "Confirm Account Registration",
                    String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now),
                    users.EmailId,
                    users.UserName,
                    passed,
                    "",
                    ""
                    );
            emailHelper.SentEmail(_emailConfig.FormEmail, _emailConfig.Password, users.EmailId, messageBody);
            return users;
        }

        public void Delete(string Id)
        {
            eVChargingDBContext.users.DeleteOne(n => n.Id == Id);
        }

       

        public bool ForgetPassword(Users user)
        {
            var password = cryptoHelper.GeneratePassword();
            var userdb = eVChargingDBContext.users.Find(v => v.Id == user.Id).FirstOrDefault();
            userdb.SaltPassword = cryptoHelper.CreateSalt();
            userdb.Password = cryptoHelper.CreatePasswordHash(password, user.SaltPassword);
            eVChargingDBContext.users.ReplaceOne(i => i.Id == user.Id, userdb);
            var pathfile = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"EmailTemplate/ForgetPasswordTemplate.html")).Replace("\\", "/");
            var build = "";
            using (StreamReader SourceReader = System.IO.File.OpenText(pathfile))
            {
                build = SourceReader.ReadToEnd();

            }
            var message = build.Replace("$$user$$", user.EmailId).Replace("$$code$$", password);
            emailHelper.SendHtmlContentMail(new string[] { user.EmailId }, "EV Charging OTP", message);
            return true;
        }

        public List<MstChargingAbout> GetByContentDetailes()
        {
            var res = eVChargingDBContext.ChargingAbout.Find(n => true).ToList();
            return res;
        }

        public Users GetById(string Id)
        {
            return eVChargingDBContext.users.Find(n => n.Id == Id).FirstOrDefault();
        }

        public List<Users> GetListUser()
        {
            List<Users> users = eVChargingDBContext.users.Find(n => true).ToList();
            return users;
        }

        public object GetListUserAsync(UserMapParams user)
        {

            var response = GetStations(user);

            List<LocationResponseViewModel> model = new List<LocationResponseViewModel>();
            foreach (var item in response)
            {
                DataProviderViewMdel dataProvider = new DataProviderViewMdel();
                UsageTypeViewMdel usageTypeView = new UsageTypeViewMdel();
                AddressInfoMdel AddressInfo = new AddressInfoMdel();
                ConnectionsMdel connections = new ConnectionsMdel();
                LevelMdel levelMdel = new LevelMdel();
                CurrentType currentType = new CurrentType();

                LocationResponseViewModel locationResponseView = new LocationResponseViewModel();

                List<LevelMdel> levelMdelmodel = new List<LevelMdel>();
                List<CurrentType> currentTypes = new List<CurrentType>();

                dataProvider.Title = item.DataProvider.Title;
                locationResponseView.DataProvider = dataProvider;
                usageTypeView.IsPayAtLocation = item.UsageType.IsPayAtLocation;
                usageTypeView.Title = item.UsageType.Title;
                locationResponseView.usageTypeView = usageTypeView;
                AddressInfo.Title = item.AddressInfo.Title;
                AddressInfo.AddressLine1 = item.AddressInfo.AddressLine1;
                AddressInfo.Town = item.AddressInfo.Title;
                AddressInfo.StateOrProvince = item.AddressInfo.StateOrProvince;
                AddressInfo.Postcode = item.AddressInfo.Postcode;
                AddressInfo.Latitude = item.AddressInfo.Latitude;
                AddressInfo.Longitude = item.AddressInfo.Longitude;
                AddressInfo.ContactTelephone1 = item.AddressInfo.ContactTelephone1;
                AddressInfo.ContactEmail = item.AddressInfo.ContactEmail;
                AddressInfo.AccessComments = item.AddressInfo.AccessComments;
                AddressInfo.RelatedURL = item.AddressInfo.RelatedURL;
                AddressInfo.Distance = item.AddressInfo.Distance;
                AddressInfo.DistanceUnit = item.AddressInfo.DistanceUnit;
                AddressInfo.CountryName = item.AddressInfo.CountryName;
                locationResponseView.addressInfo = AddressInfo;
                foreach (var conn in item.Connections)
                {
                    levelMdel.IsFastChargeCapable = conn.Level.IsFastChargeCapable;
                    levelMdel.PowerKW = conn.PowerKW;
                    levelMdel.Title = conn.Level.Title;
                    connections.Connenctions = conn.Level.Title;
                }
                locationResponseView.connectionsMdel = connections;
                levelMdelmodel.Add(levelMdel);
                locationResponseView.levelMdel = levelMdelmodel;

                foreach (var conn in item.Connections)
                {
                    currentType.Title = conn.Level.Title;
                }
                locationResponseView.connectionsMdel = connections;
                currentTypes.Add(currentType);
                locationResponseView.currentType = currentTypes;
                locationResponseView.numberOfPoints = item.NumberOfPoints;
                locationResponseView.dateLastStatusUpdate = item.DateLastStatusUpdate;

                model.Add(locationResponseView);

            }

            return model;

        }

        private static List<LocationResponseModel> GetStations(UserMapParams user)
        {
            UriBuilder builder = new UriBuilder("https://api.openchargemap.io/v3/poi/");
            builder.Query = "key=347654fb-65d0-435c-9239-15523ae572d7&output=json&countrycode=IN&distanceunit=km&maxresults=" + user.MaxResult+ "&distance="+user.Distance+"&latitude="+user.Latitude+"&longitude="+user.longitude;

            HttpClient client = new HttpClient();
            var result = client.GetAsync(builder.Uri).Result;
            List<LocationResponseModel> response = new List<LocationResponseModel>();
            if (result.IsSuccessStatusCode)
            {
                var res = result.Content.ReadAsStringAsync().Result;
                response = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LocationResponseModel>>(res);
            }
            else
            {
                response = null;
            }
            return response;
        }
        public List<MSTManufacture> GetManufactureDetailes()
        {
            List<MSTManufacture> res = eVChargingDBContext.Manufacture.Find(n => true).ToList();
            return res;
        }

        public List<MSTModel> GetManufactureModelDetailes(string Name)
        {
            var res = eVChargingDBContext.Manufacture.Find(n => n.Name == Name).FirstOrDefault();
            List<MSTModel> resp = eVChargingDBContext.Model.Find(n => n.ProvideId == res.ProvideId).ToList();
            return resp;
        }

        public Users GetUsersDetailes(string EmailId)
        {
            Users users = eVChargingDBContext.users.Find(n => n.EmailId == EmailId).FirstOrDefault();
            return users;
        }

       

        public void Update(Users users, string Id)
        {
            try
            {
                eVChargingDBContext.users.ReplaceOne(user => user.Id == Id, users);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
       

       

        

        public List<FeedBackMdel> GetFeedBackMdels()
        {

            List<FeedBackMdel> feedBackMdel = eVChargingDBContext.feedback.Find(n => true).ToList();
            return feedBackMdel;
        }





        public bool Create(FeedBackMdel feedBack)
        {
            feedBack.id= MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            eVChargingDBContext.feedback.InsertOne(feedBack);
            return true;
        }

        public List<ChargingHistoryModel> chargingHistoryModels()
        {
            List<ChargingHistoryModel>chargingHistoryModels = eVChargingDBContext.charginghistory.Find(n => true).ToList();
            return chargingHistoryModels;
        }

        public object GetChargingHistoryId(string userId)
        {
            List<ChargingHistoryModel> chargingHistoryModels = eVChargingDBContext.charginghistory.Find(n => true).ToList();
            return chargingHistoryModels;
        }

        public object Create(ChargingHistoryModel chargingHistory)
        {
            chargingHistory.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            eVChargingDBContext.charginghistory.InsertOne(chargingHistory);
            return chargingHistory;
        }

        public List<FeedBackMdel> GetFeedBackId(string userId)
        {
            List<FeedBackMdel> feedBackMdels = new List<FeedBackMdel>();
            feedBackMdels = eVChargingDBContext.feedback.Find(n => n.UserId == userId).ToList();
            return feedBackMdels;
        }

      
    }
}
