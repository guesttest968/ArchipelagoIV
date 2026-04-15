using Archipelago.MultiClient.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchipelagoIV
{
    internal class Global
    {
        public static ArchipelagoSession session;
        public static int MyIndex = 0;
        public static Queue<long> ItemQueue = new Queue<long>(); // Items waiting to be given
        public static string missionName = "NULL";
        public static bool ismissioncompleted = false;
        public static bool hasSentCheck = false;
        public static string chatlog;
        public static bool firstconnected;
        public static bool _isModalActive = true;
    }
}
