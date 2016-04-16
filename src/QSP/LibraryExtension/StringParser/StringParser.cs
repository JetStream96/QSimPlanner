using System;

namespace QSP.LibraryExtension.StringParser
{
    public class StringParser
    {
        private string text;

        // Still need this because property cannot be passed with ref keyword.
        private int _currentIndex;

        public int CurrentIndex
        {
            get
            {
                return _currentIndex;
            }

            set
            {
                if (value < 0 || value >= text.Length)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _currentIndex = value;
            }
        }

        public int Length
        {
            get
            {
                return text.Length;
            }
        }

        public StringParser(string text)
        {
            this.text = text;
            _currentIndex = 0;
        }

        public StringParser(StringParser sp)
        {
            text = sp.text;
            CurrentIndex = sp.CurrentIndex;
        }

        public string ReadString(char endChar)
        {
            return Utilities.ReadString(text, ref _currentIndex, endChar);
        }

        public string ReadString(string target)
        {
            return Utilities.ReadString(text, ref _currentIndex, target);
        }

        public string ReadString(int index)
        {
            var s = text.Substring(CurrentIndex, index - CurrentIndex + 1);
            CurrentIndex = index;
            return s;
        }

        public int NextIndexOf(char c)
        {
            return text.IndexOf(c, CurrentIndex);
        }

        public int NextIndexOf(string s)
        {
            return text.IndexOf(s, CurrentIndex);
        }

        public bool SkipToNextLine()
        {
            return Utilities.SkipToNextLine(text, ref _currentIndex);
        }

        public void SkipAny(char[] charsToSkip)
        {
            Utilities.SkipAny(text, charsToSkip, ref _currentIndex);
        }

        public string ReadToNextDelimeter(char[] Delimeters)
        {
            return Utilities.ReadToNextDelimeter(text, Delimeters, ref _currentIndex);
        }

        public bool MoveToNextIndexOf(string target)
        {
            return Utilities.MoveToNextIndexOf(text, target, ref _currentIndex);
        }

        public bool MoveToNextIndexOf(char target)
        {
            return Utilities.MoveToNextIndexOf(text, target, ref _currentIndex);
        }

        public bool MoveRight(int steps)
        {
            int newIndex = CurrentIndex + steps;

            if (newIndex < text.Length)
            {
                CurrentIndex = newIndex;
                return true;
            }
            return false;
        }

        public bool MoveLeft(int steps)
        {
            int newIndex = CurrentIndex - steps;

            if (newIndex >= 0)
            {
                CurrentIndex = newIndex;
                return true;
            }
            return false;
        }

        public bool MoveToNextDigit()
        {
            for (int i = CurrentIndex; i < text.Length; i++)
            {
                if (text[i] >= '0' && text[i] <= '9')
                {
                    CurrentIndex = i;
                    return true;
                }
            }
            return false;
        }

        public int ParseInt()
        {
            return Utilities.ParseInt(text, CurrentIndex, out _currentIndex);
        }
    }
}
