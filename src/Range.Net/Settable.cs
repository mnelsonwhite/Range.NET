namespace Range.Net
{
    internal struct Settable<T>
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

        public Settable(T value)
        {
            this.value = value;
            IsSet = true;
        }
    }
}
