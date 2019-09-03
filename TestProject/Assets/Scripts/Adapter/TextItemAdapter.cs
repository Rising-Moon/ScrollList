using UnityEngine;
using UnityEngine.UI;

public class TextItemAdapter : BaseItemAdapter<TextData>{
    private Text text;
    
    public TextItemAdapter(GameObject item, TextData data){
        base.item = item;
        base.data = data;
    }

    public override void init(){
        text = item.GetComponent<Text>();
        text.text = string.Empty;
    }

    public override void update(){
        text.text = data.text;
    }
}