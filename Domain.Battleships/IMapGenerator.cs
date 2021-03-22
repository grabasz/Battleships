using System.Collections.Generic;

namespace Domain.Battleships
{
    public interface IMapGenerator
    {
        List<Ship> Generate(List<int> shipLengths);
    }
}