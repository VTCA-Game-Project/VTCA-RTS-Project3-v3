
namespace Manager
{
    public class MainPlayer : Player
    {
        protected override void Start()
        {
            Group = EnumCollection.Group.Player;
            base.Start();
        }

    }
}