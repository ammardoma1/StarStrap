using StarStrap.Models;

namespace StarStrap.Enums
{
    public enum AIAgentProvider
    {
        [EnumName(StaticName = "OpenAI")]
        OpenAI,
        [EnumName(StaticName = "Gemini")]
        Gemini,
        [EnumName(StaticName = "Anthropic")]
        Anthropic,
        [EnumName(StaticName = "Groq")]
        Groq
    }
}
