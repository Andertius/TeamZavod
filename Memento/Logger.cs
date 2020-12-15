// <copyright file="Logger.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using log4net;
using log4net.Config;

namespace Memento
{
    /// <summary>
    /// Looger class.
    /// </summary>
    public static class Logger
    {
        private static readonly ILog LogValue = LogManager.GetLogger("LOGGER");

        /// <summary>
        /// Gets Logger.
        /// </summary>
        public static ILog Log
        {
            get { return LogValue; }
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
