﻿using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Newtonsoft.Json;

namespace Birdle
{
    public class Game1 : Game
    {
        // This is a tuple
        private static (int width, int height) t_SCREEN_DIMENSIONS = (1920, 1080);
        // Used for scaling everything down or up to screensize, doesn't do anything until I implement automatic fullscreening
        private static Vector2 vec_RenderScale = new Vector2(t_SCREEN_DIMENSIONS.width / 1920f, t_SCREEN_DIMENSIONS.height / 1080f);
        // Path to playerdata file
        private static string str_PATH = "playerdata.json";

        private static float f_SECONDS_FOR_LAST_TILE_TO_APPEAR = 3;

        private GraphicsDeviceManager m_Graphics;
        private SpriteBatch m_SpriteBatch;

        // Creates a Playerdata object which holds all the Level objects
        private PlayerData m_PlayerData;

        // String determining what gets rendered and updated
        private string str_GameState;

        // Scene objects that determines what happens during what gamestate
        private SceneGame m_SceneGame;
        private SceneMainMenu m_SceneMainMenu;
        private SceneLevelSelect m_SceneLevelSelect;
        private SceneCredits m_SceneCredits;
        private SceneEndless m_SceneEndless;

        private int i_ClickState;
        private bool b_GridSolved;

        private Song[] a_Songs;
        private SoundEffect m_TransitionSFX;
        private bool b_MusicPlaying;

        // Everything is drawn to the render m_RenderTarget "m_RenderTarget"
        private RenderTarget2D m_RenderTarget;

        public Game1()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            m_Graphics.PreferredBackBufferWidth = t_SCREEN_DIMENSIONS.width;
            m_Graphics.PreferredBackBufferHeight = t_SCREEN_DIMENSIONS.height;

            str_GameState = "main menu";

            i_ClickState = 0;
            b_GridSolved = false;
            a_Songs = new Song[10];
            MediaPlayer.Volume = 0.4f;
            b_MusicPlaying = false;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);

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

            // Main menu screen
            SpriteFont m_ButtonFont = Content.Load<SpriteFont>("Fonts/Button");
            SpriteFont m_LargeButtonFont = Content.Load<SpriteFont>("Fonts/ButtonLarge");
            Texture2D m_ButtonTexture = Content.Load<Texture2D>("Graphics/button");
            Texture2D m_TitleTextTexture = Content.Load<Texture2D>("Graphics/TitleText");
            Texture2D m_BackgroundTexture = Content.Load<Texture2D>("Graphics/title-screen");
            Texture2D m_CloudTextures = Content.Load<Texture2D>("Graphics/clouds");
            m_SceneMainMenu = new SceneMainMenu(t_SCREEN_DIMENSIONS, m_ButtonTexture, m_ButtonFont, m_LargeButtonFont, m_TitleTextTexture, m_BackgroundTexture, m_CloudTextures);

            // Actual game scene
            SpriteFont m_Font = Content.Load<SpriteFont>("Fonts/Default");
            Texture2D m_GameBackgroundTexture = Content.Load<Texture2D>("Graphics/game-background");
            m_SceneGame = new SceneGame(t_SCREEN_DIMENSIONS, m_Font, m_ButtonTexture, m_ButtonFont, m_LargeButtonFont, m_GameBackgroundTexture);

            // Endless game scene
            List<Texture2D> l_Textures = new List<Texture2D>();
            l_Textures.Add(Content.Load<Texture2D>("Graphics/level1"));
            l_Textures.Add(Content.Load<Texture2D>("Graphics/level2"));
            Texture2D m_GridBorderTexture = Content.Load<Texture2D>("Graphics/grid-border");
            m_SceneEndless = new SceneEndless(t_SCREEN_DIMENSIONS, m_Font, m_ButtonTexture, m_ButtonFont, m_LargeButtonFont, m_GameBackgroundTexture, l_Textures, m_GridBorderTexture);

            // Level select screen
            Texture2D m_Level1ButtonTexture = Content.Load<Texture2D>("Graphics/level1icon");
            Texture2D m_Level2ButtonTexture = Content.Load<Texture2D>("Graphics/level2icon");
            Texture2D m_Level3ButtonTexture = Content.Load<Texture2D>("Graphics/level3icon");
            Texture2D m_Level4ButtonTexture = Content.Load<Texture2D>("Graphics/level4icon");
            Texture2D m_LevelSelectBackgroundTexture = Content.Load<Texture2D>("Graphics/level-select-screen");
            m_SceneLevelSelect = new SceneLevelSelect(m_Level1ButtonTexture, m_Level2ButtonTexture, m_Level3ButtonTexture, m_Level4ButtonTexture, m_ButtonTexture, m_ButtonFont, m_LargeButtonFont, m_LevelSelectBackgroundTexture);

            // Credits screen
            Texture2D m_CreditsTitleText = Content.Load<Texture2D>("Graphics/CreditsText");
            m_SceneCredits = new SceneCredits(m_ButtonTexture, m_ButtonFont, m_LargeButtonFont, m_GameBackgroundTexture, m_CreditsTitleText, m_Font);

            // Loads music
            for (int i = 1; i < 11; i++)
            {
                a_Songs[i-1] = Content.Load<Song>($"Songs/{i.ToString()}");
            }

            m_TransitionSFX = Content.Load<SoundEffect>("sfx/funny slip noise");

            m_RenderTarget = new RenderTarget2D(GraphicsDevice, 1920, 1080);
        }
        // Runs every frame
        protected override void Update(GameTime gameTime)
        {
            // Add time since last frame to the timer
            float f_TimeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Checks mouse inputs to prevent 1 click pressing multiple buttons
            CheckMouseInputs();

            // Determines what to update
            if (str_GameState == "game")
            {
                m_SceneGame.Update(f_TimeElapsed);
                CheckButtonsInGame();
                if (!b_GridSolved)
                {
                    CheckSolved();
                }

                if (b_GridSolved && m_SceneGame.m_Grid.m_InvisibleTile.f_Opacity < 1f)
                {
                    m_SceneGame.m_Grid.m_InvisibleTile.f_Opacity += f_TimeElapsed * (1f / f_SECONDS_FOR_LAST_TILE_TO_APPEAR);
                    if (m_SceneGame.m_Grid.m_InvisibleTile.f_Opacity > 1f)
                    {
                        m_SceneGame.m_Grid.m_InvisibleTile.f_Opacity = 1f;
                    }
                }
            }

            else if (str_GameState == "main menu")
            {
                m_SceneMainMenu.Update(f_TimeElapsed);
                CheckButtonsInMainMenu();
            }

            else if (str_GameState == "level select")
            {
                m_SceneLevelSelect.Update(m_PlayerData);
                CheckButtonsInLevelSelect();
            }

            else if (str_GameState == "credits")
            {
                m_SceneCredits.Update();
                CheckButtonsInCredits();
            }

            else if (str_GameState == "endless")
            {
                m_SceneEndless.Update(f_TimeElapsed);
                CheckButtonsInEndless();
            }

            // Used for debug tools
            CheckKeyboardInputs();

            base.Update(gameTime);
        }

        // Should run when music ends
        void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            if (b_MusicPlaying)
            {
                PlayRandomSong();
            }
        }

        private void PlayTransitionSFX()
        {
            // m_TransitionSFX.CreateInstance().Play();
        }

        private void PlayRandomSong()
        {
            Random m_Random = new Random();
            MediaPlayer.Play(a_Songs[m_Random.Next(0, 10)]);
            b_MusicPlaying = true;
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
        }

        private void CheckButtonsInEndless()
        {
            if (m_SceneEndless.m_BackButton.b_Pressed)
            {
                SwitchTo("main menu");
                m_SceneEndless.m_BackButton.b_Pressed = false;
                b_MusicPlaying = false;
                MediaPlayer.Stop();
            }
        }

        private void CheckButtonsInCredits()
        {
            if (m_SceneCredits.m_BackButton.b_Pressed)
            {
                SwitchTo("main menu");
                m_SceneCredits.m_BackButton.b_Pressed = false;
            }
        }

        private void CheckKeyboardInputs()
        {
            KeyboardState kstate = Keyboard.GetState();
            // !For debugging 
            // Resets playerdata
            if (kstate.IsKeyDown(Keys.O))
            {
                CreateGameData();
            }
        }

        // Checks mouse inputs, called in update()
        private void CheckMouseInputs()
        {
            MouseState m_MouseState = Mouse.GetState();

            // The following is to prevent multiple things happening for a single click
            // ClickState = 0 means no click
            // ClickState = 1 means click started being held down this frame
            // ClickState = 2 means click started being held last frame
            // When ClickState = 2 buttons stop being checked

            // Resets click state when button is released
            if (m_MouseState.LeftButton == ButtonState.Released)
            {
                i_ClickState = 0;
            }

            // If mouse button is down
            else if (m_MouseState.LeftButton == ButtonState.Pressed)
            {
                // If no click last frame
                if (i_ClickState == 0)
                {
                    // Increases clickstate 
                    i_ClickState = 1;
                }
                // If click last frame
                if (i_ClickState == 1)
                {
                    // Increases clickstate
                    i_ClickState = 2;
                }
            }
        }

        // Checks if grid is solved
        // If so then saves data
        private void CheckSolved()
        {
            if (m_SceneGame.m_Grid.b_Solved)
            {
                SaveGameData();
                b_GridSolved = true;
            }
        }

        // Also runs every frame
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(m_RenderTarget);

            GraphicsDevice.Clear(Color.White);

            m_SpriteBatch.Begin();
            // Render things here

            // Draws everything when the game is running (player solving puzzle)
            if (str_GameState == "game")
            {
                m_SceneGame.Render(m_SpriteBatch);
            }
 
            else if (str_GameState == "main menu")
            {
                m_SceneMainMenu.Render(m_SpriteBatch);
            }

            else if (str_GameState == "level select")
            {
                m_SceneLevelSelect.Render(m_SpriteBatch);
            }

            else if (str_GameState == "credits")
            {
                m_SceneCredits.Render(m_SpriteBatch);
            }

            else if (str_GameState == "endless")
            {
                m_SceneEndless.Render(m_SpriteBatch);
            }

            m_SpriteBatch.End();

            // Reset Render target 
            GraphicsDevice.SetRenderTarget(null);

            m_SpriteBatch.Begin();

            // Draws render target so that everything scales properly
            m_SpriteBatch.Draw(m_RenderTarget, GraphicsDevice.Viewport.Bounds, Color.White);
            // m_SpriteBatch.Draw(m_RenderTarget, new Vector2(0, 0), null, Color.White, 0f, new Vector2(0, 0), vec_RenderScale, SpriteEffects.None, 0f);

            m_SpriteBatch.End();
            base.Draw(gameTime);
        }

        // Checks buttons in game
        private void CheckButtonsInGame()
        {
            // Prevent duplicate clicks from one input
            if (i_ClickState == 2)
            {
                return;
            }

            if (m_SceneGame.m_BackButton.b_Pressed)
            {
                SwitchTo("level select");
                m_SceneGame.m_BackButton.b_Pressed = false;
                b_MusicPlaying = false;
                MediaPlayer.Stop();
            }
        }

        // Checks buttons in level select and loads gamescene
        private void CheckButtonsInLevelSelect()
        {
            // Prevent duplicate clicks from one input
            if (i_ClickState == 2)
            {
                return;
            }

            bool b_ButtonPressed = true;
            int i_LevelIndex = 999;

            if (m_SceneLevelSelect.m_Level1Button.b_Pressed)
            {
                // Load level 1
                i_LevelIndex = 0;
            }

            else if (m_SceneLevelSelect.m_Level2Button.b_Pressed)
            {
                // Load level 2
                i_LevelIndex = 1;
            }

            else if (m_SceneLevelSelect.m_Level3Button.b_Pressed)
            {
                // Load level 3
                i_LevelIndex = 2;
            }

            else if (m_SceneLevelSelect.m_Level4Button.b_Pressed)
            {
                // Load level 4
                i_LevelIndex = 3;
            }

            // Checks for back button being pressed
            else if (m_SceneLevelSelect.m_BackButton.b_Pressed)
            {
                // Sets game to main menu
                SwitchTo("main menu");
                // To prevent level being started without texture                
                b_ButtonPressed = false;
                // Resets back button for future use
                m_SceneLevelSelect.m_BackButton.b_Pressed = false;
            }

            else
            {
                b_ButtonPressed = false;
            }

            // If level is selected
            if (b_ButtonPressed)
            {
                StartGame(i_LevelIndex);
            }
        }

        // Method to start the game with specified level
        private void StartGame(int i_LevelIndex)
        {
            LoadGameData();

            Texture2D gridTexture = Content.Load<Texture2D>($"Graphics/{m_PlayerData.a_Levels[i_LevelIndex].str_ImagePath}");
            Texture2D borderTexture = Content.Load<Texture2D>("Graphics/grid-border");
            m_SceneGame.MakeNewGrid(gridTexture, borderTexture, m_PlayerData.a_Levels[i_LevelIndex].i_GridSize, t_SCREEN_DIMENSIONS);
            m_SceneGame.m_ActiveLevel = m_PlayerData.a_Levels[i_LevelIndex];
            b_GridSolved = false;  

            // Resets game (sets timer to 0 and shuffles tiles)
            m_SceneGame.Reset();
            SwitchTo("game");

            // Resets level select
            foreach (Button button in m_SceneLevelSelect.l_Buttons)
            {
                button.b_Pressed = false;
            }
        }

        private void SwitchTo(string newGamestate)
        {
            str_GameState = newGamestate;
            if (newGamestate == "level select")
            {
                SetLevelsActive();
            }
            else if (newGamestate == "game" || newGamestate == "endless")
            {
                PlayRandomSong();
            }

            PlayTransitionSFX();
        }

        // Checks buttons in main menu and changes gamestate accordingly
        private void CheckButtonsInMainMenu()
        {
            // Prevent duplicate clicks from one input
            if (i_ClickState == 2)
            {
                return;
            }

            // Checks buttons being pressed
            if (m_SceneMainMenu.m_PlayButton.b_Pressed)
            {
                SwitchTo("level select");
            }
            else if (m_SceneMainMenu.m_EndlessButton.b_Pressed)
            {
                SwitchTo("endless");
            }
            else if (m_SceneMainMenu.m_CreditsButton.b_Pressed)
            {
                SwitchTo("credits");
            }
            // Exits the game
            else if (m_SceneMainMenu.m_QuitButton.b_Pressed)
            {
                Exit();
            }
            m_SceneMainMenu.ResetButtons();
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

        // Used before level select is shown to determine what levels should be shown as unlocked
        private void SetLevelsActive()
        {
            // Checks what levels are unlocked
            for (int level = 0; level < m_SceneLevelSelect.l_Buttons.Count - 1; level++)
            {
                if (m_PlayerData.a_Levels[level].b_Completed)
                {
                    m_SceneLevelSelect.l_Buttons[level + 1].b_Active = true;
                }
                else
                {
                    m_SceneLevelSelect.l_Buttons[level + 1].b_Active = false;
                }
            }
        }

        // Updates playerdata, called when game ends
        private void UpdatePlayerData()
        {
            if (m_SceneGame.m_ActiveLevel == null)
            {
                return;
            }
            if (m_SceneGame.f_TimeSpentOnPuzzle < m_SceneGame.m_ActiveLevel.f_PersonalBestTime || m_SceneGame.m_ActiveLevel.f_PersonalBestTime == 0)
            {
                m_SceneGame.m_ActiveLevel.f_PersonalBestTime = m_SceneGame.f_TimeSpentOnPuzzle;
            }
            if (m_SceneGame.m_Grid.i_MovesMade < m_SceneGame.m_ActiveLevel.i_PersonalBestMoves || m_SceneGame.m_ActiveLevel.i_PersonalBestMoves == 0)
            {
                m_SceneGame.m_ActiveLevel.i_PersonalBestMoves = m_SceneGame.m_Grid.i_MovesMade;
            }

            m_SceneGame.m_ActiveLevel.i_Attempts += 1;
            m_SceneGame.m_ActiveLevel.b_Completed = true;
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