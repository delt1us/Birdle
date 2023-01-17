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
        public bool b_Completed;
        public int i_GridSize;
        public string str_ImagePath;

        public Level(string LevelName, int gridSize, string imagePath)
        {
            str_LevelName = LevelName;
            f_PersonalBestTime = 0f;
            i_PersonalBestMoves = 0;
            i_Attempts = 0;
            b_Completed = false;
            i_GridSize = gridSize;
            str_ImagePath = imagePath;
        }
    }
}
