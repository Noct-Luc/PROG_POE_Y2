using System;
using System.Collections.Generic;

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
            @"",
            @"      REND - Cyber Security Guide",
            @"",
            @"      Created by: Someone who dislikes making chatbots",
            @""
        };

        Console.ForegroundColor = ConsoleColor.Cyan;
        foreach (var line in header)
        {
            Console.WriteLine(line);
        }
        Console.ResetColor();

        // Use the RendWriteLine helper so all Rend output is cyan
        RendWriteLine("Hello, I'm Rend, your cyber security guide. Ask me anything about cyber security and I'll do my best to help you out. If you want to end the conversation just say 'Bye'");

        var rendDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Hello", "Hi, how are you? That question was an obligation, I dont really care about how you are doing so ask me stuff about cyber security" },
            { "Rend", "I'm Rend, my name was given to me by my creator because he dislikes making chat bots. What this has to do with my purpose, I have no clue. But you can be sure that I'll try my best to help you" },
            { "What can you do?", "I can help you with a variety of tasks, such as answering questions, providing information, and assisting with various topics regarding cyber security. Just ask me anything that I know." },
            { "What is your purpose?", "My purpose is to satisfy the requirements of the piece of paper that is used to grade my creator and be your cyber security guide but the former takes precedent" },

            { "Cyber Security", "Cyber security is the practice of protecting computer systems, networks, and data from unauthorized access, attacks, and damage. It involves implementing measures to safeguard against cyber threats and ensure the confidentiality, integrity, and availability of information." },
            {"Types", "There are many threats to your information, but i know some stuff about malware, virusses, Online safety, Phishing and social engineering "  },
            { "Computer", "A computer is an electronic device that processes data and performs tasks according to instructions provided by software." },
            { "Virus", "A computer virus is a type of malicious software that attaches to files or programs and spreads to other systems causing damage or disruption." },
            { "Online Safety", "Always create strong passwords that are difficult to guess, ideally combining letters, numbers, and special characters." },
            { "Phishing", "Phishing is a type of cyberattack where attackers impersonate trusted sources to trick people into revealing sensitive information or installing malware." },
            { "Malware", "Malware is malicious software designed to harm, disrupt, or gain unauthorized access to computers, networks, or devices." },
            { "Social engineering", "Social engineering is the practice of manipulating human psychology to gain unauthorized access to information, systems, or valuables." },
            {"Chat","What you wanna talk about? I must needs remind you that I'm not fully sentient and just lines of dialouge written in a dictionary also praise my creator's writing skills, he actually cares aabout what i sound like. Enough boasting let's talk about cyber security because your feelings are not important to me" },
            { "Help", "You can ask me about \"Cyber Security\", \"Types of threats\", or say \"Hello and ask me my purpose\". If you want to end the conversation just say 'Bye'" },

            { "Bye", "Ciao... Please give my creator a passing grade, he tried to give me personality" }
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

            // Try exact match (case-insensitive because of dictionary comparer)
            if (rendDictionary.TryGetValue(input, out var response))
            {
                RendWriteLine(response);
                continue;
            }

            // Fallback: try to find any key appearing inside the input (simple substring match)
            var foundKey = default(string);
            foreach (var key in rendDictionary.Keys)
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
                RendWriteLine("I'm not sure I understand. Try asking about \"Cyber Security\", \"Types of threats\", or say \"Hello and ask me my purpose\".");
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
