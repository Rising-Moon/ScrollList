using UnityEngine;
using Object = System.Object;

namespace ScrollList.Interface{
    public interface IListAdapter{
        int GetCount();
        Object GetItem(int index);
        int GetItemId(int index);

        void InitItem(GameObject view,Object data);
        void UpdateItem(GameObject view,Object data);
    }
}