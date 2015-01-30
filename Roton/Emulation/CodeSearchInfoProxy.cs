using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    sealed internal class CodeSearchInfoProxy : CodeSearchInfo
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

        public CodeSearchInfoProxy(Func<int> getIndex, Action<int> setIndex, Func<string> getLabel, Action<string> setLabel, Func<int> getOffset, Action<int> setOffset)
            : base(null)
        {
            this._getIndex = getIndex;
            this._getLabel = getLabel;
            this._getOffset = getOffset;
            this._setIndex = setIndex;
            this._setLabel = setLabel;
            this._setOffset = setOffset;
        }

        public override int Index
        {
            get
            {
                if (_getIndex != null)
                {
                    return _getIndex();
                }
                return _index;
            }
            set
            {
                if (_setIndex != null)
                {
                    _setIndex(value);
                }
                _index = value;
            }
        }

        public override string Label
        {
            get
            {
                if (_getLabel != null)
                {
                    return _getLabel();
                }
                return _label;
            }
            set
            {
                if (_setLabel != null)
                {
                    _setLabel(value);
                }
                _label = value;
            }
        }

        public override int Offset
        {
            get
            {
                if (_getOffset != null)
                {
                    return _getOffset();
                }
                return _offset;
            }
            set
            {
                if (_setOffset != null)
                {
                    _setOffset(value);
                }
                _offset = value;
            }
        }
    }
}
