using System;
using System.Collections.Generic;

namespace Birdle
{
    // A class to hold data about each level
    internal class PlayerData
    {
        private static int i_NUMBER_OF_LEVELS = 1;

        public Level[] a_Levels;
        public PlayerData()
        {
            // Makes array for levels with length i_NUMBER_OF_LEVELS
            a_Levels = new Level[i_NUMBER_OF_LEVELS];

            // Generates all levels
            for (int i = 0; i < i_NUMBER_OF_LEVELS; i++)
            {
                a_Levels[i] = new Level(i_NUMBER_OF_LEVELS.ToString());
            }
        }
    }
}
