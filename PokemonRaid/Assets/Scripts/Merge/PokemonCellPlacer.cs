using System.Collections.Generic;
using DG.Tweening;
using InputPlayer;
using Pokemon;
using Pokemon.PokemonHolder;
using Pokemon.PokemonHolder.Cell;
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

        public PokemonCellPlacer(InputView inputView, FieldView fieldView, PokemonHolderModel pokemonHolderModel)
        {
            _fieldView = fieldView;
            _pokemonHolderModel = pokemonHolderModel;
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
                float distance = Vector3.Distance(_targetPokemon.transform.position, _cellViews[0].transform.position);
                float tempDistance;
                int index = 0;

                for (int i = 1; i < _cellViews.Count; i++)
                {
                    tempDistance = Vector3.Distance(_targetPokemon.transform.position,
                        _cellViews[i].transform.position);
                    if (tempDistance < distance)
                    {
                        index = i;
                        distance = tempDistance;
                    }
                }

                _targetPokemon.transform.DOMoveX(_cellViews[index].transform.position.x, 0.2f);
                _targetPokemon.transform.DOMoveZ(_cellViews[index].transform.position.z, 0.2f);
                _targetPokemon = null;
            }
        }
        
        


        private void Swap()
        {
            
        }
    }
}