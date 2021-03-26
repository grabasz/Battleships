namespace Domain.Battleships.MapGeneraton
{
    public class RandomShipLocation
    {
        public bool IsVertical { get; set; }
        public int ConstantRowOrColumn { get; set; }
        public int StartShepPoint { get; set; }
        public int ShipSize { get; set; }
    }
}