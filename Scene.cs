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

        public Button m_BackButton;

        public SceneGame((int width, int height) t_SCREEN_DIMENSIONS, SpriteFont font, Texture2D backButtonTexture, SpriteFont backButtonFont)
        {
            m_Grid = new Grid(t_SCREEN_DIMENSIONS, i_GRIDSIZE);
            f_TimeSpentOnPuzzle = 0f;

            m_Font = font;

            m_BackButton = new Button(new Vector2(50, 850), backButtonTexture, backButtonTexture.Width, backButtonTexture.Height, "Back", backButtonFont);
        }

        public void Reset()
        {
            f_TimeSpentOnPuzzle = 0f;
            m_Grid.ShuffleTiles();
        }

        // Loads Grid object
        public void LoadGrid(Texture2D GridBorder, Texture2D GridTexture)
        {
            m_Grid.m_GridBorderTexture = GridBorder;
            m_Grid.m_Image = GridTexture;
        }

        // Called every frame from Update in Game1
        public void Update(float f_TimeElapsed)
        {
            // Updates timer
            f_TimeSpentOnPuzzle += f_TimeElapsed;
            // Updates grid
            m_Grid.Update();

            // Update back button
            m_BackButton.Update();
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

            m_BackButton.Render(m_SpriteBatch);
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

    // The main menu
    internal class SceneMainMenu
    {
        // Stores buttons
        private List<Button> l_Buttons;
        public Button m_PlayButton;
        public Button m_EndlessButton;
        public Button m_SettingsButton;
        public Button m_QuitButton;
        private Texture2D m_ButtonTexture;
        private Texture2D m_TitleTexture;
        private Vector2 m_TitleLocation;

        public SceneMainMenu((int width, int height) t_SCREEN_DIMENSIONS, Texture2D buttton_texture, SpriteFont m_ButtonFont, Texture2D title_text_texture)
        {
            l_Buttons = new List<Button>();
            m_ButtonTexture = buttton_texture;
            m_TitleTexture = title_text_texture;
            m_TitleLocation = new Vector2(((float)t_SCREEN_DIMENSIONS.width - m_TitleTexture.Width) / 2f, 150);

            CreateButtons(m_ButtonFont);
        }

        // Creates and adds buttons to list 
        public void CreateButtons(SpriteFont m_ButtonFont)
        {
            m_PlayButton = new Button(new Vector2(510, 400), m_ButtonTexture, m_ButtonTexture.Width, m_ButtonTexture.Height, "Play", m_ButtonFont);
            l_Buttons.Add(m_PlayButton);
            m_EndlessButton = new Button(new Vector2(1000, 400), m_ButtonTexture, m_ButtonTexture.Width, m_ButtonTexture.Height, "Endless", m_ButtonFont);
            l_Buttons.Add(m_EndlessButton);
            m_SettingsButton = new Button(new Vector2(750, 600), m_ButtonTexture, m_ButtonTexture.Width, m_ButtonTexture.Height, "Settings", m_ButtonFont);
            l_Buttons.Add(m_SettingsButton);
            m_QuitButton = new Button(new Vector2(750, 800), m_ButtonTexture, m_ButtonTexture.Width, m_ButtonTexture.Height, "Quit", m_ButtonFont);
            l_Buttons.Add(m_QuitButton);
        }

        // Resets pressed value of all buttons to false
        public void ResetButtons()
        {
            foreach (Button button in l_Buttons)
            {
                button.b_Pressed = false;
            }
        }

        // Called every frame from Update in Game1
        public void Update(float f_TimeSinceLastFrame)
        {
            foreach (Button button in l_Buttons)
            {
                button.Update();
            }
        }

        // Called every frame from Draw in Game1
        public void Render(SpriteBatch m_SpriteBatch)
        {
            foreach (Button button in l_Buttons)
            {
                button.Render(m_SpriteBatch);
            }

            // Title text
            m_SpriteBatch.Draw(m_TitleTexture, m_TitleLocation, Color.White);
        }
    }

    // Scene for the level select
    internal class SceneLevelSelect
    {
        public List<Button> l_Buttons;
        public Button m_Level1Button;
        public Button m_Level2Button;
        public Button m_Level3Button;
        public Button m_Level4Button;
        public Button m_BackButton;

        public SceneLevelSelect(Texture2D m_Level1ButtonTexture, Texture2D m_Level2ButtonTexture, Texture2D m_Level3ButtonTexture, Texture2D m_Level4ButtonTexture, Texture2D m_BackButtonTexture, SpriteFont m_ButtonFont)
        {
            l_Buttons = new List<Button>();
            CreateButtons(m_Level1ButtonTexture, m_Level2ButtonTexture, m_Level3ButtonTexture, m_Level4ButtonTexture, m_BackButtonTexture, m_ButtonFont);
        }

        // Creates all the button objects for this scene
        private void CreateButtons(Texture2D m_Level1ButtonTexture, Texture2D m_Level2ButtonTexture, Texture2D m_Level3ButtonTexture, Texture2D m_Level4ButtonTexture, Texture2D m_BackButtonTexture, SpriteFont m_ButtonFont)
        {
            m_Level1Button = new Button(new Vector2(100, 650), m_Level1ButtonTexture, m_Level1ButtonTexture.Width, m_Level1ButtonTexture.Height, "", m_ButtonFont);
            l_Buttons.Add(m_Level1Button);
            m_Level2Button = new Button(new Vector2(500, 500), m_Level2ButtonTexture, m_Level2ButtonTexture.Width, m_Level2ButtonTexture.Height, "", m_ButtonFont);    
            l_Buttons.Add(m_Level2Button);
            m_Level3Button = new Button(new Vector2(1200, 900), m_Level3ButtonTexture, m_Level3ButtonTexture.Width, m_Level3ButtonTexture.Height, "", m_ButtonFont);
            l_Buttons.Add(m_Level3Button);
            m_Level4Button = new Button(new Vector2(120, 130), m_Level4ButtonTexture, m_Level4ButtonTexture.Width, m_Level4ButtonTexture.Height, "", m_ButtonFont);
            l_Buttons.Add(m_Level4Button);

            m_BackButton = new Button(new Vector2(50, 850), m_BackButtonTexture, m_BackButtonTexture.Width, m_BackButtonTexture.Height, "Back", m_ButtonFont);
            l_Buttons.Add(m_BackButton);
        }

        // Called every frame from Game1
        public void Update()
        {
            foreach (Button button in l_Buttons)
            {
                button.Update();
            }
        }

        // Called every frame from Game1
        public void Render(SpriteBatch m_SpriteBatch)
        {
            foreach (Button button in l_Buttons)
            {
                button.Render(m_SpriteBatch);
            }
        }
    }
}
