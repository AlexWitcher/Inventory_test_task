using System;
using System.Collections.Generic;
using TestTask.API;
using TestTask.Data;
using TestTask.Gameplay;
using UnityEngine;

namespace TestTask
{
    public class ApplicationContext
    {
        public SavesManager SavesManager { private set; get; }
        public ServerAPI ServerApi { private set; get; }
        public GameAPI GameApi { private set; get; }
        public ItemsDatabase ItemsDatabase { private set; get; }

        public static ApplicationContext Instance { get; private set; }

        private List<IGameManager> _managers = new List<IGameManager>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            if (Instance==null)
               Instance = new ApplicationContext();
        }

        private ApplicationContext()
        {
            ItemsDatabase = Resources.Load<ItemsDatabase>(Constants.ITEMS_DATABASE_PATH);
            if (ItemsDatabase == null)
            {
                Debug.LogError("Can't load items resources");
                return;
            }

            SavesManager = new SavesManager();
            ServerApi = new ServerAPI(Constants.AUTH_KEY,Constants.SERVER_URL);

            var savedData = SavesManager.LoadData();
            GameApi = new GameAPI(savedData, ServerApi);

            foreach (var gameManager in _managers)
            {
                InitializeManager(gameManager);
            }
        }

        public void RegisterManager(IGameManager manager)
        {
            if (GameApi != null)
            {
                InitializeManager(manager);
                return;
            }

            if (!_managers.Contains(manager))
             _managers.Add(manager);
        }

        private void InitializeManager(IGameManager manager)
        {
            var content = new GameContent {Api = GameApi, FocusChanged = SaveGame, ItemsData = ItemsDatabase.Items};
            manager.Initialize(content);
        }

        private void SaveGame()
        {
            SavesManager.SaveData(GameApi.BackpackItems);
        }
    }
}

