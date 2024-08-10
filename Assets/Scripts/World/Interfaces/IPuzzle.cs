namespace World.Interfaces {
    public interface IPuzzle {
        bool IsSolved();

        void ResolvePuzzle();

        void CheckPuzzle();

        void WrongSolution();
    }
}