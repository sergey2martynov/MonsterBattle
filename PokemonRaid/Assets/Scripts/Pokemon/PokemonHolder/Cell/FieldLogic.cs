using System.Collections.Generic;

namespace Pokemon.PokemonHolder.Cell
{
    public class FieldLogic
    {
        private FieldView _fieldView;
        private PokemonHolderModel _pokemonHolderModel;
        private List<List<CellData>> _cells = new List<List<CellData>>();

        private const int NumberOfRow = 4;
        private const int NumberOfColumn = 5;


        public FieldLogic(FieldView fieldView, PokemonHolderModel pokemonHolderModel)
        {
            _fieldView = fieldView;
            _pokemonHolderModel = pokemonHolderModel;
        }

        public void Initialize()
        {
            _fieldView.FieldCreated += CreateCellDates;
        }

        private void CreateCellDates(List<CellView> cells)
        {
            int count = 0;

            for (int i = 0; i < NumberOfRow; i++)
            {
                _cells.Add(new List<CellData>());
            
                for (int j = 0; j < NumberOfColumn; j++)
                {
                    _cells[i].Add(new CellData(cells[count].transform.position, i, j));
                
                    count++;
                }
            }

            _pokemonHolderModel.SetCells(_cells);
        }
    }
}