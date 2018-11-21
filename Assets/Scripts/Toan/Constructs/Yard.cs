using EnumCollection;

namespace Common.Building
{
    public class Yard : Construct
    {
        protected override void Awake()
        {
            Id = ConstructId.Yard;    
            base.Awake();
        }
        protected override void Start()
        {
            base.Start();
        }
    }
}
