using System;
using System.IO;
using System.Media;

public static class VoiceGreeting
{
    private static SoundPlayer _player;

    
    public static bool Greet(string FFVII, bool block = false)
    {
        try
        {
            string path = Path.IsPathRooted(FFVII)
                ? FFVII 
                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "C: \Users\Student\Source\Repos\PROG_POE_Y2\Assets\FFVII - fanfare.wav", FFVII);

            if (!File.Exists(path))
            {
                Console.Error.WriteLine($"VoiceGreeting: audio file not found: {path}");
                return false;
            }

            if (block)
            {
                using (var player = new SoundPlayer(path))
                {
                    player.Load();
                    player.PlaySync(); // blocking
                }

                return true;
            }

            // Non-blocking playback: keep player alive so audio continues
            _player = new SoundPlayer(path);
            _player.LoadAsync();
            _player.Play();
            return true;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"VoiceGreeting: failed to play audio: {ex.Message}");
            return false;
        }
    }

    public static void Stop()
    {
        try
        {
            _player?.Stop();
        }
        catch { /* ignore */ }
    }
}
