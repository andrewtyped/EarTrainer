using System;
using System.IO;

namespace EarTrainer.Player
{
    public class WavWriter : IDisposable
    {
        #region Constants

        private const int dataChunkSizePosition = 40;

        private const int fileSizePosition = 4;

        private const double TAU = 2 * Math.PI;

        #endregion

        #region Fields

        private readonly BinaryWriter binaryWriter;

        private readonly short bitsPerSample = 16;

        private readonly int formatChunkSize = 16;

        private readonly short formatType = 1;

        private readonly short frameSize;

        private readonly int headerSize = 8;

        private readonly MemoryStream memoryStream;

        private readonly int samplesPerSecond = 44100;

        private readonly short tracks = 1;

        private readonly int waveSize = 4;

        private int totalSamples;

        #endregion

        #region Constructors

        public WavWriter()
        {
            this.memoryStream = new MemoryStream();
            this.binaryWriter = new BinaryWriter(this.memoryStream);

            //this.frameSize = (short)(this.tracks * ((this.bitsPerSample + 7) / 8));
            this.frameSize = (short)(this.tracks * (this.bitsPerSample / 8));
            int bytesPerSecond = this.samplesPerSecond * this.frameSize;
            int fileSizePlaceHolder = 0;
            int dataChunkSizePlaceholder = 0;
            this.binaryWriter.Write(0x46464952); // = encoding.GetBytes("RIFF")
            this.binaryWriter.Write(fileSizePlaceHolder);
            this.binaryWriter.Write(0x45564157); // = encoding.GetBytes("WAVE")
            this.binaryWriter.Write(0x20746D66); // = encoding.GetBytes("fmt ")
            this.binaryWriter.Write(this.formatChunkSize);
            this.binaryWriter.Write(this.formatType);
            this.binaryWriter.Write(this.tracks);
            this.binaryWriter.Write(this.samplesPerSecond);
            this.binaryWriter.Write(bytesPerSecond);
            this.binaryWriter.Write(this.frameSize);
            this.binaryWriter.Write(this.bitsPerSample);
            this.binaryWriter.Write(0x61746164); // = encoding.GetBytes("data")
            this.binaryWriter.Write(dataChunkSizePlaceholder);
        }

        #endregion

        #region Instance Properties

        public Stream Stream => this.memoryStream;

        #endregion

        #region Instance Methods

        public void Dispose()
        {
            this.memoryStream.Dispose();
            this.binaryWriter.Dispose();
        }

        public void UpdateHeader()
        {
            this.Stream.Seek(fileSizePosition,
                             SeekOrigin.Begin);

            var fileSize = this.GetFileSize();

            this.binaryWriter.Write(fileSize);

            this.Stream.Seek(dataChunkSizePosition,
                             SeekOrigin.Begin);

            var dataChunkSize = this.GetDataChunkSize();

            this.binaryWriter.Write(dataChunkSize);

            this.Stream.Seek(0,
                             SeekOrigin.End);
        }

        public void WriteSample(double frequency,
                                int msDuration,
                                short volume = 16383)
        {
            this.WriteSample(new[]
                             {
                                 frequency
                             },
                             msDuration,
                             volume);
        }

        public void WriteSample(double[] frequencies,
                                int msDuration,
                                short volume = 16383)
        {
            // 'volume' is UInt16 with range 0 thru Uint16.MaxValue ( = 65 535)
            // we need 'amp' to have the range of 0 thru Int16.MaxValue ( = 32 767)
            double maxAmp = (double)volume / 2; // so we simply set amp = volume / 2

            double[] thetas = new double[frequencies.Length];

            for (int i = 0;
                 i < frequencies.Length;
                 i++)
            {
                thetas[i] = frequencies[i] * TAU / this.samplesPerSecond;
            }

            var samples = (int)((decimal)this.samplesPerSecond * msDuration / 1000);

            //HACK: Need these attack / release functions to prevent jarring audible clicks when frequencies end / change.
            //We should allow the user to provide these functions instead of hard coding them.
            var attackMsDuration = 3;
            var attackSamples = (int)((decimal)this.samplesPerSecond * attackMsDuration / 1000);

            var releaseMsDuration = 3;
            var releaseSamples = (int)((decimal)this.samplesPerSecond * releaseMsDuration / 1000);

            for (int step = 0;
                 step < samples;
                 step++)
            {
                double sample = 0;

                for (int i = 0;
                     i < frequencies.Length;
                     i++)
                {
                    sample += Math.Sin(thetas[i] * step);
                }

                double amp = maxAmp;
                const double growthRate = 1;

                if(step < attackSamples)
                {
                    amp = ((Math.Pow(step,growthRate)) / Math.Pow(attackSamples,growthRate)) * maxAmp;
                }
                else if(step > samples - releaseSamples)
                {
                    amp = (Math.Pow(samples - step,growthRate) / Math.Pow(releaseSamples ,growthRate)) * maxAmp;
                }

                short amplifiedSample = (short)(amp * sample);
                this.binaryWriter.Write(amplifiedSample);
            }

            this.totalSamples += samples;
        }

        private int GetDataChunkSize()
        {
            return this.totalSamples * this.frameSize;
        }

        private int GetFileSize()
        {
            return this.waveSize + this.headerSize + this.formatChunkSize + this.headerSize + (this.totalSamples * this.frameSize);
        }

        #endregion
    }
}