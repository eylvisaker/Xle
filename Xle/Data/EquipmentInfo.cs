namespace Xle.Data
{
    public class EquipmentInfo
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public int[] Prices { get; private set; }
        public bool Ranged { get; set; }

        public EquipmentInfo(int id, string name, string prices)
        {
            ID = id;
            Name = name;

            Prices = new int[5];

            if (string.IsNullOrEmpty(prices) == false)
            {
                string[] vals = prices.Split(',');

                for (int i = 0; i < vals.Length; i++)
                {
                    Prices[i] = int.Parse(vals[i]);
                }
            }
        }
    }
}