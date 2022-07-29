using System.Collections.Generic;
using UnityEngine;

namespace Analitycs
{
    public static class EventSender
    {
        private static float _levelStartTime;
        private static int  _level;
        private static int _levelCount;

        public static void SendLevelStart(int level, int levelCount)
        {
            var metrica = AppMetrica.Instance;
            _levelStartTime = Time.time;
            _level = level;
            _levelCount = levelCount;

            Debug.Log("level_start");

            var parametrs = new Dictionary<string, object>
            {
                {"level_number", level},
                {"level_name", "Level_" + level},
                {"level_count", levelCount}
            };
            
            Debug.Log("level_number " + _level);
            Debug.Log("level_name:Level_ " + _level);
            Debug.Log("level_count " + _levelCount);

            metrica.ReportEvent("level_start", parametrs);
            metrica.SendEventsBuffer();
        }

        public static void SendLevelFinish()
        {
            float time = Time.time - _levelStartTime;
            Debug.Log("level_finish");

            var metrica = AppMetrica.Instance;
            var parametrs = new Dictionary<string, object>
            {
                {"level_number", _level},
                {"level_name", "Level_" + _level},
                {"level_count", _levelCount},
                {"time", Mathf.RoundToInt(time)},
            };
            
            Debug.Log("  " );
            Debug.Log("level_number " + _level);
            Debug.Log("level_name:Level_ " + _level);
            Debug.Log("level_count " + _levelCount);
            Debug.Log("time" + parametrs["time"]);

            metrica.ReportEvent("level_finish", parametrs);
            metrica.SendEventsBuffer();
        }
    }
}
