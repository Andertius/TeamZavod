using System;
using System.Collections.Generic;
using System.Text;

using log4net;
using log4net.Config;

namespace Memento
{
    /// <summary>
    /// Looger class.
    /// </summary>
    public static class Logger
    {
        private static readonly ILog log = LogManager.GetLogger("LOGGER");

        /// <summary>
        /// Gets Logger.
        /// </summary>
        public static ILog Log
        {
            get { return log; }
        }

        /// <summary>
        /// Logger intialize.
        /// </summary>
        public static void InitLogger()
        {
            XmlConfigurator.Configure();
        }
    }
}
