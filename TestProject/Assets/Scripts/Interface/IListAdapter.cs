using UnityEngine;
using Object = System.Object;

public interface IListAdapter{
    int getCount();
    Object getItem(int index);
    void removeItem(int index);
    void removeItem(Object obj);
    
    void init();
}