using System.Collections;
using System.Collections.Generic;
using ScrollList.Base;
using ScrollList.Interface;
using UnityEngine;
using Object = System.Object;

namespace ScrollList.Adapter{
    public class SimpleListAdapter : BaseListAdapter{
        private IList list;

        public SimpleListAdapter(IList list, IItemAdapter adapter){
            this.list = list;
            base.setItemAdapter(adapter);
        }

        public override int GetCount(){
            return list.Count;
        }

        public override object GetItem(int index){
            if (index > list.Count)
                return null;
            return list[index];
        }

        public override int GetItemId(int index){
            return index;
        }
    }
}