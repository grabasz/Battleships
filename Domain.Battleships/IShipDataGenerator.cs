namespace Domain.Battleships
{
    public interface IShipDataGenerator
    {
        int GetStartShipPoint(int shipSize);
        int GetRand0To9();
        bool GetIsVertical();
    }
}