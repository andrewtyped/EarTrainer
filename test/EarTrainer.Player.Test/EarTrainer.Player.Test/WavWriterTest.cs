using Microsoft.VisualStudio.TestTools.UnitTesting;
using EarTrainer.Player;

namespace EarTrainer.Player.Test
{
    [TestClass]
    public class WavWriterTest
    {
        private WavWriter wavWriter;

        [TestInitialize]
        public void TestInitialize()
        {
            this.wavWriter = new WavWriter();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.wavWriter.Dispose();
        }

        [TestMethod]
        public void WavWriterWritesSamples()
        {
            this.wavWriter.WriteSample(440,
                                       1000);

            var currentPosition = this.wavWriter.Stream.Position;

            this.wavWriter.UpdateHeader();


            Assert.AreEqual(currentPosition,
                            this.wavWriter.Stream.Position);

        }
    }
}
