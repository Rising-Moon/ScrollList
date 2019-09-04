using ScrollList.Data;
using ScrollList.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace ScrollList.Adapter{
    public class SimpleItemAdapter : IItemAdapter{
        public void Init(GameObject item, object data){
            item.transform.Find("Content").GetComponent<Text>().text = (data as TextData).text;
        }

        public void Update(GameObject item, object data){
            item.transform.Find("Content").GetComponent<Text>().text = (data as TextData).text;
        }
    }
}