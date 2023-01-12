using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;


namespace Birdle
{
    // Class to hold everything to do with when the game is actually running
    internal class SceneGame
    {
        // Used for the solution preview
        private static Rectangle rec_SOLUTION_RECTANGLE = new Rectangle(1450, 140, 400, 400);
        private static Rectangle rec_SOLUTIONRECTANGLEBORDER = new Rectangle(rec_SOLUTION_RECTANGLE.X - 5, rec_SOLUTION_RECTANGLE.Y - 5, 410, 410);

        // Used for text shown in top left
        private static Vector2 vec_TextStartLocation = new Vector2(60, 140);

        // Grid object
        public Grid m_Grid;

        // Timer for level 
        public float f_TimeSpentOnPuzzle;

        // Font for drawing info on screen
        private SpriteFont m_Font;

        // Determines the dimensions of the grid (not scale)
        private const int i_GRIDSIZE = 3;

        public SceneGame((int width, int height) t_SCREEN_DIMENSIONS)
        {
            m_Grid = new Grid(t_SCREEN_DIMENSIONS, i_GRIDSIZE);
            f_TimeSpentOnPuzzle = 0f;

        }

        // Loads Grid object
        public void LoadGrid(Texture2D GridBorder, Texture2D GridTexture)
        {
            m_Grid.m_GridBorderTexture = GridBorder;
            m_Grid.m_Image = GridTexture;
        }

        // Loads  font 
        public void LoadFont(SpriteFont font)
        {
            m_Font = font;
        }

        // Called every frame from Update in Game1
        public void Update(float f_TimeElapsed)
        {
            // Updates timer
            f_TimeSpentOnPuzzle += f_TimeElapsed;
            // Updates grid
            m_Grid.Update();
        }

        // Called every frame from Draw in Game1
        public void Render(SpriteBatch m_SpriteBatch, PlayerData m_PlayerData)
        {
            // Draws grid
            m_Grid.Render(m_SpriteBatch);
            // Draws solution
            m_SpriteBatch.Draw(m_Grid.m_GridBorderTexture, rec_SOLUTIONRECTANGLEBORDER, null, Color.White);
            m_SpriteBatch.Draw(m_Grid.m_Image, rec_SOLUTION_RECTANGLE, null, Color.White);

            DrawText(m_SpriteBatch, m_PlayerData);
        }

        // Draws Text
        private void DrawText(SpriteBatch m_SpriteBatch, PlayerData m_PlayerData)
        {
            // Creates list
            List<string> l_StringsToDraw = new List<string>();
            // Adds to list
            l_StringsToDraw.Add($"Time: {(int)f_TimeSpentOnPuzzle}");
            l_StringsToDraw.Add($"Best time: {Math.Round((m_PlayerData.a_Levels[0].f_PersonalBestTime), 2).ToString()} seconds");
            l_StringsToDraw.Add($"Moves: {m_Grid.i_MovesMade}");
            l_StringsToDraw.Add($"Personal best: {m_PlayerData.a_Levels[0].i_PersonalBestMoves.ToString()}");
            l_StringsToDraw.Add($"Attemps: {m_PlayerData.a_Levels[0].i_Attempts.ToString()}");

            // Makes mutable copy of text start location
            Vector2 vec_TextLocation = new Vector2(vec_TextStartLocation.X, vec_TextStartLocation.Y);
            int i_LineSpacing = m_Font.LineSpacing;
            // Adds items in list to spritebatch
            foreach (string text in l_StringsToDraw)
            {
                m_SpriteBatch.DrawString(m_Font, text, vec_TextLocation, Color.Black);
                vec_TextLocation.Y += i_LineSpacing;
            }
        }
    }

    internal class SceneMainMenu
    {
        // Stores buttons
        List<Button> l_Buttons;

        public SceneMainMenu()
        {
            l_Buttons = new List<Button>();

            CreateButtons();
        }

        private void CreateButtons()
        {
            
        }

        // Called every frame from Update in Game1
        public void Update(float f_TimeSinceLastFrame)
        {

        }

        // Called every frame from Draw in Game1
        public void Render(SpriteBatch m_SpriteBatch)
        {

        }
    }
}
