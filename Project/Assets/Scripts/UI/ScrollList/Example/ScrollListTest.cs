using System.Collections.Generic;
using ScrollList.Adapter;
using ScrollList.Data;
using ScrollList.Interface;
using UnityEngine;

public class ScrollListTest : MonoBehaviour
{
    [SerializeField] public ScrollList.UI.ScrollList ScrollList;

    private List<TextData> data;

    // Start is called before the first frame update
    void Start()
    {
        data = new List<TextData>();
        for (int i = 0; i < 50; i++)
        {
            data.Add(new TextData(i.ToString()));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            IListAdapter adapter = new ListAdapter(data,new SingleTextItemAdapter());
            ScrollList.SetAdapter(adapter);
        }
        
        if (Input.GetKeyDown("a"))
        {
            data.Add(new TextData("new Item"));
        }
        
        if (Input.GetKeyDown("m"))
        {
            ScrollList.SmoothScrollByOffset(4);
        }
    }
}