using ScrollList.Interface;
using UnityEngine;
using Object = System.Object;

namespace ScrollList.Base
{
    public abstract class BaseItemAdapter : IItemAdapter
    {
        protected GameObject Item;
        protected Object Data;

        //初始化Item
        public abstract void Init(GameObject item, Object data);

        //更新Item
        public abstract void Update(GameObject item, Object data);
    }
}