using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseListAdapter : IListAdapter{
    protected List<IItemAdapter> ItemAdapters;

    //获取item数量
    public abstract int getCount();

    //获取index位置的Item
    public abstract object getItem(int index);

    //移除index位置的Item
    public abstract void removeItem(int index);

    //移除列表中的obj
    public abstract void removeItem(object obj);

    //初始化Adapter
    public abstract void init();

    public void AddUpdateCall(IItemAdapter adapter){
        ItemAdapters.Add(adapter);
    }

    public void NotifyAll(){
        foreach (var adapter in ItemAdapters) {
            adapter.update();
        }
    }
}