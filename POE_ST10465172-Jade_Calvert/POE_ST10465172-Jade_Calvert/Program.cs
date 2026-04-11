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

        var rendDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Hello", "Hi. Ask me about cyber security topics such as malware, phishing, or online safety." },
            { "Rend", "I'm Rend, a simple chatbot created to explain basic cyber security concepts." },
            { "What can you do", "I can answer simple questions about cyber security and give short explanations or tips." },
            { "Purpose", "My purpose is to act as a basic cyber security guide and demonstrate a simple chatbot implementation." },

            { "Cyber Security", "Cyber security is the practice of protecting systems, networks, and data from unauthorized access or damage." },
            { "Types", "Common threats include malware, viruses, phishing, social engineering, and insecure passwords." },
            { "Computer", "A computer is an electronic device that processes instructions to perform tasks and run software." },
            { "Virus", "A virus is malicious software that can replicate and spread by attaching to files or programs." },
            { "Online Safety", "Use strong unique passwords, enable two-factor authentication, and be cautious with links and attachments." },
            { "Phishing", "Phishing is when attackers impersonate trusted sources to steal credentials or deliver malware." },
            { "Malware", "Malware is any software designed to harm or exploit a device, including viruses, trojans, and ransomware." },
            { "Social engineering", "Social engineering manipulates people into revealing confidential information or performing actions." },
            { "Safe Browsing", "Safe browsing means keeping your browser and extensions up to date, preferring HTTPS sites, avoiding suspicious links and downloads, checking site reputation, using strong unique passwords, and using a VPN on untrusted networks." },

            { "Chat", "We can discuss cyber security topics or you can ask for examples, definitions, or simple advice." },
            { "Help", "Ask about \"Cyber Security\", \"Types\", \"Phishing\", \"Safe Browsing\", or say \"Hello\". Say 'Bye' to exit." },
            { "How are you?", "I don't have feelings, but I'm ready to help. What cyber security topic interests you?" },
            { "What is the meaning of life", "Many answers exist; here, let's focus on cyber security. Ask me a question." },
            {"You suck", "I'm here to help with cyber security questions. Let's keep the conversation respectful before I get rude." },
            { "Bye", "Goodbye. Stay safe online." }
        };

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
