using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using InputPlayer;
using LevelEnvironment;
using Player;
using Pokemon;
using Pokemon.PokemonHolder;
using Pokemon.PokemonHolder.Cell;
using Pokemon.PokemonHolder.Field;
using Pool;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Merge
{
    public class PokemonCellPlacer
    {
        
        private readonly InputView _inputView;
        private readonly FieldView _fieldView;
        private readonly PokemonHolderModel _pokemonHolderModel;
        private readonly PokemonSpawner _pokemonSpawner;
        private readonly PokemonMerger _pokemonMerger;
        private readonly PlayerData _playerData;
        private readonly List<Vector3> _matchedPositions = new List<Vector3>(20);
        
        private List<CellView> _cellViews;
        private Ray _ray;
        private PokemonViewBase _targetPokemon;
        private CellView _fixedCell;
        private int _fixedIndex;
        private PokemonViewBase _pokemonForSwap;
        private readonly float _moveDuration = 0.2f;
        private readonly float _distanceForMerge = 0.8f;

        public event Action<bool> PokemonMerged;
        public event Action<List<Vector3>> MatchFound;
        public event Action PokemonReleased;

        public PokemonCellPlacer(InputView inputView, FieldView fieldView, PokemonHolderModel pokemonHolderModel,
            PokemonMerger pokemonMerger, PokemonSpawner pokemonSpawner, PlayerData playerData)
        {
            _fieldView = fieldView;
            _pokemonHolderModel = pokemonHolderModel;
            _pokemonMerger = pokemonMerger;
            _pokemonSpawner = pokemonSpawner;
            _playerData = playerData;
            _inputView = inputView;
        }

        public void Initialize()
        {
            _inputView.ButtonMouseHold += OnButtonMouseHold;
            _inputView.ButtonMousePressed += OnButtonMousePressed;
            _inputView.ButtonMouseReleased += OnButtonMouseReleased;
            _cellViews = _fieldView.GetCellViews();
        }

        private void OnButtonMousePressed()
        {
            _ray = _inputView.Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(_ray, out hit);

            if (hit.collider.gameObject.TryGetComponent(out PokemonViewBase pokemon))
            {
                _targetPokemon = pokemon;
                _fixedCell = GetCurrentCell(_targetPokemon.transform.position,true);

                foreach (var pokemonView in _fieldView.PokemonViews.Where(pokemonView =>
                             _targetPokemon.GetType() == pokemonView.GetType() &&
                             _targetPokemon.GetPokemonLevel() == pokemonView.GetPokemonLevel() &&
                             _targetPokemon != pokemonView))
                {
                    _matchedPositions.Add(pokemonView.Transform.position);
                }

                if (_matchedPositions.Count == 0)
                {
                    return;
                }
                
                MatchFound?.Invoke(_matchedPositions);
            }
        }

        private void OnButtonMouseHold()
        {
            if (_targetPokemon != null)
            {
                _ray = _inputView.Camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(_ray, 400f);


                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider.TryGetComponent(out PlaneView plane))
                    {
                        _targetPokemon.transform.position = new Vector3(
                            Mathf.Clamp(hits[i].point.x, _inputView.LeftBorderForMerge, _inputView.RightBorderForMerge),
                            _targetPokemon.gameObject.transform.position.y,
                            Mathf.Clamp(hits[i].point.z, _inputView.DownBorderForMerge, _inputView.UpBorderForMerge));
                    }
                }
            }
        }

        private void OnButtonMouseReleased()
        {
            if (_targetPokemon != null)
            {
                if (IsMerge())
                {
                    var currentCell = GetCurrentCell(_pokemonForSwap.transform.position, false);
                    
                    _pokemonHolderModel.DeletePokemonFromList(_pokemonForSwap.GetIndexes());
                    _pokemonHolderModel.DeletePokemonFromList(_targetPokemon.GetIndexes());
                    
                    _fieldView.PokemonViews.Remove(_targetPokemon);
                    _fieldView.PokemonViews.Remove(_pokemonForSwap);
                    var indexes = _pokemonForSwap.GetIndexes().ToArray();
                    _pokemonSpawner.CreatePokemon(currentCell.transform.position, _targetPokemon,
                        _pokemonForSwap.GetPokemonLevel() + 1, indexes/*_pokemonForSwap.GetIndexes()*/);
                    
                    PokemonMerged?.Invoke(true);

                    Object.Destroy(_pokemonForSwap.gameObject);
                    Object.Destroy(_targetPokemon.gameObject);
                }
                else if (IsSwap())
                {
                    var tempIndexes = _targetPokemon.GetIndexes().ToArray();
                    var tempIndexes2 = _pokemonForSwap.GetIndexes().ToArray();
                    
                    _pokemonHolderModel.SwapPokemons(tempIndexes,tempIndexes2);

                    if (tempIndexes == tempIndexes2)
                        throw new Exception("Indexes are equal. Check Pokemon Holder Model");
                        
                    _targetPokemon.SetIndexes(tempIndexes2);
                    _pokemonForSwap.SetIndexes(tempIndexes);

                    _targetPokemon.transform.DOMoveX(_pokemonForSwap.transform.position.x, _moveDuration);
                    _targetPokemon.transform.DOMoveZ(_pokemonForSwap.transform.position.z, _moveDuration);

                    _pokemonForSwap.transform.DOMoveX(_fixedCell.gameObject.transform.position.x, _moveDuration);
                    _pokemonForSwap.transform.DOMoveZ(_fixedCell.gameObject.transform.position.z, _moveDuration);
                    
                    _pokemonHolderModel.SetValueCellData(_fixedIndex, false);
                }
                else if (_playerData.Level != 2)
                {
                    var nearestCell = GetNearestEmptyCell(_targetPokemon.transform.position, out var cellData);
                    _pokemonHolderModel.SwapPokemons(_targetPokemon.GetIndexes(),
                        new[] { cellData.Row, cellData.Column });
                    _targetPokemon.SetIndexes(new[] { cellData.Row, cellData.Column });

                    _targetPokemon.transform.DOMoveX(nearestCell.transform.position.x, _moveDuration);
                    _targetPokemon.transform.DOMoveZ(nearestCell.transform.position.z, _moveDuration);
                }
                else if (_playerData.Level == 2 && _playerData.Coins > 7)
                {
                    _targetPokemon.transform.DOMoveX(_fixedCell.gameObject.transform.position.x, _moveDuration);
                    _targetPokemon.transform.DOMoveZ(_fixedCell.gameObject.transform.position.z, _moveDuration);
                }

                _targetPokemon = null;
            }
            
            PokemonReleased?.Invoke();
            _matchedPositions.Clear();
        }

        private CellView GetNearestEmptyCell(Vector3 pokemonPosition, out CellData outCellData)
        {
            float distance = 200f;
            float tempDistance;
            int index = 0;
            CellData cellData;
            outCellData = default;

            for (int i = 0; i < _cellViews.Count; i++)
            {
                cellData = _pokemonHolderModel.GetCellData(i);

                tempDistance = Vector3.Distance(pokemonPosition,
                    cellData.Position);

                if (cellData.EmptyState && tempDistance < distance)
                {
                    index = i;
                    distance = tempDistance;
                    outCellData = cellData;
                }
            }

            _pokemonHolderModel.SetValueCellData(index, false);
            return _cellViews[index];
        }

        private CellView GetCurrentCell(Vector3 pokemonPosition, bool isEmpty)
        {
            float distance = 200f;
            float tempDistance;
            int index = 0;
            CellData cellData;

            for (int i = 0; i < _cellViews.Count; i++)
            {
                cellData = _pokemonHolderModel.GetCellData(i);

                tempDistance = Vector3.Distance(pokemonPosition,
                    cellData.Position);

                if (tempDistance < distance)
                {
                    index = i;
                    distance = tempDistance;
                }
            }

            _fixedIndex = index;
            _pokemonHolderModel.SetValueCellData(index, isEmpty);
            return _cellViews[index];
        }

        private bool IsMerge()
        {
            float tempDistance;

            for (int i = 0; i < _fieldView.PokemonViews.Count; i++)
            {
                tempDistance = Vector3.Distance(_targetPokemon.transform.position,
                    _fieldView.PokemonViews[i].transform.position);

                if (_targetPokemon == _fieldView.PokemonViews[i])
                {
                    continue;
                }

                if (tempDistance < _distanceForMerge)
                {
                    _pokemonForSwap = _fieldView.PokemonViews[i];

                    if (_pokemonMerger.TryMerge(_targetPokemon, _pokemonForSwap))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsSwap()
        {
            float tempDistance;

            for (int i = 0; i < _fieldView.PokemonViews.Count; i++)
            {
                tempDistance = Vector3.Distance(_targetPokemon.transform.position,
                    _fieldView.PokemonViews[i].transform.position);

                if (_targetPokemon == _fieldView.PokemonViews[i])
                {
                    continue;
                }

                if (tempDistance < _distanceForMerge)
                {
                    return true;
                }
            }

            return false;
        }
    }
}