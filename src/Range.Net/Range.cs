using Range.Net.Abstractions;
using System;

namespace Range.Net
{
    /// <summary>
    /// Generic range class.
    /// Inclusivity is set for min and max by default
    /// </summary>
    /// <typeparam name="T">Constrained to IComparable</typeparam>
    public sealed class Range<T> : IRange<T> where T : IComparable<T>
    {
        private T _minimum;
        private T _maximum;
        
        /// <summary>
        /// Default Inclusivity is set to InclusiveMinInclusiveMax
        /// </summary>
        public Range()
        {
            Inclusivity = RangeInclusivity.InclusiveMinInclusiveMax;
        }

        /// <param name="minimum">Minimum value</param>
        /// <param name="maximum">Maximum value</param>
        public Range(T minimum, T maximum) : this()
        {
            var reverse = minimum.CompareTo(maximum) > 0;
            _minimum = reverse ? maximum : minimum;
            _maximum = reverse ? minimum : maximum;
        }

        /// <summary>
        /// Minimum value of the range
        /// </summary>
        public T Minimum
        {
            get => _minimum;
            set
            {
                if (value.CompareTo(Maximum) > 0)
                {
                    _minimum = _maximum;
                    _maximum = value;
                }
                else
                {
                    _minimum = value;
                }
            }
        }

        /// <summary>
        /// Maximum value of the range
        /// </summary>
        public T Maximum
        {
            get => _maximum;
            set
            {
                if (Minimum.CompareTo(value) > 0)
                {
                    _maximum = _minimum;
                    _minimum = value;
                }
                else
                {
                    _maximum = value;
                }
            }
        }

        /// <summary>
        /// Gets or Sets the inclusivity of the range
        /// </summary>
        public RangeInclusivity Inclusivity { get; set; }

        /// <summary>
        /// Presents the Range in readable format
        /// </summary>
        /// <returns>String representation of the Range</returns>
        public override string ToString()
        {
            return $"[{Minimum} - {Maximum}]"; 
        }
        
        /// <summary>
        /// Override of GetHashCode to allow equality
        /// </summary>
        /// <returns>integer hash code representing the value of this instance</returns>
        public override int GetHashCode() => HashCode.Combine(Minimum, Maximum, Inclusivity);

        /// <summary>
        /// Use the overridden GetHashCode method to test equality
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if both object have the same hash code value</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;

            var val = obj as Range<T>;
            return !ReferenceEquals(val, null) &&
                Minimum.CompareTo(val.Minimum) == 0 &&
                Maximum.CompareTo(val.Maximum) == 0 &&
                Equals(Inclusivity, val.Inclusivity);
        }
    }
}