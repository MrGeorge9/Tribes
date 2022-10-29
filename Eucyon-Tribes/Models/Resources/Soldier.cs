namespace Eucyon_Tribes.Models.Resources
{
    public class Soldier : Resource
    {
        private int _level;
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                if (value > 0)
                {
                    _level = value;
                }
            }
        }
        public int TotalHP { get; set; }
        public int CurrentHP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public Army Army { get; set; }
        public int? ArmyId { get; set; }

        public Soldier()
        {
        }
    }
}