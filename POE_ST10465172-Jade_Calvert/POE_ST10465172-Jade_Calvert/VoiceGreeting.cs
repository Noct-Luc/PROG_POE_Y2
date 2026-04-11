using System;
using System.IO;
using System.Media;

public static class VoiceGreeting
{
    private static SoundPlayer _player;

    // Play a WAV file. If fileName is an absolute path it will be used directly;
    // otherwise the file is looked up in <appBase>/Assets/<fileName>.
    // Set block = true to wait for playback to finish.
    public static bool Greet(string fileName, bool block = false)
    {
        try
        {
            string path = Path.IsPathRooted(fileName)
                ? fileName
                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", C: \Users\Student\Source\Repos\PROG_POE_Y2\Assets\FFVII - fanfare.wav);

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