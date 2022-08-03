using System.Collections.Generic;
using Pokemon.PokemonHolder.Cell;
using Pool;
using Shop;

namespace Pokemon.PokemonHolder.Field
{
    public class FieldLogic
    {
        private readonly FieldView _fieldView;
        private readonly PokemonSpawner _pokemonSpawner;
        private readonly PokemonHolderModel _pokemonHolderModel;
        private readonly ShopLogic _shopLogic;
        private readonly List<List<CellData>> _cells = new List<List<CellData>>();

        private const int NumberOfRow = 4;
        private const int NumberOfColumn = 5;

        private bool _isFieldFillRequired;


        public FieldLogic(FieldView fieldView, PokemonHolderModel pokemonHolderModel, ShopLogic shopLogic, PokemonSpawner pokemonSpawner)
        {
            _fieldView = fieldView;
            _pokemonHolderModel = pokemonHolderModel;
            _shopLogic = shopLogic;
            _pokemonSpawner = pokemonSpawner;
        }

        public void Initialize(bool isFieldFillRequired)
        {
            _isFieldFillRequired = isFieldFillRequired;
            _fieldView.FieldCreated += CreateCellDates;
            _shopLogic.StartButtonPressed += DisableView;
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

            if (_isFieldFillRequired)
            {
                FillFieldsWithPokemons();
            }
        }

        private void FillFieldsWithPokemons()
        {
            var index = 0;
            
            foreach (var pokemonList in _pokemonHolderModel.PokemonsList)
            {
                foreach (var pokemon in pokemonList)
                {
                    if (pokemon == null)
                    {
                        _pokemonHolderModel.SetValueCellData(index, true);
                        index++;
                        continue;
                    }
                    
                    var indexes = pokemon.Indexes;
                    var position = _cells[indexes[0]][indexes[1]].Position;
                    _pokemonSpawner.CreatePokemonFromData(pokemon, position);
                    _pokemonHolderModel.SetValueCellData(index, false);
                    index++;
                }
            }
        }
        
        private void DisableView()
        {
            _fieldView.gameObject.SetActive(false);
        }
    }
}