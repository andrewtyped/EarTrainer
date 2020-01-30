using System.IO;

namespace EarTrainer.Player
{
    public class SoundPlayer : IPlayer
    {
        private readonly bool synchronous;

        #region Fields

        private readonly System.Media.SoundPlayer player;

        #endregion

        #region Constructors

        public SoundPlayer(bool synchronous = false)
        {
            this.synchronous = synchronous;
            this.player = new System.Media.SoundPlayer();
        }

        #endregion

        #region Instance Methods

        public void Dispose()
        {
            this.player?.Dispose();
        }

        public void Play(Stream stream)
        {
            stream.Seek(0,
                        SeekOrigin.Begin);
            this.player.Stream = stream;

            if (this.synchronous)
            {
                this.player.PlaySync();
            }
            else
            {
                this.player.Play();
            }
        }

        #endregion
    }
}