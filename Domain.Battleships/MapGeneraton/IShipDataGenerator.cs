namespace Domain.Battleships.MapGeneraton
{
    public interface IShipDataGenerator
    {
        int GetStartShipPoint(int shipSize);
        int GetRand0To9();
        bool GetIsVertical();
        int GetDirection();
    }
}