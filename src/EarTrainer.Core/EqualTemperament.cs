using System;
using System.Collections.Generic;

using EarTrainer.Core.Constants;

namespace EarTrainer.Core
{
    public class EqualTemperament : ITemperament
    {
        #region Fields

        private static readonly Dictionary<Interval, double> intervalRatios = new Dictionary<Interval, double>
                                                                              {
                                                                                  {
                                                                                      Interval.Unison, 1d
                                                                                  },
                                                                                  {
                                                                                      Interval.MinorSecond, 1.059463d
                                                                                  },
                                                                                  {
                                                                                      Interval.MajorSecond, 1.122462d
                                                                                  },
                                                                                  {
                                                                                      Interval.MinorThird, 1.189207d
                                                                                  },
                                                                                  {
                                                                                      Interval.MajorThird, 1.259921d
                                                                                  },
                                                                                  {
                                                                                      Interval.PerfectFourth, 1.334840d
                                                                                  },
                                                                                  {
                                                                                      Interval.Tritone, 1.414214d
                                                                                  },
                                                                                  {
                                                                                      Interval.PerfectFifth, 1.498307d
                                                                                  },
                                                                                  {
                                                                                      Interval.MinorSixth, 1.587401d
                                                                                  },
                                                                                  {
                                                                                      Interval.MajorSixth, 1.681793d
                                                                                  },
                                                                                  {
                                                                                      Interval.MinorSeventh, 1.781797d
                                                                                  },
                                                                                  {
                                                                                      Interval.MajorSeventh, 1.887749d
                                                                                  },
                                                                                  {
                                                                                      Interval.Octave, 2d
                                                                                  }
                                                                              };

        private static readonly Dictionary<SeventhChordCharacter, ISet<Interval>> seventhChordIntervals = new Dictionary<SeventhChordCharacter, ISet<Interval>>
                                                                                                          {
                                                                                                              {
                                                                                                                  SeventhChordCharacter.Major, new HashSet<Interval>
                                                                                                                                               {
                                                                                                                                                   Interval.Unison,
                                                                                                                                                   Interval.MajorThird,
                                                                                                                                                   Interval.PerfectFifth,
                                                                                                                                                   Interval.MajorSeventh
                                                                                                                                               }
                                                                                                              },
                                                                                                              {
                                                                                                                  SeventhChordCharacter.Minor, new HashSet<Interval>
                                                                                                                                               {
                                                                                                                                                   Interval.Unison,
                                                                                                                                                   Interval.MinorThird,
                                                                                                                                                   Interval.PerfectFifth,
                                                                                                                                                   Interval.MinorSeventh
                                                                                                                                               }
                                                                                                              },
                                                                                                              {
                                                                                                                  SeventhChordCharacter.Dominant, new HashSet<Interval>
                                                                                                                                                  {
                                                                                                                                                      Interval.Unison,
                                                                                                                                                      Interval.MajorThird,
                                                                                                                                                      Interval.PerfectFifth,
                                                                                                                                                      Interval.MinorSeventh
                                                                                                                                                  }
                                                                                                              },
                                                                                                              {
                                                                                                                  SeventhChordCharacter.HalfDiminished, new HashSet<Interval>
                                                                                                                                                        {
                                                                                                                                                            Interval.Unison,
                                                                                                                                                            Interval.MinorThird,
                                                                                                                                                            Interval.Tritone,
                                                                                                                                                            Interval.MinorSeventh
                                                                                                                                                        }
                                                                                                              },
                                                                                                              {
                                                                                                                  SeventhChordCharacter.Diminished, new HashSet<Interval>
                                                                                                                                                    {
                                                                                                                                                        Interval.Unison,
                                                                                                                                                        Interval.MinorThird,
                                                                                                                                                        Interval.Tritone,
                                                                                                                                                        Interval.MajorSixth
                                                                                                                                                    }
                                                                                                              }
                                                                                                          };

        private static readonly Dictionary<TriadCharacter, ISet<Interval>> triadIntervals = new Dictionary<TriadCharacter, ISet<Interval>>
                                                                                            {
                                                                                                {
                                                                                                    TriadCharacter.Major, new HashSet<Interval>
                                                                                                                          {
                                                                                                                              Interval.Unison,
                                                                                                                              Interval.MajorThird,
                                                                                                                              Interval.PerfectFifth
                                                                                                                          }
                                                                                                },
                                                                                                {
                                                                                                    TriadCharacter.Minor, new HashSet<Interval>
                                                                                                                          {
                                                                                                                              Interval.Unison,
                                                                                                                              Interval.MinorThird,
                                                                                                                              Interval.PerfectFifth
                                                                                                                          }
                                                                                                },
                                                                                                {
                                                                                                    TriadCharacter.Augmented, new HashSet<Interval>
                                                                                                                              {
                                                                                                                                  Interval.Unison,
                                                                                                                                  Interval.MajorThird,
                                                                                                                                  Interval.MinorSixth
                                                                                                                              }
                                                                                                },
                                                                                                {
                                                                                                    TriadCharacter.Diminished, new HashSet<Interval>
                                                                                                                               {
                                                                                                                                   Interval.Unison,
                                                                                                                                   Interval.MinorThird,
                                                                                                                                   Interval.Tritone
                                                                                                                               }
                                                                                                }
                                                                                            };

        private readonly Lazy<IReadOnlyDictionary<Pitch, double>> lazyPitchFrequencies;

        #endregion

        #region Constructors

        public EqualTemperament()
        {
            this.lazyPitchFrequencies = new Lazy<IReadOnlyDictionary<Pitch, double>>(this.GetPitchFrequencies);
        }

        #endregion

        #region Instance Properties

        public IReadOnlyDictionary<Interval, double> IntervalRatios => intervalRatios;

        public IReadOnlyDictionary<Pitch, double> PitchFrequencies => this.lazyPitchFrequencies.Value;

        public PitchFrequency Reference =>
            new PitchFrequency(Note.A,
                               4,
                               440);

        public IReadOnlyDictionary<SeventhChordCharacter, ISet<Interval>> SeventhIntervals => seventhChordIntervals;

        public IReadOnlyDictionary<TriadCharacter, ISet<Interval>> TriadIntervals => triadIntervals;

        #endregion

        #region Instance Methods

        public double GetIntervalFrequency(Pitch root,
                                           Interval interval)
        {
            var pitchFrequency = this.PitchFrequencies[root];
            var intervalRatio = this.IntervalRatios[interval];

            return pitchFrequency * intervalRatio;
        }

        public double[] GetRootRelativeIntervalFrequencies(Pitch root,
                                                           ISet<Interval> intervals)
        {
            var pitchFrequency = this.PitchFrequencies[root];

            var frequencies = new double[intervals.Count];

            int i = 0;

            foreach (var interval in intervals)
            {
                frequencies[i++] = pitchFrequency * this.IntervalRatios[interval];
            }

            return frequencies;
        }

        public double[] GetSeventhChordFrequencies(Pitch root,
                                                   SeventhChordCharacter seventhChordCharacter)
        {
            var seventhIntervals = this.SeventhIntervals[seventhChordCharacter];
            return this.GetRootRelativeIntervalFrequencies(root,
                                                           seventhIntervals);
        }

        public double[] GetTriadFrequencies(Pitch root,
                                            TriadCharacter triadCharacter)
        {
            var triadIntervals = this.TriadIntervals[triadCharacter];
            return this.GetRootRelativeIntervalFrequencies(root,
                                                           triadIntervals);
        }

        private IReadOnlyDictionary<Pitch, double> GetPitchFrequencies()
        {
            var pitchFrequencies = new Dictionary<Pitch, double>();

            var currentNote = new Pitch(Note.A,
                                        4);
            var referencePitch = 440d;
            pitchFrequencies[currentNote] = referencePitch;
            var maxNote = new Pitch(Note.B,
                                    8);

            var currentInterval = Interval.MinorSecond;

            while (currentNote <= maxNote)
            {
                //Look up interval ratio instead of using successive minor seconds to avoid rounding errors at extreme octaves.
                var currentPitch = referencePitch * this.IntervalRatios[currentInterval++];

                currentNote = currentNote.Note == Note.B
                                  ? new Pitch(Note.C,
                                              currentNote.Octave + 1)
                                  : new Pitch(currentNote.Note + 1,
                                              currentNote.Octave);

                if (currentInterval > Interval.Octave)
                {
                    currentInterval = Interval.MinorSecond;
                    referencePitch = currentPitch; //reference pitch * 2 (octave)
                }

                pitchFrequencies[currentNote] = currentPitch;
            }

            var minNote = new Pitch(Note.C,
                                    0);

            currentNote = new Pitch(Note.A,
                                    4);
            referencePitch = 440d;
            currentInterval = Interval.MinorSecond;
            
            while (currentNote >= minNote)
            {
                var currentPitch = referencePitch / this.IntervalRatios[currentInterval++];
                
                currentNote = currentNote.Note == Note.C
                                  ? new Pitch(Note.B,
                                              currentNote.Octave - 1)
                                  : new Pitch(currentNote.Note - 1,
                                              currentNote.Octave);

                if (currentInterval > Interval.Octave)
                {
                    currentInterval = Interval.MinorSecond;
                    referencePitch = currentPitch; //reference pitch / 2 (octave)
                }

                pitchFrequencies[currentNote] = currentPitch;
            }

            return pitchFrequencies;
        }

        #endregion
    }
}