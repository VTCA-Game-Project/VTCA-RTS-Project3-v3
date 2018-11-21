using EnumCollection;

namespace Common.Building
{
    public class Radar : Construct
    {
        protected override void Awake()
        {
            Id = ConstructId.Radar;
            base.Awake();
        }
    }
}