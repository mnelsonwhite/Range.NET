using Range.Net.Abstractions;
using System;

namespace Range.Net
{
    /// <summary>
    /// Generic range class.
    /// Inclusivity is set for min and max by default
    /// </summary>
    /// <typeparam name="T">Constrained to IComparable</typeparam>
    public struct Range<T> : IRange<T> where T : IComparable<T>
    {
        private Settable<T> _minimum;
        private Settable<T> _maximum;

        /// <param name="minimum">Minimum value</param>
        /// <param name="maximum">Maximum value</param>
        public Range(
            T minimum,
            T maximum,
            RangeInclusivity inclusivity = RangeInclusivity.InclusiveMinInclusiveMax)
        {
            Inclusivity = inclusivity;
            var reverse = minimum.CompareTo(maximum) > 0;
            _minimum = new Settable<T>(reverse ? maximum : minimum);
            _maximum = new Settable<T>(reverse ? minimum : maximum);
        }

        /// <summary>
        /// Minimum value of the range
        /// </summary>
        public T Minimum
        {
            get => _minimum.Value;
            set
            {
                if (_minimum.IsSet && _maximum.IsSet && value.CompareTo(Maximum) > 0)
                {
                    _minimum.Value = _maximum.Value;
                    _maximum.Value = value;
                }
                else
                {
                    _minimum.Value = value;
                }
            }
        }

        /// <summary>
        /// Maximum value of the range
        /// </summary>
        public T Maximum
        {
            get => _maximum.Value;
            set
            {
                if (_minimum.IsSet && _maximum.IsSet && Minimum.CompareTo(value) > 0)
                {
                    _maximum.Value = _minimum.Value;
                    _minimum.Value = value;
                }
                else
                {
                    _maximum.Value = value;
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

            var val = obj as Range<T>?;
            return !ReferenceEquals(val, null) &&
                Minimum.CompareTo(val.Value.Minimum) == 0 &&
                Maximum.CompareTo(val.Value.Maximum) == 0 &&
                Equals(Inclusivity, val.Value.Inclusivity);
        }
    }
}