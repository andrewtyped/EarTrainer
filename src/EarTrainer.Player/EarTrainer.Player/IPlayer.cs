using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarTrainer.Player
{
    public interface IPlayer: IDisposable
    {
        void Play(Stream stream);
    }
}
