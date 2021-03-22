namespace Domain.Battleships
{
    public class BotShipLocation
    {
        public bool IsVertical { get; set; }
        public int ConstantRowColumn { get; set; }
        public int StartShepPoint { get; set; }
        public int ShipSize { get; set; }
    }
}