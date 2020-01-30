using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

using EarTrainer.Core.Constants;

namespace EarTrainer.Core.Test
{
    [TestClass]
    public class EqualTemperamentTest
    {
        private EqualTemperament equalTemperament;

        [TestInitialize]
        public void TestInitialize()
        {
            this.equalTemperament = new EqualTemperament();
        }

        [DataTestMethod]
        [DataRow(Note.A, 0, 27.5d,0d)]
        [DataRow(Note.A, 1, 55d,0d)]
        [DataRow(Note.A, 2, 110d,0d)]
        [DataRow(Note.A, 3, 220d,0d)]
        [DataRow(Note.A, 4, 440d,0d)]
        [DataRow(Note.A, 5, 880d,0d)]
        [DataRow(Note.A, 6, 1760d,0d)]
        [DataRow(Note.A, 7, 3520d,0d)]
        [DataRow(Note.A, 8, 7040d,0d)]
        [DataRow(Note.C, 4, 261.626d, 0.005d)]
        [DataRow(Note.C, 5, 523.251d, 0.0005d)]
        [DataRow(Note.C, 0, 16.351d, 0.0006d)]
        [DataRow(Note.B, 8, 7902.132d, 0.0005d)]
        public void AssertPitchFrequency(Note note, int octave, double expectedFrequency, double tolerance)
        {
            var pitch = new Pitch(note,
                                  octave);

            var frequency = this.equalTemperament.PitchFrequencies[pitch];

            AssertFrequencyEqual(expectedFrequency,
                                 frequency,
                                 tolerance);
        }


        [DataTestMethod]
        [DataRow(Note.C, 4, Interval.MajorThird, 329.628d)]
        [DataRow(Note.B, 7, Interval.MajorSeventh, 7458.62d)]
        [DataRow(Note.D, 2, Interval.MajorSixth, 123.47d)]
        public void AssertInterval(Note note, int octave, Interval interval, double expectedFrequency)
        {
            var pitch = new Pitch(note,
                                  octave);

            var frequency = this.equalTemperament.GetIntervalFrequency(pitch,
                                                                       interval);

            AssertFrequencyEqual(expectedFrequency,
                                 frequency,
                                 //TODO: Reduce tolerance. Need to re-examine how we compute intervals. May need to employ P * 2 ^ cents / 1200 method
                                 0.002d);
        }

        [DataTestMethod]
        [DataRow(Note.C, 2, TriadCharacter.Major, new double[]{65.406d, 82.407d, 97.999d})]
        public void AssertTriad(Note root, int rootOctave, TriadCharacter triadCharacter, double[] expectedFrequencies)
        {
            var pitch = new Pitch(root,
                                  rootOctave);

            var frequencies = this.equalTemperament.GetTriadFrequencies(pitch,
                                                                        triadCharacter);

            Assert.AreEqual(expectedFrequencies.Length,
                            frequencies.Length);

            for(int i = 0; i < expectedFrequencies.Length; i++)
            {
                AssertFrequencyEqual(expectedFrequencies[i],
                                     frequencies[i],
                                     0.002d);
            }
        }

        [DataTestMethod]
        [DataRow(Note.E, 5, TriadCharacter.Major, new double[]{659.255d, 830.61d, 987.767d, 1244.508d})]
        public void AssertSeventhChordFrequencies(Note root, int rootOctave, SeventhChordCharacter seventhChordCharacter, double[] expectedFrequencies)
        {
            var pitch = new Pitch(root,
                                  rootOctave);

            var frequencies = this.equalTemperament.GetSeventhChordFrequencies(pitch,
                                                                               seventhChordCharacter);

            Assert.AreEqual(expectedFrequencies.Length,
                            frequencies.Length);

            for(int i = 0; i < expectedFrequencies.Length; i++)
            {
                AssertFrequencyEqual(expectedFrequencies[i],
                                     frequencies[i],
                                     0.002d);
            }
        }

        private static void AssertFrequencyEqual(double expected, double actual, double tolerance)
        {
            var delta = Math.Abs(expected - actual);
            Assert.IsTrue(delta <= tolerance, $"Expected {expected} but was {actual} with tolerance {tolerance}, delta {delta}");
        }
    }
}
