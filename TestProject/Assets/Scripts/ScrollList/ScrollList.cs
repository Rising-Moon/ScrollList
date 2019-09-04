using System;
using System.Collections.Generic;
using System.Drawing;
using ScrollList.Adapter;
using ScrollList.Data;
using ScrollList.DataStruct;
using ScrollList.Interface;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


namespace ScrollList{
    //水平&垂直
    public enum ListType{
        Vertical,
        Horizontal,
    }

    public class ScrollList : MonoBehaviour{
        public ScrollRect scrollRect;

        //滚动条
        public Scrollbar verticalScrollBar;
        public Scrollbar horizontalScrollBar;

        //显示区域
        public GameObject viewPort;

        //列表区域
        public GameObject content;

        //Item
        public GameObject itemPre;

        //间隔布局
        public GameObject divider;

        //间隔高度
        public float dividerSize;

        //方向
        public ListType type;

        //每页Item数量
        public int perPageCount;

        //预加载数量
        public int preLoadCount;

        //item数量
        public int itemCount {
            get { return mAdapter != null ? mAdapter.GetCount() : 0; }
        }

        //显示窗口初始大小
        private Rect oriViewPortRect;

        //列表适配器
        private IListAdapter mAdapter;

        //滚动条
        private Scrollbar scrollbar;

        //布局
        private HorizontalOrVerticalLayoutGroup layout;

        //item大小
        private Rect itemSize;

        //间隔大小
        private Rect dividerRect;

        //网格列表
        private List<Vector2> itemPointList;

        //加载项
        private GameObject[] loadItems;

        //加载项窗口
        private Window loadWindow;

        //当前位置
        private int currentPosition;


        private void Start(){
            mAdapter = new SimpleListAdapter(new List<TextData>() {
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
                new TextData("0"),
            }, new SimpleItemAdapter());

            //初始化数据
            InitData();
            //初始化界面
            InitView();
        }

        private void Update(){
            if (mAdapter != null) {
                //适配器数据数量与界面网格数量的差值
                int diff = mAdapter.GetCount() - itemPointList.Count;

                //如果数据个数有变化需要更新界面
                if (diff != 0) {
                    ItemCountChange(diff);
                }

                UpdateView();
                UpdateData();

                if (Input.GetKeyDown("q")) {
                    loadWindow.MoveBack();
                    Debug.Log(loadWindow.ToString());
                }

                if (Input.GetKeyDown("e")) {
                    loadWindow.MoveForward();
                    Debug.Log(loadWindow.ToString());
                }
            }
        }

        //初始化数据
        private void InitData(){
            //初始化当前位置
            currentPosition = 0;

            //初始化窗口
            loadWindow = new Window(perPageCount + 2 * preLoadCount);
            loadWindow.SetPosition(-preLoadCount);

            //初始化加载数组
            loadItems = new GameObject[perPageCount + 2 * preLoadCount];

            //初始化网格位置列表
            itemPointList = new List<Vector2>();

            //初始化item大小
            var fullRect = scrollRect.GetComponent<RectTransform>().rect;
            oriViewPortRect = viewPort.GetComponent<RectTransform>().rect;
            if (type == ListType.Vertical) {
                itemSize.width = oriViewPortRect.width;
                itemSize.height = (fullRect.height - GetDividerSize() * (perPageCount - 1)) / perPageCount;
            }
            else {
                itemSize.width = (fullRect.width - GetDividerSize() * (perPageCount - 1)) / perPageCount;
                itemSize.height = oriViewPortRect.height;
            }
        }

        //初始化布局
        private void InitView(){
            RectTransform fullRect = scrollRect.transform as RectTransform;
            RectTransform viewRect = viewPort.transform as RectTransform;
            if (type == ListType.Vertical) {
                //初始化滑动界面
                scrollRect.horizontal = false;
                scrollRect.vertical = true;
                scrollbar = scrollRect.verticalScrollbar;

                //初始化显示窗口
                if (fullRect != null && viewRect != null) {
                    viewRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, fullRect.rect.height);
                    scrollbar.GetComponent<RectTransform>()
                        .SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, fullRect.rect.height);
                }
            }
            else {
                //初始化滑动界面
                scrollRect.vertical = false;
                scrollRect.horizontal = true;
                scrollbar = scrollRect.horizontalScrollbar;

                //初始化显示窗口
                if (fullRect != null && viewRect != null) {
                    viewRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fullRect.rect.width);
                    scrollbar.GetComponent<RectTransform>()
                        .SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fullRect.rect.width);
                }
            }
        }

        //更新界面
        void UpdateView(){
            if (currentPosition != GetCurrentItemId()) {
                loadWindow.SetPosition(GetCurrentItemId() - preLoadCount);

                int posDiff = GetCurrentItemId() - currentPosition;
                //print("移动" + posDiff + "原：" + currentPosition + "当前:" + GetCurrentItemId());
                if (posDiff > 0) {
                    for (int i = loadItems.Length - 1; i >= loadItems.Length - posDiff ; i--) {
                        AdjustItem(i);
                    }
                }
                else if (posDiff < 0) {
                    for (int i = 0; i < -posDiff; i++) {
                        AdjustItem(i);
                    }
                }


                AdjustItemsPosition();
                currentPosition = GetCurrentItemId();
            }

            UpdateItems();
        }

        //更新数据
        void UpdateData(){
        }

        //根据偏移平滑滚动到适配器位置
        public void SmoothScrollByOffset(int offset){
        }

        //平滑滚动到指定适配器位置
        public void SmoothScrollToPosition(int position){
        }

        //设置适配器
        public void SetAdapter(IListAdapter adapter){
            mAdapter = adapter;
        }

        //获取适配器
        public IListAdapter GetAdapter(){
            return mAdapter;
        }

        //获取间隔GameObject
        public GameObject GetDivider(){
            if (divider != null)
                return divider;
            return null;
        }

        //获取间隔的高度
        public float GetDividerSize(){
            if (divider != null)
                return dividerSize;
            return 0;
        }

        //生成一个item
        GameObject createItem(){
            GameObject newItem = Instantiate(itemPre);
            RectTransform itemTransform = newItem.GetComponent<RectTransform>();
            itemTransform.pivot = new Vector2(0, 1);
            itemTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemSize.width);
            itemTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, itemSize.height);
            return newItem;
        }

        void ItemCountChange(int diff){
            //更新window大小
            loadWindow.Min = -preLoadCount;
            loadWindow.Max = mAdapter.GetCount() - 1 + preLoadCount;
            if (loadWindow.Position > loadWindow.Max || loadWindow.Position < loadWindow.Min)
                loadWindow.SetPosition(loadWindow.Min);
            else
                loadWindow.SetPosition(loadWindow.Position);


            if (type == ListType.Vertical) {
                //更新网格列表
                if (diff > 0) {
                    for (int i = 0; i < diff; i++) {
                        var point = new Vector2(0, -itemPointList.Count * (itemSize.height + GetDividerSize()));
                        itemPointList.Add(point);
                    }
                }
                else {
                    itemPointList.RemoveRange(itemPointList.Count - diff, diff);
                }
            }
            else {
                //更新网格列表
                if (diff > 0) {
                    for (int i = 0; i < diff; i++) {
                        var point = new Vector2(itemPointList.Count * (itemSize.width + GetDividerSize()), 0);
                        itemPointList.Add(point);
                    }
                }
                else {
                    itemPointList.RemoveRange(itemPointList.Count - diff, diff);
                }
            }

            RectTransform contentRect = content.transform as RectTransform;
            RectTransform itemRect = itemPre.transform as RectTransform;
            if (type == ListType.Vertical) {
                //更新内容界面大小
                if (contentRect != null && itemRect != null) {
                    var newWidth = itemSize.width;
                    var newHeight = (itemSize.height + GetDividerSize()) * itemCount - GetDividerSize();
                    contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
                    contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
                }
            }
            else {
                //更新内容界面大小
                if (contentRect != null && itemRect != null) {
                    var newWidth = (itemSize.width + GetDividerSize()) * itemCount - GetDividerSize();
                    var newHeight = itemSize.height;
                    contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
                    contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
                }
            }

            //根据列表创建或隐藏item
            int index = 0;
            for (; index < loadWindow.content.Count; index++) {
                //AdjustItem(index);
            }

            Update();

            AdjustItemsPosition();
        }

        //从界面获取当前的item的Id
        int GetCurrentItemId(){
            if (mAdapter.GetCount() < perPageCount)
                return 0;
            if (type == ListType.Vertical) {
                return (int) ((1 - scrollbar.value) * (itemPointList.Count - perPageCount));
            }
            else {
                return (int) (scrollbar.value * (itemPointList.Count - perPageCount));
            }
        }

        //调整显示item的位置
        void AdjustItemsPosition(){
            int begin = GetCurrentItemId() - preLoadCount;
            for (int i = 0; i < loadItems.Length; i++) {
                if (loadItems[i] != null && loadItems[i].transform.parent != content.transform)
                    loadItems[i].transform.parent = content.transform;

                if (begin + i >= 0 && begin + i < itemPointList.Count && loadItems[i] != null &&
                    loadItems[i].activeSelf == true) {
                    loadItems[i].GetComponent<RectTransform>().localPosition = itemPointList[begin + i];
                }
            }
        }

        //调整item
        void AdjustItem(int index){
            if (loadItems[index] == null) {
                loadItems[index] = createItem();
            }

            if (loadWindow.content[index] >= 0 && loadWindow.content[index] < mAdapter.GetCount()) {
                if (loadItems[index].activeSelf == false)
                    loadItems[index].SetActive(true);
                mAdapter.InitItem(loadItems[index], mAdapter.GetItem(loadWindow.content[index]));
                //print("显示item,index" + index + ",item" + loadWindow.content[index]);
            }
            else if (loadItems[index] != null && loadItems[index].activeSelf) {
                loadItems[index].SetActive(false);
                //print("隐藏item,index" + index + ",item" + loadWindow.content[index]);
            }
        }

        //更新显示中的item
        void UpdateItems(){
            for (int i = 0; i < loadWindow.content.Count; i++) {
                if (!loadItems[i].activeSelf && loadWindow.content[i] >= 0 &&
                    loadWindow.content[i] < mAdapter.GetCount()) {
                    loadItems[i].SetActive(true);
                    AdjustItemsPosition();
                }

                if (loadWindow.content[i] < 0 || loadWindow.content[i] >= mAdapter.GetCount()) {
                    loadItems[i].SetActive(false);
                    AdjustItemsPosition();
                }

                if (loadItems[i] != null && loadItems[i].activeSelf) {
                    mAdapter.UpdateItem(loadItems[i], mAdapter.GetItem(loadWindow.content[i]));
                }

                loadItems[i].name = "object:" + i + ",item:" + loadWindow.content[i];
            }
        }
    }
}