using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

//水平&垂直
public enum Direction{
    horizontal,
    vertical
}

[ExecuteAlways]
public class ScrollList : MonoBehaviour{
    //滚动条
    public Scrollbar verticalScrollBar;
    public Scrollbar horizontalScrollBar;

    //显示区域
    public GameObject viewPort;

    //列表区域
    public GameObject content;

    //Item
    public GameObject itemPre;

    //方向
    public Direction direction;

    //每页Item数量
    public int perCount;

    //预加载数量
    public int preLoad;

    //列表适配器
    private IListAdapter mAdapter;

    //滚动条
    private Scrollbar scrollbar;

    //缓存布局
    private Direction preDirection;

    //item大小
    private Rect item;

    //item数量
    private int count;

    public int itemCount {
        get { return count; }
    }

    private void Start(){
        preDirection = direction;
        //初始化
        ApplyDirection();
    }

    private void Update(){
#if UNITY_EDITOR
        count = content.transform.childCount;
#endif
        if (direction != preDirection) {
            ApplyDirection();
            preDirection = direction;
        }
    }

    public void setAdapter(IListAdapter listAdapter){
        mAdapter = listAdapter;
        mAdapter.init();
    }


    //应用当前设置的方向
    private void ApplyDirection(){
        print(count);
        RectTransform contentRect = content.transform as RectTransform;
        RectTransform itemRect = itemPre.transform as RectTransform;
        if (direction == Direction.vertical) {
            //hideBar(horizontalScrollBar.gameObject);
            //showBar(verticalScrollBar.gameObject);
            scrollbar = verticalScrollBar;
            contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemRect.rect.width);
            contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, itemRect.rect.height * count);
        }
        else {
            //hideBar(verticalScrollBar.gameObject);
            //showBar(horizontalScrollBar.gameObject);
            scrollbar = horizontalScrollBar;
            contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemRect.rect.width * count);
            contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, itemRect.rect.height);
        }
    }

    private void hideBar(GameObject obj){
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    private void showBar(GameObject obj){
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }
}