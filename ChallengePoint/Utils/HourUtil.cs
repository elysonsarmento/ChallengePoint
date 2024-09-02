namespace ChallengePoint.Utils
{
    public static class HourUtil
    {
        public static DateTime ConvertIsoToDateTime(string isoString)
        {
            if (DateTime.TryParse(isoString, null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime dateTime))
            {
                return dateTime;
            }
            else
            {
                throw new FormatException("A string ISO fornecida não está em um formato válido.");
            }
        }
    }
}