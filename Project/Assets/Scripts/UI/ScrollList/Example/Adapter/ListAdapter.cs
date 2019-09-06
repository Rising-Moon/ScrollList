using System.Collections;
using System.Collections.Generic;
using ScrollList.Base;
using ScrollList.Interface;
using UnityEngine;
using Object = System.Object;

/*
 * 示例类，可删除
 */

namespace ScrollList.Adapter
{
    public class ListAdapter : BaseListAdapter
    {
        private IList list;

        public ListAdapter(IList list, IItemAdapter adapter)
        {
            this.list = list;
            base.SetItemAdapter(adapter);
        }

        public override int GetCount()
        {
            return list.Count;
        }

        public override object GetItem(int index)
        {
            if (index > list.Count)
                return null;
            return list[index];
        }

        public override int GetItemId(int index)
        {
            return index;
        }
    }
}