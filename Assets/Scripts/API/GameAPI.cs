using System.Collections.Generic;

namespace TestTask.API
{
    public class GameAPI
    {
        private readonly Dictionary<int, int> _backpackItems;
        private readonly ServerAPI _serverApi;

        private const string PICK_ACTION = "pick";
        private const string DROP_ACTION = "drop";

        public EventBackpackChanged ItemPicked = new EventBackpackChanged();
        public EventBackpackChanged ItemRemoved = new EventBackpackChanged();

        public GameAPI(Dictionary<int, int> data, ServerAPI serverApi)
        {
            _backpackItems = data==null? new Dictionary<int, int>() : data;
            _serverApi = serverApi;
        }

        public Dictionary<int, int> BackpackItems
        {
            get => _backpackItems;
        }

        public int GetItemCount(int id)
        {
            if (!_backpackItems.ContainsKey(id))
                return 0;

            return _backpackItems[id];
        }

        public void PickItem(int id)
        {
            if (!_backpackItems.ContainsKey(id))
            {
                _backpackItems[id] = 1;
            }
            else
            {
                _backpackItems[id]++;
            }

            _serverApi.SendAction(id,PICK_ACTION);
            ItemPicked?.Invoke(id);
        }

        public void DropItem(int id)
        {
            if(!_backpackItems.ContainsKey(id))
                return;

            _backpackItems[id]--;

            if (_backpackItems[id] <= 0)
                _backpackItems.Remove(id);

            _serverApi.SendAction(id,DROP_ACTION);
            ItemRemoved?.Invoke(id);
        }
    }
}


