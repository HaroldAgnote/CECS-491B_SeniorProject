using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utilities.DateTime {
    // Serializable version of the DateTime since Unity does not allow you to
    // serialize the System DateTime 

    [Serializable]
    public class SerializableDateTime : IComparable<SerializableDateTime> {
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

        public int CompareTo(SerializableDateTime other) {
            if (this.year == other.year) {
                if (this.month == other.month) {
                    if (this.day == other.day) {
                        if (this.hour == other.hour) {
                            if (this.minute == other.minute) {
                                if (this.second == other.second) {
                                    return 0;
                                }
                                return this.second.CompareTo(other.second);
                            }
                            return this.minute.CompareTo(other.minute);
                        }
                        return this.hour.CompareTo(other.hour);
                    }
                    return this.day.CompareTo(other.day);
                }
                return this.month.CompareTo(other.month);
            }
            return this.year.CompareTo(other.year);
        }

        public string ToFileString() {
            return $"{year}-{month.ToString("00")}-{day.ToString("00")}_T_{hour.ToString("00")}-{minute.ToString("00")}-{String.Format("00", second)}";
        }

        public override string ToString() {
            return $"{year}/{month.ToString("00")}/{day.ToString("00")} - {hour.ToString("00")}:{minute.ToString("00")}:{second.ToString("00")}";
        }
    }
}
