using System.Collections.Generic;
using DG.Tweening;
using InputPlayer;
using Pokemon;
using Pokemon.PokemonHolder;
using Pokemon.PokemonHolder.Cell;
using Pool;
using UnityEngine;

namespace Merge
{
    public class PokemonCellPlacer
    {
        private Ray _ray;
        private PokemonViewBase _targetPokemon;
        private InputView _inputView;
        private FieldView _fieldView;
        private List<CellView> _cellViews;
        private PokemonHolderModel _pokemonHolderModel;
        private PokemonSpawner _pokemonSpawner;
        private CellView _fixedCell;
        private int _fixedIndex;
        private PokemonMerger _pokemonMerger;
        private PokemonViewBase _pokemonForSwap;
        private readonly float _moveDuration = 0.2f;
        private readonly float _distanceForMerge = 0.8f;

        public PokemonCellPlacer(InputView inputView, FieldView fieldView, PokemonHolderModel pokemonHolderModel,
            PokemonMerger pokemonMerger, PokemonSpawner pokemonSpawner)
        {
            _fieldView = fieldView;
            _pokemonHolderModel = pokemonHolderModel;
            _pokemonMerger = pokemonMerger;
            _pokemonSpawner = pokemonSpawner;
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
                    _fieldView.PokemonViews.Remove(_targetPokemon);

                    Object.Destroy(_targetPokemon.gameObject);
                    _pokemonSpawner.CreateFirstLevelPokemon(currentCell.transform.position, _targetPokemon,
                        _pokemonForSwap.GetPokemonLevel() + 1);
                    _fieldView.PokemonViews.Remove(_pokemonForSwap);
                    Object.Destroy(_pokemonForSwap.gameObject);
                }
                else if (IsSwap())
                {
                    _targetPokemon.transform.DOMoveX(_pokemonForSwap.transform.position.x, _moveDuration);
                    _targetPokemon.transform.DOMoveZ(_pokemonForSwap.transform.position.z, _moveDuration);

                    _pokemonForSwap.transform.DOMoveX(_fixedCell.gameObject.transform.position.x, _moveDuration);
                    _pokemonForSwap.transform.DOMoveZ(_fixedCell.gameObject.transform.position.z, _moveDuration);

                    _pokemonHolderModel.SetValueCellData(_fixedIndex, false);
                }
                else
                {
                    var nearestCell = GetNearestEmptyCell(_targetPokemon.transform.position);

                    _targetPokemon.transform.DOMoveX(nearestCell.transform.position.x, _moveDuration);
                    _targetPokemon.transform.DOMoveZ(nearestCell.transform.position.z, _moveDuration);
                }

                _targetPokemon = null;
            }
        }

        private CellView GetNearestEmptyCell(Vector3 pokemonPosition)
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

                if (cellData.EmptyState && tempDistance < distance)
                {
                    index = i;
                    distance = tempDistance;
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