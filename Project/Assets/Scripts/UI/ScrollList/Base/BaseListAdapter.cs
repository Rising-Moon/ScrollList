using System.Collections;
using ScrollList.Interface;
using UnityEngine;
using Object = System.Object;

namespace ScrollList.Base
{
    public abstract class BaseListAdapter : IListAdapter
    {
        //Item适配器
        private IItemAdapter itemAdapter;

        //获取item数量
        public abstract int GetCount();

        //获取index位置的Item
        public abstract object GetItem(int index);
        public abstract int GetItemId(int index);

        public void InitItem(GameObject view, Object data)
        {
            itemAdapter.Init(view, data);
        }

        public void UpdateItem(GameObject view, Object data)
        {
            itemAdapter.Update(view, data);
        }

        //设置Item适配器
        public void SetItemAdapter(IItemAdapter adapter)
        {
            itemAdapter = adapter;
        }
    }
}