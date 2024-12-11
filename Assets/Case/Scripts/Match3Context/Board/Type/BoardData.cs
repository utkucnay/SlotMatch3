namespace Case.Match3.Board
{
    [System.Serializable]
    public class BoardData
    {
        public int Row = 5;
        public int Column = 5;

        public float SpinSpeed = 20f;
        public float SlowAndStopDuration = 4f;
        public float MinSpeed = 8f;

        public int MinCount = 2;

        public float SwipeCandyDuration = 1f;
    }
}