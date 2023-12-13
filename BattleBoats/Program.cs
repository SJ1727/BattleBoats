namespace BattleBoats
{
    internal class Program
    {
        const int MAX_BOARD_WIDTH = 26;
        const int MAX_BOARD_HEIGHT = 26;

        static void Main(string[] args)
        {
            Board testBoard = new Board(10, 10);
            List<(int x, int y)> pos = new List<(int x, int y)>();
            pos.Add((1, 1));
            pos.Add((2, 1));
            pos.Add((3, 1));
            testBoard.AddBoat(new Boat(pos, 'B', '#', '.'));
            //testBoard.FireAt(1, 1);
            //testBoard.FireAt(3, 2);
            testBoard.DisplayBoard(true);
        }
    }
}