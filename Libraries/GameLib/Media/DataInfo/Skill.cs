namespace SilkroadInformationAPI.Media.DataInfo
{
    public class Skill : Base
    {
        public long Duration;
        public long Cooldown;
        public bool Buff;
        public string Description;
        public string Params;
        public string SkillIcon;
        public bool UseOnAlly;
        public bool UseOnSelf;
        public bool UseOnUnknown;
        public bool RequireTarget;
        public bool UseOnEnemy;
        public SkillType Type;
        public string Position;

        public Skill()
        {

        }
    }
}
