namespace EverybodyCodes.Gateways
{
    public class SubmitAnswerResponse
    {
        public bool Correct { get; set; }
        public bool FirstCorrect { get; set; }
        public int GlobalPlace { get; set; }
        public int GlobalScore { get; set; }
        public long GlobalTime { get; set; }
        public bool LengthCorrect { get; set; }
        public long LocalTime { get; set; }
        public long Time { get; set; }
    }
}