using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorizontalListAdapter<Data,ItemAdapter> : BaseListAdapter{
    private ScrollList context;
    private List<Data> list;
    
    public HorizontalListAdapter(ScrollList context,List<Data> list){
        this.context = context;
        this.list = list;
    }

    public override int getCount(){
        return list.Count;
    }

    public override object getItem(int index){
        return list[index];
    }

    public override void removeItem(int index){
        list.Remove(list[index]);
    }

    public override void removeItem(object obj){
        list.Remove((Data) obj);
    }


    public override void init(){
        
    }
}