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
        // Path to playerdata file
        private static string str_PATH = "playerdata.json";

        private GraphicsDeviceManager m_Graphics;
        private SpriteBatch m_SpriteBatch;

        // Creates a Playerdata object which holds all the Level objects
        private PlayerData m_PlayerData;

        // String determining what gets rendered and updated
        private string str_GameState;

        // Scene object that determines what happens during what gamestate
        private SceneGame m_SceneGame;

        public Game1()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            m_Graphics.PreferredBackBufferWidth = t_SCREEN_DIMENSIONS.width;
            m_Graphics.PreferredBackBufferHeight = t_SCREEN_DIMENSIONS.height;

            str_GameState = "game";
            m_SceneGame = new SceneGame(t_SCREEN_DIMENSIONS);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);

            // Loads grid
            Texture2D m_GridBorder = Content.Load<Texture2D>("Graphics/grid-border");
            Texture2D m_GridTexture = Content.Load<Texture2D>("Graphics/numbered-grid");
            m_SceneGame.LoadGrid(m_GridBorder, m_GridTexture);

            // Loads game font
            SpriteFont m_Font = Content.Load<SpriteFont>("Fonts/Default");
            m_SceneGame.LoadFont(m_Font);

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
            float f_TimeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Determines what to update
            if (str_GameState == "game")
            {
                m_SceneGame.Update(f_TimeElapsed);
            }

            base.Update(gameTime);
        }
        // Also runs every frame
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            m_SpriteBatch.Begin();
            // Render things here

            // Draws everything when the game is running (player solving puzzle)
            if (str_GameState == "game")
            {
                m_SceneGame.Render(m_SpriteBatch, m_PlayerData);
            }

            m_SpriteBatch.End();

            base.Draw(gameTime);
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
            if (m_SceneGame.f_TimeSpentOnPuzzle < m_PlayerData.a_Levels[0].f_PersonalBestTime || m_PlayerData.a_Levels[0].f_PersonalBestTime == 0)
            {
                m_PlayerData.a_Levels[0].f_PersonalBestTime = m_SceneGame.f_TimeSpentOnPuzzle;
            }
            if (m_SceneGame.m_Grid.i_MovesMade < m_PlayerData.a_Levels[0].i_PersonalBestMoves || m_PlayerData.a_Levels[0].i_PersonalBestMoves == 0)
            {
                m_PlayerData.a_Levels[0].i_PersonalBestMoves = m_SceneGame.m_Grid.i_MovesMade;
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