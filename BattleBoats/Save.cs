using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBoats
{
    internal class Save
    {
        public static void SaveGame(Player player1, Player player2, int turn, string filePath)
        {
            using (BinaryWriter bw = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                bw.Write(turn);
                bw.Write(player1.ownBoard.width);
                bw.Write(player1.ownBoard.height);
                bw.Write(player1.name);
                bw.Write(player2.ownBoard.width);
                bw.Write(player2.ownBoard.height);
                bw.Write(player2.name);

                // Write how many boats player 1 has
                bw.Write(player1.ownBoard.boats.Count);

                foreach (Boat boat in player1.ownBoard.boats)
                {
                    bw.Write(boat.boatTypeIdentifier);
                    bw.Write(boat.getHeadPosition().x);
                    bw.Write(boat.getHeadPosition().y);
                    bw.Write(boat.orientation);
                }

                // Write the positions that have been hit for player 1 
                for (int i = 0; i < player1.ownBoard.width; i++)
                {
                    for (int j = 0; j < player1.ownBoard.height; j++)
                    {
                        bw.Write(player1.ownBoard.IsHitPosition(i, j));
                    }
                }

                // Write how many boats player 2 has
                bw.Write(player2.ownBoard.boats.Count);

                foreach (Boat boat in player2.ownBoard.boats)
                {
                    bw.Write(boat.boatTypeIdentifier);
                    bw.Write(boat.getHeadPosition().x);
                    bw.Write(boat.getHeadPosition().y);
                    bw.Write(boat.orientation);
                }

                // Write the positions that have been hit for player 2 
                for (int i = 0; i < player2.ownBoard.width; i++)
                {
                    for (int j = 0; j < player2.ownBoard.height; j++)
                    {
                        bw.Write(player2.ownBoard.IsHitPosition(i, j));
                    }
                }
            }
        }

        public static void LoadGame(ref Player player1, ref Player player2, ref int turn, string filePath)
        {
            Board newPlayer1Board = null;
            Board newPlayer2Board = null;
            Boat[] player1Boats = null;
            Boat[] player2Boats = null;
            int player1Width = 0;
            int player1Height = 0;
            int player2Width = 0;
            int player2Height = 0;
            string name = string.Empty;
            int numberOfBoats = 0;
            int boatType = 0;
            int boatHeadX = 0;
            int boatHeadY = 0;
            int boatOrientation = 0;


            using (BinaryReader br = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                turn = br.ReadInt32();

                player1Width = br.ReadInt32();
                player1Height = br.ReadInt32();
                name = br.ReadString();
                newPlayer1Board = new Board(player1Width, player1Height, name);

                player2Width = br.ReadInt32();
                player2Height = br.ReadInt32();
                name = br.ReadString();
                newPlayer2Board = new Board(player2Width, player2Height, name);

                // Get player 1 boats
                numberOfBoats = br.ReadInt32();
                player1Boats = new Boat[numberOfBoats];
                for (int i = 0; i < numberOfBoats; i++)
                {
                    boatType = br.ReadInt32();
                    boatHeadX = br.ReadInt32();
                    boatHeadY = br.ReadInt32();
                    boatOrientation = br.ReadInt32();

                    player1Boats[i] = Utils.GetBoatFromData(boatType, boatHeadX, boatHeadY, boatOrientation);
                }

                // Placing player 1 boats on the board
                newPlayer1Board.AddBoats(player1Boats);

                // Setting the positions that have been hit for player 1
                for (int i = 0; i < player1Width; i++)
                {
                    for (int j = 0; j < player1Height; j++)
                    {
                        if (br.ReadBoolean())
                        {
                            newPlayer1Board.FireAt(i, j);
                        }
                    }
                }

                // Get player 1 boats
                numberOfBoats = br.ReadInt32();
                player2Boats = new Boat[numberOfBoats];
                for (int i = 0; i < numberOfBoats; i++)
                {
                    boatType = br.ReadInt32();
                    boatHeadX = br.ReadInt32();
                    boatHeadY = br.ReadInt32();
                    boatOrientation = br.ReadInt32();

                    player2Boats[i] = Utils.GetBoatFromData(boatType, boatHeadX, boatHeadY, boatOrientation);
                }

                // Placing player 1 boats on the board
                newPlayer2Board.AddBoats(player2Boats);

                // Setting the positions that have been hit for player 1
                for (int i = 0; i < player2Width; i++)
                {
                    for (int j = 0; j < player2Height; j++)
                    {
                        if (br.ReadBoolean())
                        {
                            newPlayer2Board.FireAt(i, j);
                        }
                    }
                }
            }

            player1.setBoard(newPlayer1Board);
            player2.setBoard(newPlayer2Board);
        }
    }
}
