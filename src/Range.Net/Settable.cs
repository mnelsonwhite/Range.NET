﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Range.Net
{
    internal class Settable<T>
    {
        private T value;

        public T Value
        {
            get => IsSet ? value : default;
            set {
                this.value = value;
                IsSet = true;
            }
        }

        public bool IsSet { get; private set; }

        public Settable()
        {

        }

        public Settable(T value)
        {
            this.value = value;
            IsSet = true;
        }
    }
}