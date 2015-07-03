namespace ERY.Xle.Data
{
    public class MagicSpell
    {
        public MagicSpell()
        {
        }


        public string Name { get; set; }
        public int BasePrice { get; set; }
        public int MaxCarry { get; set; }

        public int ItemID { get; set; }

        public string PluralName
        {
            get
            {
                if (Name.EndsWith("sh"))
                    return Name + "es";
                else
                    return Name + "s";
            }
        }

        public int ID { get; set; }
    }
}

