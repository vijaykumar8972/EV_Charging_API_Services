using System;
using System.Collections.Generic;
using System.Text;
using static EVCharging.Models.AddressInfoMdel;

namespace EVCharging.Models
{
    public class LocationResponseViewModel
    {
        public DataProviderViewMdel DataProvider { get; set; }
        public UsageTypeViewMdel usageTypeView { get; set; }
        public AddressInfoMdel addressInfo { get; set; }

        public ConnectionsMdel connectionsMdel { get; set; }

        public List<LevelMdel> levelMdel { get; set; }

        public List<CurrentType> currentType { get; set; }
        public int numberOfPoints { get; set; }
        public string dateLastStatusUpdate { get; set; }
    }
    public class DataProviderViewMdel
    {
        public string Title { get; set; }


    }
    public class UsageTypeViewMdel
    {
        public object IsPayAtLocation { get; set; }
        public string Title { get; set; }


    }
    public class AddressInfoMdel
    {


        public string Title { get; set; }
        public string AddressLine1 { get; set; }
        public string Town { get; set; }
        public string StateOrProvince { get; set; }
        public object Postcode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public object ContactTelephone1 { get; set; }
        public object ContactEmail { get; set; }
        public object AccessComments { get; set; }
        public string RelatedURL { get; set; }
        public object Distance { get; set; }
        public int DistanceUnit { get; set; }
        public string CountryName { get; set; }
    }


    public class ConnectionsMdel
    {
        public string Connenctions { get; set; }


    }
    public class LevelMdel
    {
        public object IsFastChargeCapable { get; set; }
        public string Title { get; set; }
        public double PowerKW { get; set; }


    }

    public class CurrentType
    {
        public string Title { get; set; }

    }

    public class UserMapParams
    {
        public double Latitude { get; set; }
        public double longitude { get; set; }
        public int Distance { get; set; }
        public int MaxResult { get; set; }
    }
}
