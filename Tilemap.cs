using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace TimeGame
{
    public class Tilemap
    {
        /// <summary>
        /// The dimensions of tils and the map
        /// </summary>
        int _tileWidth, _tileHeight, _mapWidth, _mapHeight;
        int numTilesHeight, numTilesWidth;

        /// <summary>
        /// The tileset texture
        /// </summary>
        Texture2D _tilesetTexture;

        /// <summary>
        /// The tile info in the tileset
        /// </summary>
        Rectangle[] _tiles;

        /// <summary>
        /// The tile map data
        /// </summary>
        int[,] _map;

        /// <summary>
        /// The filename of the map
        /// </summary>
        string _filename;
        /// <summary>
        /// Creates a tilemap
        /// </summary>
        /// <param name="width">width of game</param>
        /// <param name="height">height of game</param>
        public Tilemap(int width, int height)
        {
            _tileWidth = 64;
            _tileHeight = 64;
            _mapWidth = width;
            _mapHeight = height;
        }

        public void newFrame()
        {
            Random rand = new Random();
            for (int y = 0; y < numTilesHeight; y++)
            {
                for (int x = 0; x < numTilesWidth; x++)
                {
                    int index = y * numTilesWidth + x;
                    if (x == numTilesWidth - 1)
                    {
                        if (y == 0)
                        {
                            _map[index, 0] = rand.Next(0, 5);
                            _map[index, 1] = 0;
                        }
                        else if (y == numTilesHeight - 1)
                        {
                            _map[index, 0] = rand.Next(0, 5);
                            _map[index, 1] = 2;
                        }
                        else
                        {
                            _map[index, 0] = 5 + rand.Next(0, 5);
                            _map[index, 1] = rand.Next(0, 5);
                        }
                    }
                    else
                    {
                        if(index < numTilesHeight * numTilesWidth-1)
                        {
                            _map[index, 0] = _map[index+1, 0];
                            _map[index, 1] = _map[index+1, 1];
                        }
                    }
                }
            }
        }

        public void LoadContent(ContentManager content)
        {
            _tilesetTexture = content.Load<Texture2D>("TileMap");

            //Now we can determine our tile bounds
            int tilesetColumns = _tilesetTexture.Width / _tileWidth;
            int tilesetRows = _tilesetTexture.Height / _tileHeight;

            _tiles = new Rectangle[tilesetColumns * tilesetRows];

            for (int x = 0; x < tilesetRows; x++)
            {
                for (int y = 0; y < tilesetColumns; y++)
                {
                    int index = y + x * tilesetColumns;
                    _tiles[index] = new Rectangle(
                        y * _tileWidth,
                        x * _tileHeight,
                        _tileWidth,
                        _tileHeight
                        );
                }
            }

            //Now we can create our map
            numTilesHeight = (_mapHeight / _tileHeight);
            numTilesWidth = (_mapWidth / _tileWidth) + 2;
            _map = new int[numTilesWidth * numTilesHeight, 2];

            Random rand = new Random();
            for (int y = 0; y < numTilesHeight; y++)
            {
                for (int x = 0; x < numTilesWidth; x++)
                {
                    int index = y * numTilesWidth + x;
                    if (y == 0)
                    {
                        _map[index, 0] = rand.Next(0, 5);
                        _map[index, 1] = 0;
                    }
                    else if (y == numTilesHeight-1)
                    {
                        _map[index, 0] = rand.Next(0, 5);
                        _map[index, 1] = 2;
                    }
                    else
                    {
                        _map[index, 0] = 5 + rand.Next(0, 5);
                        _map[index, 1] = rand.Next(0, 4);
                    }
                }
            }
            /*
            var fourthLine = lines[3].Split(',');
            _map = new int[_mapWidth * _mapHeight];
            for (int i = 0; i < _mapWidth * _mapHeight; i++)
            {
                _map[i] = int.Parse(fourthLine[i]);
            }
            */
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int y = 0; y < numTilesHeight; y++)
            {
                for (int x = 0; x < numTilesWidth; x++)
                {
                    int index = _map[y * numTilesWidth + x, 0];
                    int rotation = _map[y * numTilesWidth + x, 1];
                    if (index == -1)
                    {
                        continue;
                    }
                    //spriteBatch.Draw(_tilesetTexture, new Vector2(x * _tileWidth, y * _tileHeight), _tiles[index], Color.White);
                    spriteBatch.Draw(_tilesetTexture, new Vector2(x * _tileWidth + _tileWidth / 2, y * _tileHeight + _tileHeight / 2), _tiles[index], Color.White, MathHelper.PiOver2*rotation, new Vector2(_tileWidth/2, _tileHeight/2), 1, SpriteEffects.None, 0);
                }
            }
        }

    }
}
