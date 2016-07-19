﻿using System;

namespace Roton.Emulation
{
    internal sealed class CodeSearchInfoProxy : CodeSearchInfo
    {
        private Func<int> _getIndex;
        private Func<string> _getLabel;
        private Func<int> _getOffset;
        private int _index;
        private string _label;
        private int _offset;
        private Action<int> _setIndex;
        private Action<string> _setLabel;
        private Action<int> _setOffset;

        public CodeSearchInfoProxy(Func<int> getIndex, Action<int> setIndex, Func<string> getLabel,
            Action<string> setLabel, Func<int> getOffset, Action<int> setOffset)
            : base(null)
        {
            _getIndex = getIndex;
            _getLabel = getLabel;
            _getOffset = getOffset;
            _setIndex = setIndex;
            _setLabel = setLabel;
            _setOffset = setOffset;
        }

        public override int Index
        {
            get
            {
                return _getIndex?.Invoke() ?? _index;
            }
            set
            {
                _setIndex?.Invoke(value);
                _index = value;
            }
        }

        public override string Label
        {
            get
            {
                return _getLabel != null ? _getLabel() : _label;
            }
            set
            {
                _setLabel?.Invoke(value);
                _label = value;
            }
        }

        public override int Offset
        {
            get
            {
                return _getOffset?.Invoke() ?? _offset;
            }
            set
            {
                _setOffset?.Invoke(value);
                _offset = value;
            }
        }
    }
}