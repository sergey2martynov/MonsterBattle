using System.Collections.Generic;
using Pokemon.PokemonHolder.Cell;

namespace Pokemon.PokemonHolder
{
    public class PokemonHolderModel
    {
        private List<List<CellData>> _cells;
        private List<PokemonDataBase> _pokemons;
        private Queue<CellData> _emptyCells;
    }
}