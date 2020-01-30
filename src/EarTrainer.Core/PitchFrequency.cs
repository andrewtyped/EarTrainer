using System;

using EarTrainer.Core.Constants;

namespace EarTrainer.Core
{
    public class PitchFrequency : Pitch,
                                  IEquatable<PitchFrequency>
    {
        public bool Equals(PitchFrequency other)
        {
            throw new NotImplementedException();
        }

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

            return this.Equals((PitchFrequency)obj);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(PitchFrequency left,
                                       PitchFrequency right)
        {
            return Equals(left,
                          right);
        }

        public static bool operator !=(PitchFrequency left,
                                       PitchFrequency right)
        {
            return !Equals(left,
                           right);
        }

        public PitchFrequency(Note note,
                              int octave,
                              double frequency)
            : base(note,
                   octave)
        {
            this.Frequency = frequency;
        }

        public double Frequency
        {
            get;
        }
    }
}