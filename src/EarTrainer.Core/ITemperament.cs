using System.Collections.Generic;

using EarTrainer.Core.Constants;

namespace EarTrainer.Core
{
    public interface ITemperament
    {
        #region Instance Properties

        IReadOnlyDictionary<Interval, double> IntervalRatios
        {
            get;
        }

        IReadOnlyDictionary<Pitch, double> PitchFrequencies
        {
            get;
        }

        PitchFrequency Reference
        {
            get;
        }

        IReadOnlyDictionary<SeventhChordCharacter, ISet<Interval>> SeventhIntervals
        {
            get;
        }

        IReadOnlyDictionary<TriadCharacter, ISet<Interval>> TriadIntervals
        {
            get;
        }

        #endregion

        #region Instance Methods

        double GetIntervalFrequency(Pitch root,
                                    Interval interval);

        double[] GetRootRelativeIntervalFrequencies(Pitch root,
                                                    ISet<Interval> intervals);

        double[] GetSeventhChordFrequencies(Pitch root,
                                            SeventhChordCharacter seventhChordCharacter);

        double[] GetTriadFrequencies(Pitch root,
                                     TriadCharacter triadCharacter);

        #endregion
    }
}