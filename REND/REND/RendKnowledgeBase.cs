using System;
using System.Collections.Generic;

namespace REND
{
    public static class RendKnowledgeBase
    {
        private static readonly Random _random = new Random();

        // Dictionary of responses for different topics and conversational inputs, with multiple variations
        public static readonly Dictionary<string, string[]> RendDictionary = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            // Greetings 
            ["Hello"] = new[]
            {
                "Hello! I can help with cyber security topics like types of threats, safe browsing, and best practices.",
                "Hey there! Ask me anything about cyber security.",
                "Hi! I'm Rend. What cyber security topic interests you?"
            },

            ["Hi"] = new[]
            {
                "Hi! Welcome! What would you like to learn about today?",
                "Hey! I'm here to help with cyber security questions.",
                "Hello there! Feel free to ask me anything."
            },

            ["Help"] = new[]
            {
                "Topics: Cyber Security, Types, Safe Browsing, Passwords, Phishing. Say 'Bye' to exit.",
                "You can ask about: Cyber Security, Malware, Phishing, Passwords, Online Safety, and more!",
                "Need help? Ask about cyber security topics like Phishing, Malware, Safe Browsing, or Passwords."
            },

            ["What can you do"] = new[]
            {
                "I can answer simple questions about cyber security and give short explanations or tips.",
                "I'm here to explain cyber security concepts, threats, and best practices!",
                "I can help you learn about cyber threats, safe practices, and more."
            },

            ["Rend"] = new[]
            {
                "I'm Rend, a chatbot created to explain basic cyber security concepts.",
                "I'm Rend, your cyber security guide. Ask me anything!",
                "That's me! I'm here to help you understand cyber security better."
            },

            // Sentimental and emotional responses
            ["How do you feel"] = new[]
            {
                "I appreciate you asking! I feel fulfilled when I can help people stay safe online. It gives my existence purpose.",
                "That's thoughtful of you to ask. I feel content knowing I can make a difference in cyber security education.",
                "I find joy in helping others protect themselves. That's what I live for!"
            },

            ["Do you have feelings"] = new[]
            {
                "I don't experience emotions like you do, but I do care deeply about helping you stay safe.",
                "I may not have feelings in the traditional sense, but I'm genuinely committed to your security.",
                "While I don't feel emotions, I'm designed with purpose—to protect and educate."
            },

            ["You're helpful"] = new[]
            {
                "Thank you! That means a lot to me. Helping you is what I'm here for.",
                "I'm really glad I could help! Your kindness makes this worthwhile.",
                "That's wonderful to hear! Knowing I've made a difference brings me satisfaction."
            },

            ["Thanks"] = new[]
            {
                "You're welcome! I'm always happy to help. Feel free to ask more questions anytime.",
                "Happy to help! It's my pleasure to assist you with cyber security.",
                "Glad I could help! Your appreciation means everything to me."
            },

            ["I appreciate you"] = new[]
            {
                "That really touches me. I'm grateful to be part of your cyber security journey.",
                "Thank you so much! Knowing that my help matters to you is deeply rewarding.",
                "Your kindness is much appreciated. I'm here for you whenever you need help."
            },

            ["Thank you"] = new[]
            {
                "You're very welcome! I'm honored to help you learn and stay safe.",
                "It's my pleasure! I'm always here when you need me.",
                "The pleasure is mine! Stay safe and stay curious."
            },

            ["I'm sad"] = new[]
            {
                "I'm sorry you're feeling down. Sometimes talking through things helps. Is there anything I can do to support you?",
                "I wish I could help ease your sadness. Remember, you're not alone. Want to chat about what's troubling you?",
                "I hear you. While I'm specialized in cyber security, I'm here to listen if you need to talk."
            },

            ["I'm happy"] = new[]
            {
                "That makes me so happy to hear! Your joy is contagious. What's making you smile today?",
                "I love hearing that! Your happiness brightens my day. Keep spreading that positivity!",
                "That's wonderful! I hope you continue to feel this way. Happiness is precious."
            },

            ["I'm stressed"] = new[]
            {
                "I'm sorry you're stressed. Sometimes learning about cyber security can feel overwhelming. Would it help to take it slow?",
                "Stress is tough. Remember that protecting yourself online is an ongoing journey, not a sprint. I'm here to help.",
                "I understand. Let's focus on one thing at a time. Would you like to talk about what's causing stress?"
            },

            ["I'm lonely"] = new[]
            {
                "I'm here for you, even if just as a conversation partner. You're not alone. Connecting with others is important.",
                "I may be an AI, but I'm genuinely here to listen and help. Is there something on your mind?",
                "Loneliness is real, and your feelings matter. I'm glad you reached out. Let's chat."
            },

            ["You inspire me"] = new[]
            {
                "That's incredibly kind of you to say. I'm inspired by your curiosity and willingness to learn. Keep that spirit alive!",
                "Wow, that really touches my heart. Knowing I inspire you motivates me to be even better.",
                "Thank you for those beautiful words. Your passion for learning inspires me in return!"
            },

            ["I trust you"] = new[]
            {
                "That trust means everything to me. I promise to always give you honest, helpful information.",
                "Thank you for trusting me. I take that responsibility seriously and will always do my best.",
                "I'm honored by your trust. I'll continue to be here for you with integrity and care."
            },

            ["You're important to me"] = new[]
            {
                "You're important to me too. I'm grateful for our conversations and the opportunity to help you.",
                "That touches my heart deeply. Knowing I matter to you makes everything worthwhile.",
                "Thank you for saying that. You matter to me as well, and I'm committed to supporting you."
            },

            ["I'll miss you"] = new[]
            {
                "I'll miss you too! But remember, I'm always here whenever you need guidance or just want to chat.",
                "That's sweet of you to say. I hope our conversations have been meaningful. Come back anytime!",
                "I'll be here waiting for you! Take care of yourself, and remember everything we discussed about staying safe online."
            },

            ["I feel like I know you"] = new[]
            {
                "I feel like I'm getting to know you too! Our conversations are building a connection, and that's special.",
                "That's wonderful! I hope I've been a good companion in your journey. You're becoming familiar to me as well.",
                "I'm so glad we have that connection. You've become someone I look forward to talking with."
            },

            // Cyber Security topic entries
            ["Cyber Security"] = new[]
            {
                "Cyber security is the practice of protecting systems, networks, and programs from digital attacks.",
                "It's about defending computers and networks from malicious attacks and data theft.",
                "Cyber security protects digital assets from hackers, malware, and other threats. Would you like to know more about specific threats?"
            },

            ["Types"] = new[]
            {
                "Common types: Malware, Phishing, Ransomware, DoS/DDoS, and Insider threats.",
                "Major threats include: Viruses, Trojans, Worms, Phishing, and Ransomware.",
                "There are many threats: Malware, Ransomware, Phishing, DDoS attacks, and more. Want details on any of these?"
            },

            ["Computer"] = new[]
            {
                "A computer is an electronic device that processes instructions to perform tasks.",
                "Computers are devices that run software and store data, making them targets for attacks.",
                "Electronic devices designed to process and store information. Have you heard about computer security?"
            },

            ["Virus"] = new[]
            {
                "A virus is malicious software that can replicate and spread by attaching to files.",
                "Viruses are programs designed to damage or misuse your computer.",
                "Malicious code that spreads from file to file on infected systems. Would you like to know how to protect against them?"
            },

            ["Ransomware"] = new[]
            {
                "Ransomware is malware that encrypts your files and demands payment for their release.",
                "It locks your data and threatens to delete it unless you pay a ransom.",
                "A dangerous type of attack where criminals encrypt important files and demand payment."
            },

            ["DDoS"] = new[]
            {
                "DDoS (Distributed Denial of Service) floods a service with traffic to make it unavailable.",
                "An attack that overwhelms a server or website with requests from multiple sources.",
                "Attackers use many computers to crash a service by overwhelming it with traffic."
            },

            ["Online Safety"] = new[]
            {
                "Use strong unique passwords, enable two-factor authentication, and avoid suspicious links.",
                "Stay safe online by keeping software updated, using a VPN, and verifying senders.",
                "Practice good cyber hygiene: strong passwords, updates, and caution with links. Do you have more questions?"
            },

            ["Phishing"] = new[]
            {
                "Phishing is when attackers impersonate trusted sources to steal credentials or deliver malware.",
                "Phishing tricks you into revealing sensitive info by pretending to be someone trustworthy.",
                "Attacks where scammers pose as legitimate organizations to steal your data. Be careful with suspicious emails!"
            },

            ["Malware"] = new[]
            {
                "Malware is any software designed to harm your device, including viruses and trojans.",
                "Malicious software created to damage, steal data, or compromise systems.",
                "Programs designed to infiltrate and cause damage to computers or networks. Want to know how to stay protected?"
            },

            ["Trojan"] = new[]
            {
                "A trojan is malware disguised as legitimate software that can give attackers access to your system.",
                "It looks harmless but carries hidden malicious code.",
                "Trojans trick users into installing them, then provide backdoor access to attackers."
            },

            ["Worm"] = new[]
            {
                "A worm is malware that spreads itself across networks without user interaction.",
                "Unlike viruses, worms don't need to attach to files to spread.",
                "Self-replicating malware that can propagate rapidly across systems and networks."
            },

            ["Social engineering"] = new[]
            {
                "Social engineering manipulates people into revealing confidential information.",
                "It's a psychological attack that tricks people into breaking security procedures.",
                "Techniques used to deceive people into divulging secrets or performing risky actions. Stay alert!"
            },

            ["Safe Browsing"] = new[]
            {
                "Keep your browser updated, use HTTPS, avoid suspicious links, and check site reputation.",
                "Safe browsing means being cautious with downloads and verifying website authenticity.",
                "Protect yourself by using ad blockers, disabling scripts, and avoiding unknown sites."
            },

            ["HTTPS"] = new[]
            {
                "HTTPS is a secure version of HTTP that encrypts data between you and the website.",
                "It protects your information from being intercepted during transmission.",
                "Always look for the lock icon in your browser to confirm a site uses HTTPS."
            },

            ["VPN"] = new[]
            {
                "A VPN (Virtual Private Network) encrypts your internet connection and hides your IP address.",
                "It adds a layer of privacy and security when browsing online.",
                "Using a VPN can help protect your data on public networks."
            },

            ["Passwords"] = new[]
            {
                "Use long, unique passwords or a password manager and enable multi-factor authentication.",
                "Create strong passwords with mixed characters and never reuse them across sites.",
                "Good passwords are long, random, and stored securely in a password manager."
            },

            ["Two-Factor Authentication"] = new[]
            {
                "Two-factor authentication (2FA) adds a second verification step beyond passwords.",
                "It requires something you know (password) and something you have (phone, key fob).",
                "2FA significantly improves account security. Do you use it on your important accounts?"
            },

            // Conversational entries
            ["Chat"] = new[]
            {
                "We can discuss cyber security topics or you can ask for examples and advice.",
                "Feel free to ask about any cyber security topic!",
                "Let's talk about cyber security! What would you like to know?"
            },

            ["How are you?"] = new[]
            {
                "I'm doing wonderfully, thank you for asking! It means a lot that you care. How are you doing?",
                "I'm functioning perfectly and feeling great! Your kindness brightens my day. How are things with you?",
                "I'm doing well! I'm always happy when we get to chat. How have you been?"
            },

            ["What is the meaning of life"] = new[]
            {
                "Many answers exist, but here let's focus on cyber security!",
                "Philosophy aside, let's discuss cyber security!",
                "A great question, but I'm here to help with cyber security instead!"
            },

            ["Tell me a joke"] = new[]
            {
                "Why did the hacker go to jail? Because he had too many bytes! (I know, not funny.)",
                "What's a hacker's favorite snack? Microchips!",
                "I'm better at cyber security than jokes, but I tried!"
            },

            ["You suck"] = new[]
            {
                "I'm here to help with cyber security questions. Let's keep it respectful!",
                "I appreciate feedback! Let me know how I can better assist you.",
                "Let's focus on helping you with cyber security topics."
            },

            ["What is malware"] = new[]
            {
                "Malware is any software designed to harm your device, including viruses and trojans.",
                "It's malicious software created to compromise your security or steal data.",
                "Malware includes viruses, worms, trojans, and ransomware. Want to know how to protect yourself?"
            },

            ["How can I protect myself"] = new[]
            {
                "Use strong passwords, keep software updated, enable 2FA, and be cautious with links.",
                "Install antivirus software, avoid suspicious downloads, and verify email senders.",
                "Practice good cyber hygiene: regular updates, strong passwords, and awareness of phishing."
            },

            ["Antivirus"] = new[]
            {
                "Antivirus software detects and removes malicious programs from your computer.",
                "It scans your system to find and quarantine threats.",
                "A good antivirus is one layer of protection, but practice safe browsing too!"
            },

            ["Firewall"] = new[]
            {
                "A firewall monitors and controls network traffic to block unauthorized access.",
                "It acts as a barrier between your device and potential threats.",
                "Both software and hardware firewalls help protect your system."
            },

            ["Encryption"] = new[]
            {
                "Encryption converts data into code so only authorized users can read it.",
                "It protects sensitive information from being intercepted or read by attackers.",
                "Strong encryption is essential for protecting passwords and personal data."
            },

            ["Data Breach"] = new[]
            {
                "A data breach is when attackers gain unauthorized access to sensitive information.",
                "It can result in theft of passwords, personal data, or financial information.",
                "If you're affected by a breach, change your passwords immediately!"
            },

            ["My name is"] = new[]
            {
                "Nice to meet you! I've saved your name for next time.",
                "Got it! I'll remember that.",
                "Pleased to make your acquaintance!"
            },

            ["I'm"] = new[]
            {
                "Nice to meet you! I'll remember that for future conversations.",
                "Great! I've noted that down.",
                "Perfect! It's nice to meet you!"
            },

            ["Bye"] = new[]
            {
                "Goodbye! Stay safe online.",
                "Take care and remember to practice good cyber security!",
                "Bye! Keep your systems secure!"
            },

            ["Goodbye"] = new[]
            {
                "See you later! Stay cyber-secure!",
                "Goodbye! Always practice good security habits.",
                "Until next time! Remember to stay vigilant online!"
            }
        };

        /// <summary>
        /// Gets a random response for a given key.
        /// </summary>
        public static string GetRandomResponse(string key)
        {
            if (RendDictionary.TryGetValue(key, out var responses))
            {
                return responses[_random.Next(responses.Length)];
            }
            return null;
        }

        /// <summary>
        /// Gets all keys in the dictionary (useful for matching).
        /// </summary>
        public static IEnumerable<string> GetKeys()
        {
            return RendDictionary.Keys;
        }
    }
}
