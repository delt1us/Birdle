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
        protected static Vector2 vec_TextStartLocation = new Vector2(60, 140);

        // Grid object
        public Grid m_Grid;

        // Timer for level 
        public float f_TimeSpentOnPuzzle;

        // Font for drawing info on screen
        protected SpriteFont m_Font;

        public Button m_BackButton;
        public Level m_ActiveLevel;

        protected Texture2D m_BackgroundTexture;

        public SceneGame((int width, int height) t_SCREEN_DIMENSIONS, SpriteFont font, Texture2D backButtonTexture, SpriteFont backButtonFont, SpriteFont largeButtonFont, Texture2D backgroundTexture)
        {
            f_TimeSpentOnPuzzle = 0f;
            m_Font = font;
            m_BackButton = new Button(new Vector2(50, 850), backButtonTexture, backButtonTexture.Width, backButtonTexture.Height, "Back", backButtonFont, largeButtonFont);
            m_BackButton.m_TextColor = Color.White;

            m_BackgroundTexture = backgroundTexture;
        }

        public void Reset()
        {
            f_TimeSpentOnPuzzle = 0f;
            m_Grid.ShuffleTiles();
            m_Grid.b_Solved = false;
        }

        // Loads Grid object
        public void LoadGrid(Texture2D GridBorder, Texture2D GridTexture)
        {
            m_Grid.m_GridBorderTexture = GridBorder;
            m_Grid.m_Image = GridTexture;
        }

        // Called every frame from Update in Game1
        public virtual void Update(float f_TimeElapsed)
        {
            if (!m_Grid.b_Solved)
            {
                // Updates timer
                f_TimeSpentOnPuzzle += f_TimeElapsed;
            }
            // Updates grid
            m_Grid.Update(f_TimeElapsed);

            // Update back button
            m_BackButton.Update();
        }

        // Called every frame from Draw in Game1
        public void Render(SpriteBatch m_SpriteBatch)
        {
            m_SpriteBatch.Draw(m_BackgroundTexture, new Vector2(0, 0), null, Color.White);
            // Draws grid
            m_Grid.Render(m_SpriteBatch);
            // Draws solution
            m_SpriteBatch.Draw(m_Grid.m_GridBorderTexture, rec_SOLUTIONRECTANGLEBORDER, null, Color.White);
            m_SpriteBatch.Draw(m_Grid.m_Image, rec_SOLUTION_RECTANGLE, null, Color.White);

            DrawText(m_SpriteBatch);

            m_BackButton.Render(m_SpriteBatch);
        }

        // Draws Text
        protected virtual void DrawText(SpriteBatch m_SpriteBatch)
        {
            // Creates list
            List<string> l_StringsToDraw = new List<string>();
            // Adds to list
            l_StringsToDraw.Add($"Time: {(int)f_TimeSpentOnPuzzle}");
            l_StringsToDraw.Add($"Best time: {Math.Round((m_ActiveLevel.f_PersonalBestTime), 2).ToString()} seconds");
            l_StringsToDraw.Add($"Moves: {m_Grid.i_MovesMade}");
            l_StringsToDraw.Add($"Personal best: {m_ActiveLevel.i_PersonalBestMoves.ToString()}");
            l_StringsToDraw.Add($"Attemps: {m_ActiveLevel.i_Attempts.ToString()}");

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

        // Generates a new grid
        public void MakeNewGrid(Texture2D texture, Texture2D borderTexture, int size, (int width, int height) screenDimensions)
        {
            m_Grid = new Grid(screenDimensions, size);
            LoadGrid(borderTexture, texture);
        }
    }

    internal class SceneEndless : SceneGame
    {
        private List<Texture2D> l_Textures;
        private Texture2D m_GridBorderTexture;
        private (int width, int height) t_ScreenDimensions;
        public int i_PuzzlesCompleted;

        public SceneEndless((int width, int height) t_SCREEN_DIMENSIONS, SpriteFont font, Texture2D backButtonTexture, SpriteFont backButtonFont, SpriteFont largeButtonFont, Texture2D backgroundTexture, List<Texture2D> textures, Texture2D gridBorderTexture) : base(t_SCREEN_DIMENSIONS, font, backButtonTexture, backButtonFont, largeButtonFont, backgroundTexture)
        {
            l_Textures = textures;
            m_GridBorderTexture = gridBorderTexture;
            t_ScreenDimensions = t_SCREEN_DIMENSIONS;
            i_PuzzlesCompleted = 0;

            Random m_Random = new Random();
            MakeNewGrid(l_Textures[m_Random.Next(0, l_Textures.Count)], m_GridBorderTexture, 3, t_ScreenDimensions);
            m_Grid.ShuffleTiles();
        }

        public override void Update(float f_TimeElapsed)
        {
            f_TimeSpentOnPuzzle += f_TimeElapsed;

            if(m_Grid.b_Solved)
            {
                i_PuzzlesCompleted++;
                Random m_Random = new Random();
                MakeNewGrid(l_Textures[m_Random.Next(0, l_Textures.Count)], m_GridBorderTexture, 3, t_ScreenDimensions);
                m_Grid.ShuffleTiles();
            }

            m_Grid.Update(f_TimeElapsed);
            m_BackButton.Update();
        }

        protected override void DrawText(SpriteBatch m_SpriteBatch)
        {
            // Creates list
            List<string> l_StringsToDraw = new List<string>();
            // Adds to list
            l_StringsToDraw.Add($"Time: {(int)f_TimeSpentOnPuzzle}");
            l_StringsToDraw.Add($"Moves: {m_Grid.i_MovesMade}");
            l_StringsToDraw.Add($"PuzzlesFinished: {i_PuzzlesCompleted}");

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
        public Button m_CreditsButton;
        public Button m_QuitButton;
        private Texture2D m_ButtonTexture;
        private Texture2D m_TitleTexture;
        private Vector2 m_TitleLocation;
        private Texture2D m_BackgroundTexture;
        private Cloud[] a_Clouds;

        public SceneMainMenu((int width, int height) t_SCREEN_DIMENSIONS, Texture2D buttton_texture, SpriteFont m_ButtonFont, SpriteFont m_LargeButtonFont, Texture2D title_text_texture, Texture2D backgroundTexture, Texture2D cloudTextures)
        {
            l_Buttons = new List<Button>();
            m_ButtonTexture = buttton_texture;
            m_TitleTexture = title_text_texture;
            m_TitleLocation = new Vector2(((float)t_SCREEN_DIMENSIONS.width - m_TitleTexture.Width) / 2f, 150);
            m_BackgroundTexture = backgroundTexture;

            CreateClouds(cloudTextures);
            CreateButtons(m_ButtonFont, m_LargeButtonFont);
        }

        // Creates clouds
        private void CreateClouds(Texture2D cloudTextures)
        {  
            float cloudSpeedMultiplier = 1f;
            a_Clouds = new Cloud[5];
            Rectangle cloudArea = new Rectangle(1, 1, 501, 301);
            a_Clouds[0] = new Cloud(cloudTextures, new Vector2(20, 0), 1, 5f * cloudSpeedMultiplier, cloudArea);
            cloudArea.X = 503;
            a_Clouds[1] = new Cloud(cloudTextures, new Vector2(1730, 350), -1, 3f * cloudSpeedMultiplier, cloudArea);
            cloudArea.X = 1;
            cloudArea.Y = 303;
            a_Clouds[2] = new Cloud(cloudTextures, new Vector2(320, 110), 1, 6f * cloudSpeedMultiplier, cloudArea);
            cloudArea.X = 503;
            a_Clouds[3] = new Cloud(cloudTextures, new Vector2(40, 660), 1, 8f * cloudSpeedMultiplier, cloudArea);
            cloudArea.X = 1;
            cloudArea.Y = 605;
            a_Clouds[4] = new Cloud(cloudTextures, new Vector2(1320, 900), -1, 10f * cloudSpeedMultiplier, cloudArea);
        }

        // Creates and adds buttons to list 
        private void CreateButtons(SpriteFont m_ButtonFont, SpriteFont m_LargeButtonFont)
        {
            m_PlayButton = new Button(new Vector2(510, 400), m_ButtonTexture, m_ButtonTexture.Width, m_ButtonTexture.Height, "Play", m_ButtonFont, m_LargeButtonFont);
            l_Buttons.Add(m_PlayButton);
            m_EndlessButton = new Button(new Vector2(1000, 400), m_ButtonTexture, m_ButtonTexture.Width, m_ButtonTexture.Height, "Endless", m_ButtonFont, m_LargeButtonFont);
            l_Buttons.Add(m_EndlessButton);
            m_CreditsButton = new Button(new Vector2(750, 600), m_ButtonTexture, m_ButtonTexture.Width, m_ButtonTexture.Height, "Credits", m_ButtonFont, m_LargeButtonFont);
            l_Buttons.Add(m_CreditsButton);
            m_QuitButton = new Button(new Vector2(750, 800), m_ButtonTexture, m_ButtonTexture.Width, m_ButtonTexture.Height, "Quit", m_ButtonFont, m_LargeButtonFont);
            l_Buttons.Add(m_QuitButton);

            foreach (Button button in l_Buttons)
            {
                button.m_TextColor = Color.White;
            }
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

            foreach (Cloud cloud in a_Clouds)
            {
                cloud.Update(f_TimeSinceLastFrame);
            }
        }

        // Called every frame from Draw in Game1
        public void Render(SpriteBatch m_SpriteBatch)
        {
            // Draw background
            m_SpriteBatch.Draw(m_BackgroundTexture, new Vector2(0, 0), null, Color.White);

            foreach (Cloud cloud in a_Clouds)
            {
                cloud.Render(m_SpriteBatch);
            }

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

        private Texture2D m_BackgroundTexture;

        public SceneLevelSelect(Texture2D m_Level1ButtonTexture, Texture2D m_Level2ButtonTexture, Texture2D m_Level3ButtonTexture, Texture2D m_Level4ButtonTexture, Texture2D m_BackButtonTexture, SpriteFont m_ButtonFont, SpriteFont m_LargeButtonFont, Texture2D backgroundTexture)
        {
            l_Buttons = new List<Button>();
            CreateButtons(m_Level1ButtonTexture, m_Level2ButtonTexture, m_Level3ButtonTexture, m_Level4ButtonTexture, m_BackButtonTexture, m_ButtonFont, m_LargeButtonFont);

            m_BackgroundTexture = backgroundTexture;
        }

        // Creates all the button objects for this scene
        private void CreateButtons(Texture2D m_Level1ButtonTexture, Texture2D m_Level2ButtonTexture, Texture2D m_Level3ButtonTexture, Texture2D m_Level4ButtonTexture, Texture2D m_BackButtonTexture, SpriteFont m_ButtonFont, SpriteFont m_LargeButtonFont)
        {
            m_Level1Button = new Button(new Vector2(100, 650), m_Level1ButtonTexture, m_Level1ButtonTexture.Width, m_Level1ButtonTexture.Height, "level 1", m_ButtonFont, m_LargeButtonFont, false);
            l_Buttons.Add(m_Level1Button);
            m_Level2Button = new Button(new Vector2(500, 500), m_Level2ButtonTexture, m_Level2ButtonTexture.Width, m_Level2ButtonTexture.Height, "level 2", m_ButtonFont, m_LargeButtonFont, false);    
            l_Buttons.Add(m_Level2Button);
            m_Level3Button = new Button(new Vector2(1200, 900), m_Level3ButtonTexture, m_Level3ButtonTexture.Width, m_Level3ButtonTexture.Height, "level 3", m_ButtonFont, m_LargeButtonFont, false);
            l_Buttons.Add(m_Level3Button);
            m_Level4Button = new Button(new Vector2(120, 130), m_Level4ButtonTexture, m_Level4ButtonTexture.Width, m_Level4ButtonTexture.Height, "level 4", m_ButtonFont, m_LargeButtonFont, false);
            l_Buttons.Add(m_Level4Button);

            m_BackButton = new Button(new Vector2(50, 850), m_BackButtonTexture, m_BackButtonTexture.Width, m_BackButtonTexture.Height, "Back", m_ButtonFont, m_LargeButtonFont);
            m_BackButton.m_TextColor = Color.White;
        }

        // Called every frame from Game1
        public void Update(PlayerData m_PlayerData)
        {
            foreach (Button button in l_Buttons)
            {
                if (button.b_Active)
                {
                    button.Update();
                }
            }
            m_BackButton.Update();
        }

        // Called every frame from Game1
        public void Render(SpriteBatch m_SpriteBatch)
        {
            m_SpriteBatch.Draw(m_BackgroundTexture, new Vector2(0, 0), null, Color.White);

            foreach (Button button in l_Buttons)
            {
                button.Render(m_SpriteBatch);
            }
            m_BackButton.Render(m_SpriteBatch);
        }
    }

    internal class SceneCredits
    {
        public Button m_BackButton;
        private Texture2D m_BackgroundTexture;
        private string str_CreditString;
        private Texture2D m_TitleTexture;
        private SpriteFont m_Font;
        private Vector2 vec_TextLocation;

        public SceneCredits(Texture2D m_BackButtonTexture, SpriteFont m_ButtonFont, SpriteFont m_LargeButtonFont, Texture2D backgroundTexture, Texture2D titleTexture, SpriteFont font)
        {
            m_BackButton = new Button(new Vector2(50, 850), m_BackButtonTexture, m_BackButtonTexture.Width, m_BackButtonTexture.Height, "Back", m_ButtonFont, m_LargeButtonFont);
            m_BackButton.m_TextColor = Color.White;

            m_BackgroundTexture = backgroundTexture;
            m_TitleTexture = titleTexture;
            m_Font = font;

            str_CreditString = "Music by Rahetalius\nCheck licenses.txt for links";
            Vector2 stringSize = m_Font.MeasureString(str_CreditString);
            vec_TextLocation = new Vector2(220, 200 + stringSize.Y / 2f);
        }

        public void Update()
        {
            m_BackButton.Update();
        }

        public void Render(SpriteBatch m_SpriteBatch)
        {
            m_SpriteBatch.Draw(m_BackgroundTexture, new Vector2(0, 0), null, Color.White);
            m_SpriteBatch.Draw(m_TitleTexture, new Vector2(200, 150), null, Color.White);
            m_SpriteBatch.DrawString(m_Font, str_CreditString, vec_TextLocation, Color.White);
            m_BackButton.Render(m_SpriteBatch);
        }
    }
}
