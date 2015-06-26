namespace ERY.Xle.LotA.MapExtenders.Castle.Events
{
    public class SpiralSuccess : Spiral
    {
        public override bool StepOn()
        {
            if (AnyBad)
                return false;

            SoundMan.PlaySoundSync(LotaSound.VeryGood);

            ClearSpiral();
            RemoveSpiralEvents();

            for (int y = 3; y < TheEvent.Rectangle.Y - 1; y++)
            {
                for (int x = TheEvent.Rectangle.X - 3; x <= TheEvent.Rectangle.X; x++)
                {
                    Map[x, y] = 0;
                }
            }

            return true;
        }
    }
}