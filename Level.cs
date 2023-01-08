using System;

namespace Birdle
{
    // Class for holding information about each level 
    internal class Level
    {
        public string str_LevelName;
        public float f_PersonalBestTime;
        public int i_PersonalBestMoves;
        public int i_Attempts;

        public Level(string LevelName)
        {
            str_LevelName = LevelName;
            f_PersonalBestTime = 0f;
            i_PersonalBestMoves = 0;
            i_Attempts = 0;
        }
    }
}
