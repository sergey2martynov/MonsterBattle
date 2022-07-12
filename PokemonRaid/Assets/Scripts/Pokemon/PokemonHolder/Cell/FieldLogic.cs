using System.Collections.Generic;
using Pokemon.PokemonHolder;
using Pokemon.PokemonHolder.Cell;

public class FieldLogic
{
    private FieldView _fieldView;
    private List<CellView> _cellsList;
    private PokemonHolderModel _pokemonHolderModel;
    private List<List<CellData>> _cells;

    private const int NumberOfRow = 5;
    private const int NumberOfColumn = 4;


    public FieldLogic(FieldView fieldView, List<CellView> cells, PokemonHolderModel pokemonHolderModel)
    {
        _fieldView = fieldView;
        _cellsList = cells;
        _pokemonHolderModel = pokemonHolderModel;
    }

    public void CreateCellDates()
    {
        int count = 0;

        for (int i = 0; i < NumberOfRow; i++)
        {
            for (int j = 0; j < NumberOfColumn; j++)
            {
                _cells[i][j] = new CellData(_cellsList[count].transform.position, j, i);
                    count++;
            }
        }
        
        _pokemonHolderModel.SetCells(_cells);
    }
}