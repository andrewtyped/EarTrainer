using System;
using System.Collections.Generic;

using EarTrainer.Core.Constants;

namespace EarTrainer.Core
{
    public class Pitch : IEquatable<Pitch>,
                         IComparable<Pitch>,
                         IComparable
    {
        #region Constructors

        public Pitch(Note note,
                     int octave)
        {
            this.Note = note;
            this.Octave = octave;
        }

        #endregion

        #region Instance Properties

        public Note Note
        {
            get;
        }

        public int Octave
        {
            get;
        }

        #endregion

        #region Instance Methods

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null,
                                obj))
            {
                return false;
            }

            if (ReferenceEquals(this,
                                obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((Pitch)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)this.Note * 397) ^ this.Octave;
            }
        }

        public int CompareTo(Pitch other)
        {
            if (ReferenceEquals(this,
                                other))
            {
                return 0;
            }

            if (ReferenceEquals(null,
                                other))
            {
                return 1;
            }

            var octaveComparison = this.Octave.CompareTo(other.Octave);
            if (octaveComparison != 0)
            {
                return octaveComparison;
            }

            return this.Note.CompareTo(other.Note);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null,
                                obj))
            {
                return 1;
            }

            if (ReferenceEquals(this,
                                obj))
            {
                return 0;
            }

            return obj is Pitch other
                       ? this.CompareTo(other)
                       : throw new ArgumentException($"Object must be of type {nameof(Pitch)}");
        }

        public bool Equals(Pitch other)
        {
            if (ReferenceEquals(null,
                                other))
            {
                return false;
            }

            if (ReferenceEquals(this,
                                other))
            {
                return true;
            }

            return this.Note == other.Note && this.Octave == other.Octave;
        }

        #endregion

        #region Class Methods

        public static bool operator ==(Pitch left,
                                       Pitch right)
        {
            return Equals(left,
                          right);
        }

        public static bool operator >(Pitch left,
                                      Pitch right)
        {
            return Comparer<Pitch>.Default.Compare(left,
                                                   right)
                   > 0;
        }

        public static bool operator >=(Pitch left,
                                       Pitch right)
        {
            return Comparer<Pitch>.Default.Compare(left,
                                                   right)
                   >= 0;
        }

        public static bool operator !=(Pitch left,
                                       Pitch right)
        {
            return !Equals(left,
                           right);
        }

        public static bool operator <(Pitch left,
                                      Pitch right)
        {
            return Comparer<Pitch>.Default.Compare(left,
                                                   right)
                   < 0;
        }

        public static bool operator <=(Pitch left,
                                       Pitch right)
        {
            return Comparer<Pitch>.Default.Compare(left,
                                                   right)
                   <= 0;
        }

        #endregion
    }
}