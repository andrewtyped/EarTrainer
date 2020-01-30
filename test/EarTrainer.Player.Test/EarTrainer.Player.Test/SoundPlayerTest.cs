using EarTrainer.Core;
using EarTrainer.Core.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarTrainer.Player.Test
{
    [TestClass]
    public class SoundPlayerTest
    {
        private SoundPlayer player;

        private WavWriter wavWriter;

        private EqualTemperament temperament;

        [TestInitialize]
        public void TestInitialize()
        {
            //Use synchronous mode to hear the player during testing. Otherwise the pitch is offloaded
            //to a new thread and never sounds b/c the test process has ended.
            this.player = new SoundPlayer(synchronous: true);
            this.wavWriter = new WavWriter();
            this.temperament = new EqualTemperament();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.player.Dispose();
            this.wavWriter.Dispose();
        }

        [TestMethod]
        public void PlayerPlaysTone()
        {
            var pitch = new Pitch(Note.A,
                                  4);

            var frequency = this.temperament.PitchFrequencies[pitch];

            this.wavWriter.WriteSample(frequency,
                                       1000);

            this.wavWriter.UpdateHeader();

            this.player.Play(this.wavWriter.Stream);
        }


        [TestMethod]
        public void PlayerPlaysSequence()
        {
            var pitches = new[]
                          {
                              new Pitch(Note.C,4),
                              new Pitch(Note.D,4),
                              new Pitch(Note.E,4),
                              new Pitch(Note.F,4),
                              new Pitch(Note.G,4),
                              new Pitch(Note.A,4),
                              new Pitch(Note.B,4),
                              new Pitch(Note.C,5),
                          };

            var frequencies = pitches.Select(pitch => this.temperament.PitchFrequencies[pitch])
                                     .ToArray();

            foreach (var frequency in frequencies)
            {
                this.wavWriter.WriteSample(frequency,
                                           500);
            }

            this.wavWriter.UpdateHeader();

            this.player.Play(this.wavWriter.Stream);
        }

        [TestMethod]
        public void PlayerPlaysOscillation()
        {
            for(int i = 440; i < 660; i+=5)
            {
                this.wavWriter.WriteSample(i,
                                           100);
            }

            this.wavWriter.UpdateHeader();

            this.player.Play(this.wavWriter.Stream);
        }

        [TestMethod]
        public void PlayerPlaysTriad()
        {
            var pitches = new Pitch(Note.C,
                                    4);
            var frequencies = this.temperament.GetTriadFrequencies(pitches,
                                                                   TriadCharacter.Major);
            this.wavWriter.WriteSample(frequencies,
                                       1000);
            this.wavWriter.UpdateHeader();
            this.player.Play(this.wavWriter.Stream);
        }

        [TestMethod]
        public void PlayerPlaysSeventhChord()
        {
            var pitches = new Pitch(Note.C,
                                    4);
            var frequencies = this.temperament.GetSeventhChordFrequencies(pitches,
                                                                          SeventhChordCharacter.Diminished);
            this.wavWriter.WriteSample(frequencies,
                                       2000);
            this.wavWriter.UpdateHeader();
            this.player.Play(this.wavWriter.Stream);
        }
    }
}
