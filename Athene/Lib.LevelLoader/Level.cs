using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Input.Mapping;
using Lib.LevelLoader.LevelItems;
using Lib.Visuals.Graphics;
using OpenTK;
using OpenTK.Input;

namespace Lib.LevelLoader
{
    public class Level
    {
        /// <summary>
        /// List of all blocks drawn in the level, like walkable, water, lava, trees..
        /// </summary>
        public List<Block> Blocks { get; set; }

        /// <summary>
        /// List of all players in the level
        /// </summary>
        public List<LevelItemPlayer> Players { get; set; }

        /// <summary>
        /// Initializes a empty level
        /// </summary>
        public  Level()
		{
			Blocks = new List<Block>();
            Players = new List<LevelItemPlayer>();
            CreateTestPlayer();
		}

        /// <summary>
        /// updates the level
        /// </summary>
        public void UpdateLogic()
        {
            foreach (var player in Players)
            {
                player.UpdateLogic(this);
            }
            CheckCollision();
            
           
        }

        public void CheckCollision()
        {
            foreach (var player in Players)
            {
                var oldPositionOfPlayer = new Vector2(player.Position.X, player.Position.Y);
                BlockType playerEnvironment = BlockType.Solid;
                

                foreach (var block in Blocks)
                {
                    if (block.Position.X == 5 && block.Position.Y == -8)
                    {
                        
                    }
                    if (block.Collision && player.Physics.Box2D.Intersects(block.Box2D))
                    {
                        
                        player.Physics.ReactToCollision(block);
                    }

                    if (block.Position.X == Math.Round(player.Position.X) &&
                        block.Position.Y == Math.Round(player.Position.Y))
                        playerEnvironment = block.BlockType;

                }
                player.Physics.SetEnvironment(playerEnvironment);
            }
        }

        private void CreateTestPlayer()
        {
            LevelItemPlayer player = null;

            SpriteAnimated playerSprite = new SpriteAnimated();
            playerSprite.AddAnimation("Pics/Worm/walk", 1000);
            playerSprite.AddAnimation("Pics/Worm/idle", 1000);
            playerSprite.StartAnimation("walk");

            var mapList = new InputMapList<LevelItemPlayerActions>();

            mapList.AddMappingGamePad(0, pad => pad.ThumbSticks.Left, inp => inp.MoveLeft, (inval, curval) => inval.Length > 0.01 && inval.X > 0 ? -inval.X : 0);
            mapList.AddMappingGamePad(0, pad => pad.ThumbSticks.Left, inp => inp.MoveRight, (inval, curval) => inval.Length > 0.01 && inval.X < 0 ? inval.X : 0);
            mapList.AddMappingGamePad(0, pad => pad.ThumbSticks.Left, inp => inp.MoveUp, (inval, curval) => inval.Length > 0.01 && inval.Y > 0 ? inval.Y : 0);
            mapList.AddMappingGamePad(0, pad => pad.ThumbSticks.Left, inp => inp.MoveDown, (inval, curval) => inval.Length > 0.01 && inval.Y < 0 ? -inval.Y : 0);
            mapList.AddMappingGamePad(0, pad => pad.Buttons.A, inp => inp.Jump, (inval, curval) => inval == ButtonState.Pressed);

            mapList.AddMappingKeyboard(Key.Left, inp => inp.MoveLeft, (inval, curval) => inval ? +1 : 0);
            mapList.AddMappingKeyboard(Key.Right, inp => inp.MoveRight, (inval, curval) => inval ? +1 : 0);
            mapList.AddMappingKeyboard(Key.Up, inp => inp.MoveUp, (inval, curval) => inval ? +1 : 0);
            mapList.AddMappingKeyboard(Key.Down, inp => inp.MoveDown, (inval, curval) => inval ? +1 : 0);
            mapList.AddMappingKeyboard(Key.Space, inp => inp.Jump, (inval, curval) => inval);

            player = new LevelItemPlayer(Vector2.Zero, BlockType.Solid, mapList, playerSprite);
            Players.Add(player);

        }

        /// <summary>
        /// Draws the level
        /// </summary>
        public void Draw()
        {
            foreach (var block in Blocks)
            {
                block.Draw();
            }
            foreach (var player in Players)
            {
                player.Draw();
            }
        }

    }
}
