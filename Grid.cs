using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;

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
        private Tile m_InvisibleTile;
        // Coordinates for where to draw the grid
        private Point m_GridCoordinates;

        // Used to only move tile once per keypress and not every frame
        private Dictionary<string, bool> m_MovementKeyStates;

        // Size in images (e.g. 3x3, 5x5 etc.) 
        private int i_Size;


        public Grid((int width, int height) t_ScreenDimensions, int size)
        {
            i_Size = size;
            // Creates 2D array 
            a_Tiles = new Tile[i_Size, i_Size];

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
            // TODO: ShuffleTiles();
        }

        // Called every frame from Game1 Update()
        public void Update()
        {
            HandleInputs();

            foreach (Tile tile in a_Tiles)
            {
                tile.Update();
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

        private void ShuffleTiles()
        {
            // TODO
        }

        // Handles all player inputs
        private void HandleInputs()
        {
            KeyboardState kstate = Keyboard.GetState();

            // Checks if keys are depressed
            CheckForKeysDepressed(kstate);

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
        private void SwapTiles(Point tile1Location, Point tile2Location)
        {
            // Simple swap with temp variable 
            Tile tempTile = a_Tiles[tile1Location.X, tile1Location.Y];
            MoveTile(a_Tiles[tile2Location.X, tile2Location.Y], tile2Location, tile1Location);
            MoveTile(tempTile, tile1Location, tile2Location);

            // Updates movecounter
            i_MovesMade += 1;
        }

        // Moves tile to different position in the grid and updates its location
        private void MoveTile(Tile tile, Point location, Point destination)
        {
            // Moves location on coordinate grid
            tile.m_GridCoordinates = destination;

            // Moves tile rectangle so it is drawn properly
            int i_TileSize = (int)(i_GridSideLength / i_Size);
            tile.m_Rectangle.Location = new Point(m_GridCoordinates.X + i_TileSize * destination.X, m_GridCoordinates.Y + i_TileSize * destination.Y);

            // Moves tile in tile array
            a_Tiles[destination.X, destination.Y] = tile;
        }

        // Creates all tiles to put into the grid
        private void CreateTiles()
        {
            int i_TileSize = (int)(i_GridSideLength / i_Size);
            // Each row of the grid
            for (int row = 0; row < i_Size; row++)
            {
                // Each column of the grid
                for (int column = 0; column < i_Size; column++)
                {
                    Rectangle CurrentTileRectangle = new Rectangle(m_GridCoordinates.X + i_TileSize * column, m_GridCoordinates.Y + i_TileSize * row, i_TileSize, i_TileSize);
                    Rectangle CurrentTileRegionRectangle = new Rectangle(column * i_TileSize, row * i_TileSize, i_TileSize, i_TileSize);
                    Point TileGridCoordinates = new Point(column, row);
                    Tile tile = new Tile(CurrentTileRectangle, CurrentTileRegionRectangle, TileGridCoordinates);
                    a_Tiles[column, row] = tile;
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
            a_Tiles[column, row].b_IsVisible = false;

            // Used for moving tile around
            m_InvisibleTile = a_Tiles[column, row];
        }
    }
}