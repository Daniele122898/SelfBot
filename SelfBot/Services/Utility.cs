using Discord;

namespace SelfBot.Services
{
    public static class Utility
    {
        
        public static Discord.Color GreenSuccessEmbed = new Discord.Color(119,178,85);
        public static Discord.Color RedFailiureEmbed = new Discord.Color(221,46,68);

        public static string[] SuccessLevelEmoji = new string[]
        {
            "✅","❌"
        };

        
        private static string _prefix;

        public static string Prefix
        {
            get { return _prefix; }
        }

        public static void SetPrefix(string prefix)
        {
            _prefix = prefix;
        }
        
        public static EmbedBuilder ResultFeedback(Discord.Color color, string symbol, string text)
        {
            var eb = new EmbedBuilder()
            {
                Color = color,
                Title = $"{symbol} {text}"
            };
            return eb;
        }

    }
}