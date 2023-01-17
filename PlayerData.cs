using System;
using System.Collections.Generic;

namespace Birdle
{
    // A class to hold data about each level
    internal class PlayerData
    {
        private static int i_NUMBER_OF_LEVELS = 4;

        public Level[] a_Levels;
        public PlayerData()
        {
            // Makes array for levels with length i_NUMBER_OF_LEVELS
            a_Levels = new Level[i_NUMBER_OF_LEVELS];

            // Generates all levels
            a_Levels[0] = new Level("level 1", 3, "level1");
            a_Levels[1] = new Level("level 2", 3, "level2");
            a_Levels[2] = new Level("level 3", 5, "level3");
            a_Levels[3] = new Level("level 4", 5, "level4");            
        }
    }
}
