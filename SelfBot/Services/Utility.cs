namespace SelfBot.Services
{
    public static class Utility
    {
        private static string _prefix;

        public static string Prefix
        {
            get { return _prefix; }
        }

        public static void SetPrefix(string prefix)
        {
            _prefix = prefix;
        }
    }
}