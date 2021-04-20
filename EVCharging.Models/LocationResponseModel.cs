using System;
using System.Collections.Generic;
using System.Text;

namespace EVCharging.Models
{
    public class LocationResponseModel
    {
        public readonly object connections;

        public DataProvider DataProvider { get; set; }
        public OperatorInfo OperatorInfo { get; set; }
        public UsageType UsageType { get; set; }
        public StatusType StatusType { get; set; }
        public SubmissionStatus SubmissionStatus { get; set; }
        public List<UserComment> UserComments { get; set; }
        public object PercentageSimilarity { get; set; }
        public List<MediaItem> MediaItems { get; set; }
        public bool IsRecentlyVerified { get; set; }
        public string DateLastVerified { get; set; }
        public int ID { get; set; }
        public string UUID { get; set; }
        public object ParentChargePointID { get; set; }
        public int DataProviderID { get; set; }
        public object DataProvidersReference { get; set; }
        public int OperatorID { get; set; }
        public object OperatorsReference { get; set; }
        public int UsageTypeID { get; set; }
        public object UsageCost { get; set; }
        public AddressInfo AddressInfo { get; set; }
        public List<Connection> Connections { get; set; }
        public int NumberOfPoints { get; set; }
        public object GeneralComments { get; set; }
        public object DatePlanned { get; set; }
        public object DateLastConfirmed { get; set; }
        public int StatusTypeID { get; set; }
        public string DateLastStatusUpdate { get; set; }
        public object MetadataValues { get; set; }
        public int DataQualityLevel { get; set; }
        public string DateCreated { get; set; }
        public int SubmissionStatusTypeID { get; set; }
    }
    public class DataProviderStatusType
    {
        public object IsProviderEnabled { get; set; }
        public int ID { get; set; }
        public string Title { get; set; }
    }

    public class DataProvider
    {
        public string WebsiteURL { get; set; }
        public object Comments { get; set; }
        public DataProviderStatusType DataProviderStatusType { get; set; }
        public object IsRestrictedEdit { get; set; }
        public object IsOpenDataLicensed { get; set; }
        public object IsApprovedImport { get; set; }
        public string License { get; set; }
        public object DateLastImported { get; set; }
        public int ID { get; set; }
        public string Title { get; set; }
    }

    public class OperatorInfo
    {
        public string WebsiteURL { get; set; }
        public object Comments { get; set; }
        public object PhonePrimaryContact { get; set; }
        public object PhoneSecondaryContact { get; set; }
        public object IsPrivateIndividual { get; set; }
        public object AddressInfo { get; set; }
        public object BookingURL { get; set; }
        public object ContactEmail { get; set; }
        public object FaultReportEmail { get; set; }
        public object IsRestrictedEdit { get; set; }
        public int ID { get; set; }
        public string Title { get; set; }
    }

    public class UsageType
    {
        public object IsPayAtLocation { get; set; }
        public object IsMembershipRequired { get; set; }
        public object IsAccessKeyRequired { get; set; }
        public int ID { get; set; }
        public string Title { get; set; }
    }

    public class StatusType
    {
        public object IsOperational { get; set; }
        public object IsUserSelectable { get; set; }
        public int ID { get; set; }
        public string Title { get; set; }
    }

    public class SubmissionStatus
    {
        public object IsLive { get; set; }
        public int ID { get; set; }
        public string Title { get; set; }
    }

    public class CommentType
    {
        public int ID { get; set; }
        public string Title { get; set; }
    }

    public class User
    {
        public int ID { get; set; }
        public object IdentityProvider { get; set; }
        public object Identifier { get; set; }
        public object CurrentSessionToken { get; set; }
        public string Username { get; set; }
        public object Profile { get; set; }
        public object Location { get; set; }
        public object WebsiteURL { get; set; }
        public int ReputationPoints { get; set; }
        public object Permissions { get; set; }
        public object PermissionsRequested { get; set; }
        public object DateCreated { get; set; }
        public object DateLastLogin { get; set; }
        public object IsProfilePublic { get; set; }
        public object IsEmergencyChargingProvider { get; set; }
        public object IsPublicChargingProvider { get; set; }
        public object Latitude { get; set; }
        public object Longitude { get; set; }
        public object EmailAddress { get; set; }
        public object EmailHash { get; set; }
        public string ProfileImageURL { get; set; }
        public object IsCurrentSessionTokenValid { get; set; }
        public object APIKey { get; set; }
        public object SyncedSettings { get; set; }
    }

    public class CheckinStatusType
    {
        public object IsPositive { get; set; }
        public object IsAutomatedCheckin { get; set; }
        public int ID { get; set; }
        public string Title { get; set; }
    }

    public class UserComment
    {
        public int ID { get; set; }
        public int ChargePointID { get; set; }
        public int CommentTypeID { get; set; }
        public CommentType CommentType { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
        public object Rating { get; set; }
        public string RelatedURL { get; set; }
        public string DateCreated { get; set; }
        public User User { get; set; }
        public int CheckinStatusTypeID { get; set; }
        public CheckinStatusType CheckinStatusType { get; set; }
        public object IsActionedByEditor { get; set; }
    }

    public class MediaItem
    {
        public int ID { get; set; }
        public int ChargePointID { get; set; }
        public string ItemURL { get; set; }
        public string ItemThumbnailURL { get; set; }
        public string Comment { get; set; }
        public object IsEnabled { get; set; }
        public object IsVideo { get; set; }
        public object IsFeaturedItem { get; set; }
        public object IsExternalResource { get; set; }
        public object MetadataValue { get; set; }
        public User User { get; set; }
        public string DateCreated { get; set; }
    }

    public class Country
    {
        public string ISOCode { get; set; }
        public string ContinentCode { get; set; }
        public int ID { get; set; }
        public string Title { get; set; }
    }

    public class AddressInfo
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string AddressLine1 { get; set; }
        public object AddressLine2 { get; set; }
        public string Town { get; set; }
        public string StateOrProvince { get; set; }
        public object Postcode { get; set; }
        public int CountryID { get; set; }
        public Country Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public object ContactTelephone1 { get; set; }
        public object ContactTelephone2 { get; set; }
        public object ContactEmail { get; set; }
        public object AccessComments { get; set; }
        public string RelatedURL { get; set; }
        public object Distance { get; set; }
        public int DistanceUnit { get; set; }
        public string CountryName { get; set; }
    }

    public class ConnectionType
    {
        public string FormalName { get; set; }
        public object IsDiscontinued { get; set; }
        public object IsObsolete { get; set; }
        public int ID { get; set; }
        public string Title { get; set; }
    }

    public class Level
    {
        public string Comments { get; set; }
        public object IsFastChargeCapable { get; set; }
        public int ID { get; set; }
        public string Title { get; set; }
    }

    public class Connection
    {
        public int ID { get; set; }
        public int ConnectionTypeID { get; set; }
        public ConnectionType ConnectionType { get; set; }
        public object Reference { get; set; }
        public object StatusTypeID { get; set; }
        public object StatusType { get; set; }
        public int LevelID { get; set; }
        public Level Level { get; set; }
        public object Amps { get; set; }
        public object Voltage { get; set; }
        public double PowerKW { get; set; }
        public object CurrentTypeID { get; set; }
        public object CurrentType { get; set; }
        public object Quantity { get; set; }
        public object Comments { get; set; }
    }
}
