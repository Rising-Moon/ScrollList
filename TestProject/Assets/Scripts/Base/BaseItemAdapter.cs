using UnityEngine;

public abstract class BaseItemAdapter<Data> : IItemAdapter{
    protected GameObject item;
    protected Data data;

    //初始化Item
    public abstract void init();

    //更新Item
    public abstract void update();

    //销毁适配器
    public void drop(){
        item = null;
        data = default(Data);
    }
}