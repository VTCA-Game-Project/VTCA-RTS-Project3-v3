
using EnumCollection;
using Manager;

namespace DelegateCollection
{
    public delegate void Create(System.Enum solidier);
    public delegate void GameAction(object data);
    public delegate void PlayerAction(Player data);
    public delegate NodeState ActionNodeDelegate();
}
