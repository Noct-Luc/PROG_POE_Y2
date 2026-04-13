using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        // ASCII-art header
        var header = new[]
        {
            @"  ____  _____ _   _ ____  ",
            @" |  _ \| ____| \ | |  _ \ ",
            @" | |_) |  _| |  \| | | | |",
            @" |  _ <| |___| |\  | |_| |",
            @" |_| \_\_____|_| \_|____/ ",
            "",
            @"      REND - Cyber Security Guide",
            "",
            @"      Created by: J",
            ""
        };

        Console.ForegroundColor = ConsoleColor.Cyan;
        foreach (var line in header)
        {
            Console.WriteLine(line);
        }
        Console.ResetColor();

        // Use the RendWriteLine helper so all Rend output is cyan
        RendWriteLine("Hello, I'm Rend, your cyber security guide. Ask me anything about cyber security and I'll do my best to help you. Ask for 'Help' to see topics you can ask about. Say 'Bye' to end the conversation.");

        // Load the knowledge base from the separate class
        var rendDictionary = RendKnowledgeBase.RendDictionary;

        while (true)
        {
            Console.Write("You: ");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            input = input.Trim();

            // Exit conditions
            if (input.Equals("bye", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("goodbye", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                RendWriteLine(rendDictionary["Bye"]);
                break;
            }

            // Exact match (case-insensitive because of dictionary comparer)
            if (rendDictionary.TryGetValue(input, out var response))
            {
                RendWriteLine(response);
                continue;
            }

            // Fallback: try to find any key appearing inside the input (prefer longer keys first)
            string foundKey = null;
            foreach (var key in rendDictionary.Keys.OrderByDescending(k => k.Length))
            {
                if (input.IndexOf(key, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    foundKey = key;
                    break;
                }
            }

            if (foundKey != null)
            {
                RendWriteLine(rendDictionary[foundKey]);
            }
            else
            {
                RendWriteLine("I'm not sure I understand. Try asking about \"Cyber Security\", \"Types\", \"Safe Browsing\", or say \"Hello\".");
            }
        }
    }

    // Helper to print Rend's lines in cyan and restore previous color
    private static void RendWriteLine(string message)
    {
        var previous = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Rend: " + message);
        Console.ForegroundColor = previous;
    }
}
