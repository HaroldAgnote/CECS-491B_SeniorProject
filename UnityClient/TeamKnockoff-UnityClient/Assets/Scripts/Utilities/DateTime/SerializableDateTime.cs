using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utilities.DateTime {
    // Serializable version of the DateTime since Unity does not allow you to
    // serialize the System DateTime 

    [Serializable]
    public class SerializableDateTime {
        public int second;
        public int minute;
        public int hour;

        public int day;
        public int month;
        public int year;

        public SerializableDateTime(System.DateTime dateTime) {
            second = dateTime.Second;
            minute = dateTime.Minute;
            hour = dateTime.Hour;
            day = dateTime.Day;
            month = dateTime.Month;
            year = dateTime.Year;
        }

        public override string ToString() {
            return $"{year}-{month}-{day}_T_{hour}-{minute}-{second}";
        }
    }
}
