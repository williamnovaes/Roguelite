using UnityEngine;

namespace Main {
    public static class Constants {
        public static string SAVE_FILE_NAME = "gameData.lh";
        public static string SAVE_FILE_DIR = "/gameData.lh";
        public static int DESIRED_SECONDS_LENGTH = 4;
        public static int TEN_MINUTES_TO_SECONDS = 600;
        public static long TEN_MINUTS_TO_MILISECONDS = 1000 * 60 * 10;
        public static long ONE_SECOND_IN_MILISECONDS = 1000;
        public static float JOYSTICK_AXIS_MAX_VALUE = 1f;
    }
}