using UnityEngine;
using Object = System.Object;

namespace ScrollList.Interface{
    public interface IItemAdapter{
        void Init(GameObject item, Object data);
        void Update(GameObject item, Object data);
    }
}