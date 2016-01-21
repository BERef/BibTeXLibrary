using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibTeXLibrary
{
    public static class Config
    {
        #region BibEntry ToString Config
        private static string _retract = "  ";
        private static string _lineFeed = "\n";

        private static bool _align = false;

        public static string Retract
        {
            get { return _retract; }
        }

        public static string LineFeed
        {
            get { return _lineFeed; }
        }

        public static bool Align
        {
            get { return _align; }
        }
        #endregion

        #region Public Static Method
        public static void Load()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
