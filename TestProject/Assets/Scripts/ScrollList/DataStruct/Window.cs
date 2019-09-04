using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ScrollList.DataStruct{
    public class Window{
        private int _min;
        private int _max;
        private int _position;
        private int length;
        private List<int> _content;

        public int Min {
            get => _min;
            set {
                _min = value;
                while (_content.Count != 0 && _content[0] < _min) {
                    _content.RemoveAt(0);
                }
            }
        }

        public int Max {
            get => _max;
            set {
                _max = value;
                while (_content.Count != 0 && _content[_content.Count - 1] > _max) {
                    _content.RemoveAt(_content.Count - 1);
                }
            }
        }

        public List<int> content {
            get => _content;
        }

        public int Position => _position;

        public Window(int length) : this(int.MinValue, int.MaxValue, length){
        }

        public Window(int min, int max, int length){
            _content = new List<int>();
            _min = min;
            _max = max;
            this.length = length;
            SetPosition(0);
        }

        //设置窗口位置
        public void SetPosition(int position){
            _content.Clear();
            for (int i = 0; i < length; i++)
                if (position + i <= _max && position + i >= _min)
                    _content.Add(position + i);
            _position = position;
        }

        //向前移动窗口
        public void MoveForward(){
            _position++;
            SetPosition(_position);
        }

        //向后移动窗口
        public void MoveBack(){
            _position--;
            SetPosition(_position);
        }

        public override string ToString(){
            StringBuilder builder = new StringBuilder();
            builder.Append("Window[Position:" + _position + ",min:" + _min + ",max:" + _max + ",[");
            foreach (var i in _content) {
                builder.Append(i.ToString() + ",");
            }

            builder.Remove(builder.Length - 1, 1);
            builder.Append("]");
            return builder.ToString();
        }
    }
}