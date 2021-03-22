namespace Domain.Battleships
{
    public interface IShipDataGenerator
    {
        int GetStartShipPoint(int shipSize);
        int GetConstantRowColumn();
        bool GetIsVertical();
    }
}