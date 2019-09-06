using ScrollList.Data;
using ScrollList.Interface;
using UnityEngine;
using UnityEngine.UI;

/*
 * 示例类，可删除
 */

namespace ScrollList.Adapter
{
    public class SingleTextItemAdapter : IItemAdapter
    {
        public void Init(GameObject item, object data)
        {
            item.transform.Find("Content").GetComponent<Text>().text = (data as TextData).Text;
            item.GetComponent<Button>().onClick.RemoveAllListeners();
            item.GetComponent<Button>().onClick.AddListener(() => { Debug.Log((data as TextData).Text); });
        }

        public void Update(GameObject item, object data)
        {
            item.transform.Find("Content").GetComponent<Text>().text = (data as TextData).Text;
        }
    }
}