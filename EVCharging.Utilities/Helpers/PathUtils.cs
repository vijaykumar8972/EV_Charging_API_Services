using System;
using System.Collections.Generic;
using System.Text;

namespace EVCharging.Utilities.Helpers
{
    public class PathUtils
    {
        private const string _mainFolderPath = "Files\\";

        public const string UserProfile = _mainFolderPath + "{0}\\{1}\\{2}\\{3}";
        public const string UserLogFile = _mainFolderPath + "{0}\\{1}";
    }
}
