using ScrollList.Interface;
using UnityEngine;
using Object = System.Object;

namespace ScrollList.Base{
    public abstract class BaseItemAdapter : IItemAdapter{
        protected GameObject item;
        protected Object data;

        //初始化Item
        public abstract void Init(GameObject item, Object data);

        //更新Item
        public abstract void Update(GameObject item, Object data);
    }
}