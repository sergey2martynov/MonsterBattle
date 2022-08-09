using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using CardsCollection;
using Player;
using Pokemon.PokemonHolder;
using UnityEngine;

namespace SaveLoad
{
    public class SaveLoadSystem
    {
        private readonly PlayerData _playerData;
        private readonly PokemonHolderModel _pokemonHolderModel;
        private readonly PokemonAvailabilityData _pokemonAvailabilityData;
        private FileStream _fileStream;

        public SaveLoadSystem(PlayerData playerData, PokemonHolderModel pokemonHolderModel,
            PokemonAvailabilityData pokemonAvailabilityData)
        {
            _playerData = playerData;
            _pokemonHolderModel = pokemonHolderModel;
            _pokemonAvailabilityData = pokemonAvailabilityData;
        }

        public void SaveData()
        {
            var binaryFormatter = new BinaryFormatter();
            var path = Path.Combine(Application.persistentDataPath, "data.oi");
            var fileStream = new FileStream(path, FileMode.Create);
            var data = new Data(_pokemonHolderModel.PokemonsList, _playerData.Level, _playerData.Coins,
                _pokemonAvailabilityData.MeleePokemonAvailabilities,
                _pokemonAvailabilityData.RangePokemonAvailabilities, _playerData.LevelCount,
                _playerData.MeleeBuyCounter, _playerData.RangedBuyCounter, _playerData.Gems);

            using (fileStream)
            {
                binaryFormatter.Serialize(fileStream, data);
            }
        }

        public bool TryLoadData(out Data data)
        {
            var path = Path.Combine(Application.persistentDataPath, "data.oi");
            var binaryFormatter = new BinaryFormatter();
            //var fileStream = new FileStream(path, FileMode.Open);

            try
            {
                _fileStream = new FileStream(path, FileMode.Open);
                data = binaryFormatter.Deserialize(_fileStream) as Data;
                return true;
            }
            catch (FileNotFoundException)
            {
                data = default;
                return false;
            }
            catch (SerializationException)
            {
                data = default;
                return false;
            }
            finally
            {
                _fileStream?.Dispose();
            }
        }
    }
}