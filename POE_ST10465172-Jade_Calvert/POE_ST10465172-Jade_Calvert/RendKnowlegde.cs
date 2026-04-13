using System;
using System.Collections.Generic;

public static class RendKnowledgeBase
{
    public static readonly Dictionary<string, string> RendDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
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
        { "You suck", "I'm here to help with cyber security questions. Let's keep the conversation respectful before I get rude." },
        { "Bye", "Goodbye. Stay safe online." }
    };
}