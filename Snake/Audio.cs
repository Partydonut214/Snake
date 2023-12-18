using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Snake
{
    internal class Audio
    {
        public readonly static MediaPlayer GameOver = LoadAudio("gameOver.wav", 0.3);
        public readonly static MediaPlayer PreGame = LoadAudio("preGameMusic.wav", 0.5, true);
        public readonly static MediaPlayer BackgroundMusic = LoadAudio("gameMusic.wav", 1, true);
        public readonly static MediaPlayer CountdownBeginning = LoadAudio("countdownBeginning.wav", 0.75);
        public readonly static MediaPlayer CountdownStinger = LoadAudio("countdownStinger.wav", 0.75);
        public readonly static MediaPlayer deathSoundIntro = LoadAudio("deathSoundIntro.wav", 0.75);
        public readonly static MediaPlayer deathSoundLoop = LoadAudio("deathSoundLoop.wav", 0.75);
        public readonly static MediaPlayer deathSoundOutro = LoadAudio("deathSoundOutro.wav", 0.75);


        private static MediaPlayer LoadAudio(string filename, double volume = 1, bool repeat = false, bool autoReset = true)
        {
            MediaPlayer player = new();
            player.Open(new Uri($"Assets/{filename}", UriKind.Relative));
            player.Volume = volume;

            if (repeat)
            {
                player.MediaEnded += PlayerRepeat_MediaEnded;
            }

            if (autoReset)
            {
                player.MediaEnded += Player_MediaEnded;
            }
            return player;
        }

        public static void PlayAudio(MediaPlayer sender)
        {
            MediaPlayer m = sender;
            m.Play();
        }
        public static void StopAudio(MediaPlayer sender)
        {
            MediaPlayer m = sender;
            m.Stop();
            m.Position = new TimeSpan(0);
        }

        private static void Player_MediaEnded(object sender, EventArgs e)
        {
            MediaPlayer m = sender as MediaPlayer;
            m.Stop();
            m.Position = new TimeSpan(0);
        }

        private static void PlayerRepeat_MediaEnded(object sender, EventArgs e)
        {
            MediaPlayer m = sender as MediaPlayer;
            m.Stop();
            m.Position = new TimeSpan(0);
            m.Play();

        }
    }
}
