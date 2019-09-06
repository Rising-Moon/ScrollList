using System;
using System.Collections.Generic;
using ScrollList.DataStruct;
using ScrollList.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace ScrollList.UI
{
    //水平&垂直
    public enum ListType
    {
        Vertical,
        Horizontal,
    }

    public class ScrollList : MonoBehaviour
    {
        //滚动界面
        public ScrollRect ScrollRect;

        //显示区域
        public GameObject ViewPort;

        //列表区域
        public GameObject Content;

        //Item
        public GameObject ItemPre;

        //间隔布局
        public GameObject Divider;

        //间隔高度
        public float DividerSize;

        //方向
        public ListType Type;

        //每页Item数量
        public int PerPageCount;

        //预加载数量
        public int PreLoadCount;

        //滚动条是否显示
        public bool ShowScrollBar = true;

        //item数量
        public int ItemCount => mAdapter != null ? mAdapter.GetCount() : 0;

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

        //分界线
        private List<GameObject> loadDividers;

        private void Start()
        {
            if (ItemPre)
            {
                //初始化数据
                InitData();
                //初始化界面
                InitView();
            }
            else
            {
                Debug.LogError("Need item prefab");
            }
        }

        private void LateUpdate()
        {
            if (mAdapter != null && ItemPre != null)
            {
                //适配器数据数量与界面网格数量的差值
                int itemCountDiff = mAdapter.GetCount() - itemPointList.Count;

                //如果数据个数有变化需要更新界面
                if (itemCountDiff != 0)
                {
                    OnItemCountChange(itemCountDiff);
                    AdjustItemsPosition();
                }

                //界面实际位置与最后一个位置的差值
                int diffPosition = GetCurrentItemId() - currentPosition;

                if (diffPosition != 0)
                {
                    OnPositionChange(diffPosition);
                    currentPosition = GetCurrentItemId();
                    AdjustItemsPosition();
                }

                UpdateAllItem();
            }
        }

        //初始化数据
        private void InitData()
        {
            //初始化当前位置
            currentPosition = 0;

            //初始化窗口
            loadWindow = new Window(PerPageCount + 2 * PreLoadCount);
            loadWindow.SetPosition(-PreLoadCount);

            //初始化加载数组
            loadItems = new GameObject[PerPageCount + 2 * PreLoadCount];

            //初始化分界线缓存
            loadDividers = new List<GameObject>();

            //初始化网格位置列表
            itemPointList = new List<Vector2>();

            //初始化item大小
            var fullRect = ScrollRect.GetComponent<RectTransform>().rect;
            oriViewPortRect = ViewPort.GetComponent<RectTransform>().rect;
            if (Type == ListType.Vertical)
            {
                itemSize.width = oriViewPortRect.width;
                itemSize.height = (fullRect.height - GetDividerSize() * (PerPageCount - 1)) / PerPageCount;
                dividerRect.width = oriViewPortRect.width;
                dividerRect.height = DividerSize;
            }
            else
            {
                itemSize.width = (fullRect.width - GetDividerSize() * (PerPageCount - 1)) / PerPageCount;
                itemSize.height = oriViewPortRect.height;
                dividerRect.width = DividerSize;
                dividerRect.height = oriViewPortRect.height;
            }
        }

        //初始化布局
        private void InitView()
        {
            RectTransform fullRect = ScrollRect.transform as RectTransform;
            RectTransform viewRect = ViewPort.transform as RectTransform;
            if (Type == ListType.Vertical)
            {
                //初始化滑动界面
                ScrollRect.horizontal = false;
                ScrollRect.vertical = true;
                scrollbar = ScrollRect.verticalScrollbar;

                //初始化显示窗口
                if (fullRect != null && viewRect != null)
                {
                    viewRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, fullRect.rect.height);
                    scrollbar.GetComponent<RectTransform>()
                        .SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, fullRect.rect.height);
                }
            }
            else
            {
                //初始化滑动界面
                ScrollRect.vertical = false;
                ScrollRect.horizontal = true;
                scrollbar = ScrollRect.horizontalScrollbar;

                //初始化显示窗口
                if (fullRect != null && viewRect != null)
                {
                    viewRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fullRect.rect.width);
                    scrollbar.GetComponent<RectTransform>()
                        .SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fullRect.rect.width);
                }
            }

            if (!ShowScrollBar)
            {
                var canvasGroup = scrollbar.GetComponent<CanvasGroup>();
                canvasGroup.alpha = 0;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        }

        //根据偏移平滑滚动到适配器位置
        public void SmoothScrollByOffset(int offset)
        {
            if (mAdapter == null)
                return;
            SmoothScrollToPosition(currentPosition + offset);
        }

        //平滑滚动到指定适配器位置
        public void SmoothScrollToPosition(int position)
        {
            if (mAdapter == null)
                return;
            float target = (float) position / (itemPointList.Count - PerPageCount) +
                           1 / (float) itemPointList.Count * 0.01f;
            print(target);
            if (Type == ListType.Vertical)
            {
                scrollbar.GetComponent<SmoothMoveUtil>().SmoothMoveTo(1 - target, 0.3f);
            }
            else
            {
                scrollbar.GetComponent<SmoothMoveUtil>().SmoothMoveTo(target, 0.3f);
            }
        }

        //设置适配器
        public void SetAdapter(IListAdapter adapter)
        {
            mAdapter = adapter;
        }

        //获取适配器
        public IListAdapter GetAdapter()
        {
            return mAdapter;
        }

        //获取间隔GameObject
        public GameObject GetDivider()
        {
            if (Divider != null)
                return Divider;
            return null;
        }

        //获取间隔的高度
        public float GetDividerSize()
        {
            if (Divider != null)
                return DividerSize;
            return 0;
        }

        //生成一个item
        GameObject CreateItem()
        {
            GameObject newItem = Instantiate(ItemPre);
            RectTransform itemTransform = newItem.GetComponent<RectTransform>();
            itemTransform.pivot = new Vector2(0, 1);
            itemTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemSize.width);
            itemTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, itemSize.height);
            return newItem;
        }

        //生成一条分界线
        GameObject CreateDivider()
        {
            GameObject newDivider = Instantiate(Divider);
            newDivider.name = "Divider";
            RectTransform dividerTransform = newDivider.GetComponent<RectTransform>();
            dividerTransform.SetParent(Content.transform);
            dividerTransform.pivot = new Vector2(0, 1);
            dividerTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, dividerRect.width);
            dividerTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, dividerRect.height);
            return newDivider;
        }

        //数据数量变化
        void OnItemCountChange(int diff)
        {
            //更新window大小
            loadWindow.Min = -PreLoadCount;
            loadWindow.Max = mAdapter.GetCount() - 1 + PreLoadCount;
            if (loadWindow.Position > loadWindow.Max || loadWindow.Position < loadWindow.Min)
                loadWindow.SetPosition(loadWindow.Min);
            else
                loadWindow.SetPosition(loadWindow.Position);


            if (Type == ListType.Vertical)
            {
                //更新网格列表
                if (diff > 0)
                {
                    for (int i = 0; i < diff; i++)
                    {
                        var point = new Vector2(0, -itemPointList.Count * (itemSize.height + GetDividerSize()));
                        itemPointList.Add(point);
                    }
                }
                else
                {
                    itemPointList.RemoveRange(itemPointList.Count - diff, diff);
                }
            }
            else
            {
                //更新网格列表
                if (diff > 0)
                {
                    for (int i = 0; i < diff; i++)
                    {
                        var point = new Vector2(itemPointList.Count * (itemSize.width + GetDividerSize()), 0);
                        itemPointList.Add(point);
                    }
                }
                else
                {
                    itemPointList.RemoveRange(itemPointList.Count - diff, diff);
                }
            }

            RectTransform contentRect = Content.transform as RectTransform;
            RectTransform itemRect = ItemPre.transform as RectTransform;
            if (Type == ListType.Vertical)
            {
                //更新内容界面大小
                if (contentRect != null && itemRect != null)
                {
                    var newWidth = itemSize.width;
                    var newHeight = (itemSize.height + GetDividerSize()) * ItemCount - GetDividerSize();
                    contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
                    contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
                }
            }
            else
            {
                //更新内容界面大小
                if (contentRect != null && itemRect != null)
                {
                    var newWidth = (itemSize.width + GetDividerSize()) * ItemCount - GetDividerSize();
                    var newHeight = itemSize.height;
                    contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
                    contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
                }
            }

            if (diff > 0)
            {
                for (int i = mAdapter.GetCount() - diff; i < mAdapter.GetCount(); i++)
                {
                    if (loadWindow.Content.Contains(i))
                    {
                        int index = loadWindow.Content.IndexOf(i);
                        InitItem(index);
                    }
                }
            }
        }

        //列表位置变化
        void OnPositionChange(int diff)
        {
            loadWindow.SetPosition(GetCurrentItemId() - PreLoadCount);

            //print("移动" + diff + "原：" + currentPosition + "当前:" + GetCurrentItemId());
            //向前移动
            if (diff > 0)
            {
                if (diff > loadItems.Length)
                {
                    for (int i = 0; i < loadItems.Length; i++)
                        InitItem(i);
                }
                else
                {
                    for (int i = 0; i < diff; i++)
                    {
                        GameObject temp = loadItems[0];
                        for (int j = 1; j < loadItems.Length; j++)
                            loadItems[j - 1] = loadItems[j];

                        loadItems[loadItems.Length - 1] = temp;
                    }

                    for (int i = loadItems.Length - 1; i >= loadItems.Length - diff; i--)
                        InitItem(i);
                }
            }
            //向后移动
            else if (diff < 0)
            {
                if (-diff > loadItems.Length)
                {
                    for (int i = 0; i < loadItems.Length; i++)
                        InitItem(i);
                }
                else
                {
                    for (int i = 0; i < -diff; i++)
                    {
                        GameObject temp = loadItems[loadItems.Length - 1];
                        for (int j = loadItems.Length - 2; j >= 0; j--)
                            loadItems[j + 1] = loadItems[j];
                        loadItems[0] = temp;
                    }

                    for (int i = 0; i < -diff; i++)
                        InitItem(i);
                }
            }
        }

        //从界面获取当前的item的Id
        int GetCurrentItemId()
        {
            if (mAdapter.GetCount() <= PerPageCount)
                return 0;
            if (Type == ListType.Vertical)
            {
                return (int) ((1 - scrollbar.value) * (itemPointList.Count - PerPageCount));
            }
            else
            {
                return (int) (scrollbar.value * (itemPointList.Count - PerPageCount));
            }
        }

        //调整显示item的位置
        void AdjustItemsPosition()
        {
            int begin = GetCurrentItemId() - PreLoadCount;
            for (int i = 0; i < loadItems.Length; i++)
            {
                if (loadItems[i] == null)
                    continue;

                if (loadItems[i].transform.parent != Content.transform)
                    loadItems[i].transform.SetParent(Content.transform);

                if (begin + i >= 0 && begin + i < itemPointList.Count)
                {
                    var itemTransform = loadItems[i].GetComponent<RectTransform>();
                    itemTransform.localPosition = itemPointList[begin + i];
                    //绘制分界线
                    if (Divider != null)
                    {
                        while (loadDividers.Count <= i)
                            loadDividers.Add(CreateDivider());
                        var dividerTransform = loadDividers[i].GetComponent<RectTransform>();
                        var dividerPosition = itemPointList[begin + i];
                        if (Type == ListType.Vertical)
                            dividerPosition.y -= itemSize.height;
                        else
                            dividerPosition.x += itemSize.width;
                        dividerTransform.localPosition = dividerPosition;
                    }
                }
            }
        }

        //初始化item
        void InitItem(int index)
        {
            if (loadItems[index] == null)
            {
                loadItems[index] = CreateItem();
            }

            if (loadWindow.Content[index] >= 0 && loadWindow.Content[index] < mAdapter.GetCount())
            {
                mAdapter.InitItem(loadItems[index], mAdapter.GetItem(loadWindow.Content[index]));
            }
        }

        //更新显示中的item
        void UpdateAllItem()
        {
            for (int i = 0; i < loadWindow.Content.Count; i++)
            {
                if (loadItems[i] == null)
                    continue;
                if (!loadItems[i].activeSelf && loadWindow.Content[i] >= 0 &&
                    loadWindow.Content[i] < mAdapter.GetCount())
                {
                    loadItems[i].SetActive(true);
                }

                if (loadWindow.Content[i] < 0 || loadWindow.Content[i] >= mAdapter.GetCount())
                {
                    loadItems[i].SetActive(false);
                }

                if (loadItems[i].activeSelf)
                {
                    mAdapter.UpdateItem(loadItems[i], mAdapter.GetItem(loadWindow.Content[i]));
                }
            }
        }
    }
}