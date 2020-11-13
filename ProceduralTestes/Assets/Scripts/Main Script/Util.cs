using UnityEngine;

namespace Main {
    public static class Util {
        public static bool IsValidString(string value) {
            string textAux = value.Trim();
            return textAux != null && textAux.Length > 0;
        }

        public static bool IntToBool(int value) {
            return value == 1;
        }

        public static int BoolToInt(bool value) {
            return value ? 1 : 0;
        }

        public static bool Between(float min, float max, float value) {
            return value >= min && value <= max;
        }

        public static float AngleFacingLeft(float angle) {
            return angle + 180;
        }

        public static Quaternion AngleAim(float dir, float angle) {
            return Quaternion.Euler(0f, 0f, dir > 0f ? angle : AngleFacingLeft(angle));
        }

        public static int StringToHash(string name) {
            return name.GetHashCode();
        }

        public static string SecondsToTimeString(long seconds) {
            string convertedText = "";
            int length = seconds.ToString().Length;
            if (Constants.DESIRED_SECONDS_LENGTH - length > 0) {
                for (int i = 0; i < Constants.DESIRED_SECONDS_LENGTH - length; i++) {
                    convertedText += "0";
                }
            }

            convertedText += seconds.ToString();
            return convertedText;
        }

        public static string MilisecondsToMinSecString(long miliseconds) {
            return ((miliseconds / 1000) / 60).ToString("00") + ":" + ((miliseconds / 1000) % 60).ToString("00");
        }

        public static float Normalize(float value, float max) {
            if (value == 0f) {
                return 0f;
            }

            return value / max;
        }
    }
}