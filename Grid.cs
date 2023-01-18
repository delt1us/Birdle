using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Birdle
{
    internal class Grid
    {
        private static int i_GridSideLength = 900;

        // Number of moves made
        public int i_MovesMade;

        public Texture2D m_Image;
        public Texture2D m_GridBorderTexture;

        private Vector2 vec_GridBorderCoordinates;

        // Comma indicates a 2D array
        private Tile[,] a_Tiles;
        public Tile m_InvisibleTile;
        // Coordinates for where to draw the grid
        private Point m_GridCoordinates;

        // Used to only move tile once per keypress and not every frame
        private Dictionary<string, bool> m_MovementKeyStates;

        // Size in images (e.g. 3x3, 5x5 etc.) 
        private int i_Size;

        public bool b_Solved;

        public Grid((int width, int height) t_ScreenDimensions, int size)
        {
            i_Size = size;
            // Creates 2D array 
            a_Tiles = new Tile[i_Size, i_Size];

            b_Solved = false;   

            // Centers the grid
            m_GridCoordinates = new Point((int)((t_ScreenDimensions.width - i_GridSideLength) / 2f), (int)((t_ScreenDimensions.height - i_GridSideLength) / 2f));
            vec_GridBorderCoordinates = new Vector2((float)m_GridCoordinates.X - 10, (float)m_GridCoordinates.Y - 10);

            // Used to ensure tile isn't moved multiple times per keypress
            m_MovementKeyStates = new Dictionary<string, bool>()
            {
                {"W", false},
                {"A", false},
                {"S", false},
                {"D", false}
            };

            i_MovesMade = 0;

            CreateTiles();
            SetRandomTileInvisible();
        }

        // Called every frame from Game1 Update()
        public void Update(float f_TimeElapsed)
        {
            HandleInputs();

            foreach (Tile tile in a_Tiles)
            {
                tile.Update(f_TimeElapsed);
            }

            // If solved
            if (CheckIfSolved())
            {
                b_Solved = true;
            }
        }

        // This method 
        public void Render(SpriteBatch m_SpriteBatch)
        {
            // Draws the grid
            m_SpriteBatch.Draw(m_GridBorderTexture, vec_GridBorderCoordinates, Color.White);

            foreach (Tile tile in a_Tiles)
            {
                tile.Render(m_SpriteBatch, m_Image);
            }
        }

        // Checks if everything is in the right place, called every frame from Grid.Update()
        private bool CheckIfSolved()
        {
            bool solved = true;
            for (int row = 0; row < i_Size; row++)
            {
                for (int column = 0; column < i_Size; column++)
                {
                    if (a_Tiles[row, column].m_OriginalLocation != new Point(row, column))
                    {
                        solved = false;
                    }
                }
            }
            return solved;
        }

        // Shuffles tiles
        public void ShuffleTiles()
        {
            Random m_Random = new Random();
            bool b_Moved = false;
            for (int i = 0; i < 100; i++)
            {
                // Move tile randomly
                b_Moved = false;
                while (!b_Moved)
                {
                    // Determines direction to move tile
                    int i_move = m_Random.Next(1, 5);
                    Debug.WriteLine($"Random number: {i_move}");
                    b_Moved = true;

                    // up
                    if (i_move == 1 && m_InvisibleTile.m_GridCoordinates.Y > 0)
                    {
                        SwapTiles(m_InvisibleTile.m_GridCoordinates, new Point(m_InvisibleTile.m_GridCoordinates.X, m_InvisibleTile.m_GridCoordinates.Y - 1), false);
                    }

                    // right
                    else if (i_move == 2 && m_InvisibleTile.m_GridCoordinates.X < i_Size - 1)
                    {
                        SwapTiles(m_InvisibleTile.m_GridCoordinates, new Point(m_InvisibleTile.m_GridCoordinates.X + 1, m_InvisibleTile.m_GridCoordinates.Y), false);
                    }

                    // down
                    else if (i_move == 3 && m_InvisibleTile.m_GridCoordinates.Y < i_Size - 1)
                    {
                        SwapTiles(m_InvisibleTile.m_GridCoordinates, new Point(m_InvisibleTile.m_GridCoordinates.X, m_InvisibleTile.m_GridCoordinates.Y + 1), false);
                    }

                    // left
                    else if (i_move == 4  && m_InvisibleTile.m_GridCoordinates.X > 0)
                    {
                        SwapTiles(m_InvisibleTile.m_GridCoordinates, new Point(m_InvisibleTile.m_GridCoordinates.X - 1, m_InvisibleTile.m_GridCoordinates.Y), false);
                    }
                    else
                    {
                        b_Moved = false;
                    }
                }
            }

            foreach (Tile tile in a_Tiles)
            {
                tile.UpdateLocation(0f, true);
            }
        }

        // Handles all player inputs
        private void HandleInputs()
        {
            KeyboardState kstate = Keyboard.GetState();

            // Checks if keys are depressed
            CheckForKeysDepressed(kstate);

            // !Debug tool
            if (kstate.IsKeyDown(Keys.I))
            {
                b_Solved = true;
            }
            if (kstate.IsKeyDown(Keys.P))
            {
                ShuffleTiles();
            }

            // Move tile up
            if (kstate.IsKeyDown(Keys.W) && m_MovementKeyStates["W"] == false)
            {
                m_MovementKeyStates["W"] = true;

                if (m_InvisibleTile.m_GridCoordinates.Y > 0)
                {
                    Point targetCoordinates = new Point(m_InvisibleTile.m_GridCoordinates.X, m_InvisibleTile.m_GridCoordinates.Y - 1);
                    SwapTiles(m_InvisibleTile.m_GridCoordinates, targetCoordinates);
                }
            }

            // Move tile left
            if (kstate.IsKeyDown(Keys.A) && m_MovementKeyStates["A"] == false)
            {
                // To stop it happening every frame
                m_MovementKeyStates["A"] = true;

                // Check if tile isn't in leftmost position
                if (m_InvisibleTile.m_GridCoordinates.X > 0)
                {
                    // Swaps tile with tile on the right
                    Point targetCoordinates = new Point(m_InvisibleTile.m_GridCoordinates.X - 1, m_InvisibleTile.m_GridCoordinates.Y);
                    SwapTiles(m_InvisibleTile.m_GridCoordinates, targetCoordinates);
                }
            }

            // Move tile right
            if (kstate.IsKeyDown(Keys.D) && m_MovementKeyStates["D"] == false)
            {
                m_MovementKeyStates["D"] = true;

                if (m_InvisibleTile.m_GridCoordinates.X < i_Size - 1)
                {
                    Point targetCoordinates = new Point(m_InvisibleTile.m_GridCoordinates.X + 1, m_InvisibleTile.m_GridCoordinates.Y);
                    SwapTiles(m_InvisibleTile.m_GridCoordinates, targetCoordinates);
                }
            }

            // Move tile down
            if (kstate.IsKeyDown(Keys.S) && m_MovementKeyStates["S"] == false)
            {
                m_MovementKeyStates["S"] = true;

                if (m_InvisibleTile.m_GridCoordinates.Y < i_Size - 1)
                {
                    Point targetCoordinates = new Point(m_InvisibleTile.m_GridCoordinates.X, m_InvisibleTile.m_GridCoordinates.Y + 1);
                    SwapTiles(m_InvisibleTile.m_GridCoordinates, targetCoordinates);
                }
            }
        }

        // Checks if player has let go of any keys
        private void CheckForKeysDepressed(KeyboardState kstate)
        {
            if (kstate.IsKeyUp(Keys.W))
            {
                m_MovementKeyStates["W"] = false;
            }
            if (kstate.IsKeyUp(Keys.A))
            {
                m_MovementKeyStates["A"] = false;
            }
            if (kstate.IsKeyUp(Keys.S))
            {
                m_MovementKeyStates["S"] = false;
            }
            if (kstate.IsKeyUp(Keys.D))
            {
                m_MovementKeyStates["D"] = false;
            }
        }

        // Swaps 2 tiles given their coordinates 
        private void SwapTiles(Point tile1Location, Point tile2Location, bool increaseMoveCounter = true)
        {
            // Simple swap with temp variable 
            Tile tempTile = a_Tiles[tile1Location.X, tile1Location.Y];
            MoveTile(a_Tiles[tile2Location.X, tile2Location.Y], tile1Location);
            MoveTile(tempTile, tile2Location);

            if (increaseMoveCounter)
            {
                // Updates movecounter
                i_MovesMade += 1;
            }
        }

        // Moves tile to different position in the grid and updates its location
        private void MoveTile(Tile tile, Point destination)
        {
            // Moves location on coordinate grid
            tile.m_GridCoordinates = destination;

            // Moves tile rectangle so it is drawn properly
            int i_TileSize = (int)(i_GridSideLength / i_Size);
            tile.m_TargetLocation = new Point(m_GridCoordinates.X + i_TileSize * destination.X, m_GridCoordinates.Y + i_TileSize * destination.Y);

            // Moves tile in tile array
            a_Tiles[destination.X, destination.Y] = tile;
        }

        // Creates all tiles to put into the grid
        private void CreateTiles()
        {
            int i_TileSize = (int)(i_GridSideLength / i_Size);
            // Each row of the grid
            for (int y = 0; y < i_Size; y++)
            {
                // Each column of the grid
                for (int x = 0; x < i_Size; x++)
                {
                    Rectangle CurrentTileRectangle = new Rectangle(m_GridCoordinates.X + i_TileSize * x, m_GridCoordinates.Y + i_TileSize * y, i_TileSize, i_TileSize);
                    Rectangle CurrentTileRegionRectangle = new Rectangle(x * i_TileSize, y * i_TileSize, i_TileSize, i_TileSize);
                    Point TileGridCoordinates = new Point(x, y);
                    Tile tile = new Tile(CurrentTileRectangle, CurrentTileRegionRectangle, TileGridCoordinates, new Point(x, y));
                    a_Tiles[x, y] = tile;
                }
            }
        }

        // Sets a random tile to be invisible
        private void SetRandomTileInvisible()
        {
            // Picks a random tile
            Random m_Random = new Random();
            int row = m_Random.Next(0, i_Size);
            int column = m_Random.Next(0, i_Size);

            // Makes tile be invisible
            a_Tiles[column, row].f_Opacity = 0f;

            // Used for moving tile around
            m_InvisibleTile = a_Tiles[column, row];
        }
    }
}