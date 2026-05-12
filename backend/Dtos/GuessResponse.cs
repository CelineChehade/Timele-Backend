namespace Timele.Dtos
{
    public class GuessResponse
    {
        public string Result { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public int CorrectYear { get; set; }

        public int PointsEarned { get; set; }
    }
}