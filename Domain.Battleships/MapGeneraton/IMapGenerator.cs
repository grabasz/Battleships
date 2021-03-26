using System.Collections.Generic;
using Domain.Battleships.Model;

namespace Domain.Battleships.MapGeneraton
{
    public interface IMapGenerator
    {
        List<Ship> Generate(List<int> shipLengths);
    }
}