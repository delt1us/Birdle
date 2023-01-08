using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Birdle
{
    public class Game1 : Game
    {
        // This is a tuple
        private static (int width, int height) t_SCREEN_DIMENSIONS = (1920, 1080);
        // Used for the solution preview
        private static Rectangle rec_SOLUTIONRECTANGLE = new Rectangle(1450, 140, 400, 400);
        private static Rectangle rec_SOLUTIONRECTANGLEBORDER = new Rectangle(rec_SOLUTIONRECTANGLE.X - 5, rec_SOLUTIONRECTANGLE.Y - 5, 410, 410);
        // Path to playerdata file
        private static string str_PATH = "playerdata.json";
        private static Vector2 vec_TextStartLocation = new Vector2(60, 140);
        // Determines the dimensions of the grid (not scale)
        private const int i_GRIDSIZE = 3;

        private GraphicsDeviceManager m_Graphics;
        private SpriteBatch m_SpriteBatch;
        private Grid m_Grid;

        // Font for drawing info on screen
        private SpriteFont m_Font;
        // Timer
        private float f_TimeElapsed;
        // Creates a Playerdata object which holds all the Level objects
        private PlayerData m_PlayerData;

        public Game1()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            m_Grid = new Grid(t_SCREEN_DIMENSIONS, i_GRIDSIZE);

            m_Graphics.PreferredBackBufferWidth = t_SCREEN_DIMENSIONS.width;
            m_Graphics.PreferredBackBufferHeight = t_SCREEN_DIMENSIONS.height;

            f_TimeElapsed = 0f;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);

            LoadGrid();
            LoadFont();

            // Game data (information like high score, best time etc)
            // Check if file exists already
            if (File.Exists(str_PATH))
            {
                LoadGameData();
            }
            else
            {
                CreateGameData();
            }
        }
        // Runs every frame
        protected override void Update(GameTime gameTime)
        {
            // Add time since last frame to the timer
            f_TimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            m_Grid.Update();

            base.Update(gameTime);
        }
        // Also runs every frame
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            m_SpriteBatch.Begin();
            // Render things here
            // Draws grid
            m_Grid.Render(m_SpriteBatch);
            // Draws solution
            m_SpriteBatch.Draw(m_Grid.m_GridBorderTexture, rec_SOLUTIONRECTANGLEBORDER, null, Color.White);
            m_SpriteBatch.Draw(m_Grid.m_Image, rec_SOLUTIONRECTANGLE, null, Color.White);

            DrawText(m_SpriteBatch);

            m_SpriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawText(SpriteBatch m_SpriteBatch)
        {
            // Creates list
            List<String> l_StringsToDraw = new List<string>();
            // Adds to list
            l_StringsToDraw.Add($"Time: {(int)f_TimeElapsed}");
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

        // Loads Grid object
        private void LoadGrid()
        {
            m_Grid.m_GridBorderTexture = Content.Load<Texture2D>("Graphics/grid-border");
            m_Grid.m_Image = Content.Load<Texture2D>("Graphics/numbered-grid");
        }
        // Loads font
        private void LoadFont()
        {
            m_Font = Content.Load<SpriteFont>("Fonts/Default");

        }

        // Loads user data
        private void LoadGameData()
        {
            // Reads from file 
            string str_PlayerDataJson = File.ReadAllText(str_PATH);
            // Converts to an object
            m_PlayerData = JsonConvert.DeserializeObject<PlayerData>(str_PlayerDataJson);
        }

        // Saves user data
        private void SaveGameData()
        {
            UpdatePlayerData();
            // Converts m_PlayerData into a string in json format
            string str_PlayerDataJson = JsonConvert.SerializeObject(m_PlayerData);
            // Creates/Overwrites file with new contents
            File.WriteAllText(str_PATH, str_PlayerDataJson);
        }

        // Creates settings.json with default settings
        private void CreateGameData()
        {
            m_PlayerData = new PlayerData();
            // Converts m_PlayerData into a string in json format
            string str_PlayerDataJson = JsonConvert.SerializeObject(m_PlayerData);
            // Creates/Overwrites file with new contents
            File.WriteAllText(str_PATH, str_PlayerDataJson);
        }

        // Updates playerdata, called when game ends
        private void UpdatePlayerData()
        {
            if (f_TimeElapsed < m_PlayerData.a_Levels[0].f_PersonalBestTime || m_PlayerData.a_Levels[0].f_PersonalBestTime == 0)
            {
                m_PlayerData.a_Levels[0].f_PersonalBestTime = f_TimeElapsed;
            }
            if (m_Grid.i_MovesMade < m_PlayerData.a_Levels[0].i_PersonalBestMoves || m_PlayerData.a_Levels[0].i_PersonalBestMoves == 0)
            {
                m_PlayerData.a_Levels[0].i_PersonalBestMoves = m_Grid.i_MovesMade;
            }
            m_PlayerData.a_Levels[0].i_Attempts += 1;
        }

        // Override this so that game saves when the player quits the game
        protected override void OnExiting(object sender, EventArgs args)
        {
            // Saves game
            SaveGameData();
            base.OnExiting(sender, args);
        }
    }
}