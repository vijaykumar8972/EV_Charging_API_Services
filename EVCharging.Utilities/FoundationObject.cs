using EVCharging.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVCharging.Utilities
{
    public class FoundationObject
    {
        private static FoundationObject _foundationObj;
        public static FoundationObject FoundationObj
        {
            get
            {
                if (_foundationObj == null)
                {
                    _foundationObj = new FoundationObject();
                }
                return _foundationObj;
            }
        }

        #region Crypto Helper Object Creation
        private CryptoHelper _cryptoHelper;
        public CryptoHelper CryptoHelper
        {
            get
            {
                if (_cryptoHelper == null)
                {
                    _cryptoHelper = new CryptoHelper();
                };
                return _cryptoHelper;
            }
        }
        #endregion

        #region authentication Helper Object Creation
        private Authentication _authentication;
        public Authentication authentication
        {
            get
            {
                if (_authentication == null)
                {
                    _authentication = new Authentication();
                };
                return _authentication;
            }
        }
        #endregion


        #region EmailHelper Helper Object Creation
        private EmailHelper _emailHelper;
        public EmailHelper emailHelper
        {
            get
            {
                if (_emailHelper == null)
                {
                    _emailHelper = new EmailHelper();
                };
                return _emailHelper;
            }
        }
        #endregion


        #region ConfigHelper Helper Object Creation
        private ConfigHelper _configHelper;
        public ConfigHelper configHelper
        {
            get
            {
                if (_configHelper == null)
                {
                    _configHelper = new ConfigHelper();
                };
                return _configHelper;
            }
        }
        #endregion

        #region FileHelper Helper Object Creation
        private FileHelper _fileHelper;
        public FileHelper fileHelper
        {
            get
            {
                if (_fileHelper == null)
                {
                    _fileHelper = new FileHelper();
                };
                return _fileHelper;
            }
        }
        #endregion

        #region LogHelper Helper Object Creation
        private LogHelper _logHelper;
        public LogHelper logHelper
        {
            get
            {
                if (_logHelper == null)
                {
                    _logHelper = new LogHelper();
                };
                return _logHelper;
            }
        }
        #endregion
    }
}
