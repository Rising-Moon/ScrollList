using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ScrollList.DataStruct
{
    public class Window
    {
        private int min;
        private int max;
        private int position;
        private int length;
        private List<int> content;

        public int Min
        {
            get => min;
            set
            {
                min = value;
                while (content.Count != 0 && content[0] < min)
                {
                    content.RemoveAt(0);
                }
            }
        }

        public int Max
        {
            get => max;
            set
            {
                max = value;
                while (content.Count != 0 && content[content.Count - 1] > max)
                {
                    content.RemoveAt(content.Count - 1);
                }
            }
        }

        public List<int> Content
        {
            get => content;
        }

        public int Position => position;

        public Window(int length) : this(int.MinValue, int.MaxValue, length) { }

        public Window(int min, int max, int length)
        {
            content = new List<int>();
            this.min = min;
            this.max = max;
            this.length = length;
            SetPosition(0);
        }

        //设置窗口位置
        public void SetPosition(int position)
        {
            content.Clear();
            for (int i = 0; i < length; i++)
                if (position + i <= max && position + i >= min)
                    content.Add(position + i);
            this.position = position;
        }

        //向前移动窗口
        public void MoveForward()
        {
            position++;
            SetPosition(position);
        }

        //向后移动窗口
        public void MoveBack()
        {
            position--;
            SetPosition(position);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Window[Position:" + position + ",min:" + min + ",max:" + max + ",[");
            foreach (var i in content)
            {
                builder.Append(i.ToString() + ",");
            }

            builder.Remove(builder.Length - 1, 1);
            builder.Append("]");
            return builder.ToString();
        }
    }
}