namespace EverybodyCodes
{
    public static class Globals
    {
        // The first Everybody Codes came out in 2024
        public const int START_YEAR = 2024;
        // Everybody Codes only starts in November
        public const int EVENT_MONTH = 11;
        // Everybody Codes has 20 days of puzzles
        public const int LAST_PUZZLE = 20;

        // TODO, verify this timezone
        // Everybody Codes's server is +1 UTC (CET)
        // We use -23 so that the day lines up with our logic
        public const int SERVER_UTC_OFFSET = -23;
    }
}
