
namespace Darkhood.TexasHoldem.Net
{
    public interface IGameEventProcessor
    {
        void ProcessEvent(GameEvent gameEvent);
    }
}
