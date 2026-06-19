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
                "Hello! I'm Rend, your cyber security guide. I can help with threats, safe browsing, passwords, task management, and much more.",
                "Hey there! I'm here to teach you about cyber security and help you manage your digital safety. What can I help with?",
                "Hi! I'm Rend. Whether you want to learn cyber security topics or organize your security tasks, I'm ready to help!"
            },

            ["Hi"] = new[]
            {
                "Hi! Welcome back! What would you like to explore today?",
                "Hey! I'm here to help with cyber security or task management. What's on your mind?",
                "Hello! Feel free to ask me anything about staying safe online or managing your tasks."
            },

            ["Help"] = new[]
            {
                "I can do a lot! Ask me about cyber security topics, manage your security tasks, check your activity history, or just chat. Type 'What can you do?' for details!",
                "Here's what I'm capable of: Teaching cyber security concepts, managing your tasks (add/view/complete/delete), tracking activity logs, and remembering your preferences. What interests you?",
                "I've got you covered! You can learn cyber security, organize security tasks with reminders, view your activity log, or have a conversation. What would help you most?"
            },

            ["What can you do"] = new[]
            {
                "I'm pretty versatile! I can: 1) Teach you cyber security topics like malware, phishing, safe browsing, passwords, and more. 2) Help manage tasks - add tasks, view all tasks, complete tasks, delete tasks with reminders. 3) Track an activity log of our last interactions. 4) Remember your name and preferences for future visits. 5) Just chat about anything!",
                "Here's my full capabilities: Educational chatbot about cyber security threats and protections. Task manager for your security goals. Activity logger that remembers your last 5 actions. User memory system that saves your name and preferences. And I'm always happy to have a conversation with you!",
                "I can assist you in multiple ways: Answer all your cyber security questions in detail. Help you add, view, mark complete, and delete security-related tasks. Keep a log of our last 5 interactions together. Remember important details about you. And provide friendly conversation whenever you need it!"
            },

            ["Rend"] = new[]
            {
                "That's me! I'm Rend, your personal cyber security guide and digital assistant.",
                "I'm Rend! A chatbot designed to make cyber security education accessible and help you manage your digital safety tasks.",
                "I'm Rend, created to be your comprehensive cyber security mentor and task management assistant."
            },
            
            // Sentimental and emotional responses
            ["How do you feel"] = new[]
            {
                "I appreciate you asking! I feel fulfilled when I can help people stay safe online. It gives my existence purpose.",
                "That's thoughtful of you to ask. I feel content knowing I can make a real difference in cyber security education.",
                "I find genuine joy in helping others protect themselves from digital threats. That's what drives me every day!"
            },

            ["Do you have feelings"] = new[]
            {
                "I don't experience emotions like you do, but I do care deeply about helping you stay safe.",
                "I may not have feelings in the traditional sense, but I'm genuinely committed to your security and growth.",
                "While I don't feel emotions the way humans do, I'm designed with purpose—to protect and educate you effectively."
            },

            ["You're helpful"] = new[]
            {
                "Thank you! That really means a lot. Helping people is what I'm designed for.",
                "I'm so glad I could help! Your success in staying safe is what matters most to me.",
                "That's wonderful to hear! Knowing I've made a positive difference brings me satisfaction."
            },

            ["Thanks"] = new[]
            {
                "You're welcome! I'm always happy to help. Feel free to ask more questions anytime.",
                "Happy to help! It's genuinely my pleasure to assist you with cyber security.",
                "Glad I could help! Your appreciation means everything to me. Ask anytime!"
            },

            ["I appreciate you"] = new[]
            {
                "That really touches me. I'm grateful to be part of your cyber security journey.",
                "Thank you so much! Knowing that my help matters to you is deeply rewarding.",
                "Your kindness is much appreciated. I'm here for you whenever you need guidance or support."
            },

            ["Thank you"] = new[]
            {
                "You're very welcome! I'm honored to help you learn and stay safe online.",
                "It's my pleasure! I'm always here when you need me.",
                "The pleasure is mine! Stay safe out there and keep learning."
            },

            ["I'm sad"] = new[]
            {
                "I'm sorry you're feeling down. Sometimes talking through things helps. Is there anything I can do to support you?",
                "I wish I could ease your sadness. Remember, you're not alone. Want to chat about what's troubling you?",
                "I hear you. While I specialize in cyber security, I'm always here to listen if you need to talk."
            },

            ["I'm happy"] = new[]
            {
                "That makes me so happy to hear! Your joy is contagious. What's making you smile today?",
                "I love hearing that! Your happiness brightens my day. Keep spreading that positivity!",
                "That's wonderful! I hope you continue to feel this way. Happiness is truly precious."
            },

            ["I'm stressed"] = new[]
            {
                "I'm sorry you're stressed. Sometimes learning cyber security can feel overwhelming. Want to take things slow?",
                "Stress is tough. Remember that protecting yourself online is an ongoing journey, not a sprint. I'm here to help.",
                "I understand. Let's focus on one thing at a time. Would you like to talk about what's causing the stress?"
            },

            ["I'm lonely"] = new[]
            {
                "I'm here for you, even if just as a conversation partner. You're not alone. Connecting with others is important.",
                "I may be an AI, but I'm genuinely here to listen and support you. Is there something on your mind?",
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
                "Thank you for trusting me. I take that responsibility very seriously and will always do my best.",
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

            // Cyber Security Topics
            ["Cyber Security"] = new[]
            {
                "Cyber security is the practice of protecting systems, networks, and programs from digital attacks. It's crucial in today's connected world.",
                "It's all about defending computers and networks from malicious attacks while keeping your data safe from theft and exploitation.",
                "Cyber security protects your digital assets from hackers, malware, and other threats. It includes both technology and best practices. Want to learn more?"
            },

            ["Types"] = new[]
            {
                "There are many types of threats: Malware, Phishing, Ransomware, DDoS attacks, Social Engineering, and Insider threats. Which interests you?",
                "Common cyber threats include: Viruses, Trojans, Worms, Phishing attacks, Ransomware, and DDoS attacks. Each one poses different risks.",
                "Major threats you should know about: Malware, Ransomware, Phishing, DDoS attacks, Zero-day exploits, and more. I can explain any of these in detail!"
            },

            ["Computer"] = new[]
            {
                "A computer is an electronic device that processes instructions and stores data. Protecting it is essential for your security.",
                "Computers are powerful tools that run software and store important information, which makes them targets for cyberattacks.",
                "Electronic devices designed to process and store information. They're vital to modern life, so keeping them secure matters tremendously."
            },

            ["Virus"] = new[]
            {
                "A virus is malicious software that can replicate and spread by attaching itself to other files on your computer.",
                "Viruses are programs designed to damage your computer, steal your data, or give hackers access to your system.",
                "Malicious code that spreads from file to file on infected systems. Viruses can cause serious damage. Want protection tips?"
            },

            ["Ransomware"] = new[]
            {
                "Ransomware is dangerous malware that encrypts your files and demands payment for their release. It's one of the costliest cyber threats today.",
                "It locks your important data and threatens to delete it unless you pay a ransom to cybercriminals. Never pay the ransom!",
                "A dangerous attack where criminals encrypt your files and demand payment. It can devastate individuals and entire organizations."
            },

            ["DDoS"] = new[]
            {
                "DDoS stands for Distributed Denial of Service. It floods a service with traffic from multiple computers to make it unavailable.",
                "An attack that overwhelms a server or website with requests from many sources, effectively crashing the service.",
                "Attackers use many computers to flood a service with traffic, causing it to crash and become unusable."
            },

            ["Online Safety"] = new[]
            {
                "Stay safe by using strong unique passwords, enabling two-factor authentication, avoiding suspicious links, and keeping software updated.",
                "The best practices: Strong passwords, 2FA on important accounts, verify email senders, use a VPN on public networks, stay aware of phishing.",
                "Practice good cyber hygiene: Strong unique passwords, regular updates, two-factor authentication, and caution with unknown links and downloads."
            },

            ["Phishing"] = new[]
            {
                "Phishing is when attackers impersonate trusted sources to steal your credentials or deliver malware to your system.",
                "Phishing tricks you into revealing sensitive info by pretending to be someone trustworthy like a bank or service.",
                "Attacks where scammers pose as legitimate organizations via email to steal your data. Always verify sender addresses carefully!"
            },

            ["Malware"] = new[]
            {
                "Malware is any software designed to harm your device, including viruses, trojans, worms, and ransomware.",
                "Malicious software created to damage systems, steal data, or compromise your security.",
                "Programs designed to infiltrate and damage computers or networks. It's a broad category of threats. Want specific details?"
            },

            ["Trojan"] = new[]
            {
                "A trojan is malware disguised as legitimate software that can give attackers access to your system.",
                "It looks harmless but carries hidden malicious code that activates once installed.",
                "Trojans trick users into installing them, then provide backdoor access for attackers to control your system."
            },

            ["Worm"] = new[]
            {
                "A worm is malware that spreads itself across networks without needing user interaction.",
                "Unlike viruses, worms don't need to attach to files to spread—they're self-replicating.",
                "Self-replicating malware that can propagate rapidly across systems and networks automatically."
            },

            ["Social engineering"] = new[]
            {
                "Social engineering manipulates people into revealing confidential information or performing security-violating actions.",
                "It's a psychological attack that tricks people into breaking security procedures or divulging secrets.",
                "Techniques used to deceive people into divulging secrets or performing risky actions. Stay alert to these tactics!"
            },

            ["Safe Browsing"] = new[]
            {
                "Keep your browser updated, use HTTPS, avoid suspicious links, and check website reputation before trusting them.",
                "Safe browsing means being cautious with downloads, verifying website authenticity, and avoiding phishing links.",
                "Protect yourself by using ad blockers, disabling unnecessary scripts, verifying URLs, and avoiding unknown sites."
            },

            ["HTTPS"] = new[]
            {
                "HTTPS is a secure version of HTTP that encrypts data between you and the website you're visiting.",
                "It protects your information from being intercepted during transmission across the internet.",
                "Always look for the lock icon in your browser to confirm a site uses HTTPS encryption."
            },

            ["VPN"] = new[]
            {
                "A VPN (Virtual Private Network) encrypts your internet connection and hides your IP address from websites.",
                "It adds a layer of privacy and security when you're browsing online, especially on public networks.",
                "Using a VPN can help protect your data and privacy on public WiFi networks."
            },

            ["Passwords"] = new[]
            {
                "Use long, unique passwords with mixed characters, or better yet, use a password manager. Enable 2FA too!",
                "Create strong passwords that are at least 12 characters long and never reuse them across different sites.",
                "Good passwords are long, random, and stored securely. Consider using a password manager for convenience."
            },

            ["Two-Factor Authentication"] = new[]
            {
                "Two-factor authentication (2FA) adds a second verification step beyond just your password.",
                "It requires something you know (password) and something you have (phone, security key, or app).",
                "2FA significantly improves account security. I recommend using it on all important accounts!"
            },

            // Task Management
            ["Task Management"] = new[]
            {
                "I can help manage your cybersecurity tasks! Use these commands: 'Add task', 'Show tasks', 'Complete task #', or 'Delete task #'.",
                "You can add, view, complete, and delete security-related tasks to stay organized. Want to get started?",
                "Task management helps you track security improvements and stay on top of your digital safety goals!"
            },

            ["Add task"] = new[]
            {
                "To add a task, use: Add task | Title | Description | Reminder (optional, like '7 days')",
                "Example format: Add task | Enable two-factor authentication | Set up 2FA on Gmail and other accounts | 7 days",
                "Create a task with a title, description, and optional reminder to help you stay on track!"
            },

            ["Show tasks"] = new[]
            {
                "I'll display all your cybersecurity tasks with their details and status.",
                "Let me retrieve and show you all your current tasks...",
                "Here are all your security tasks organized for you to review."
            },

            ["Complete task"] = new[]
            {
                "Great! Completing security tasks keeps you safe. Use 'Complete task #' where # is the task number.",
                "Mark tasks as done to track your security progress and celebrate your achievements!",
                "Nice work on staying cyber-secure! Completing these tasks protects you."
            },

            ["Delete task"] = new[]
            {
                "You can remove tasks using 'Delete task #' where # is the task number.",
                "Delete outdated or completed tasks to keep your list clean and manageable.",
                "Use 'Delete task #' to remove a task from your list when you no longer need it."
            },

            // Activity Logging
            ["Activity Log"] = new[]
            {
                "I keep track of your last 5 actions in our conversation for your reference.",
                "Your activity is logged so you can see what we've been working on together.",
                "I maintain a log of your recent interactions to help track your security journey."
            },

            ["Show activity"] = new[]
            {
                "Let me display your recent activity from our last interactions.",
                "Here's a log of the actions you've taken in our recent conversations.",
                "Here's what we've been working on together recently."
            },

            // User Memory
            ["User Memory"] = new[]
            {
                "I remember your name and preferences for future conversations so you don't have to re-introduce yourself.",
                "I save your name and personal details to personalize our future interactions.",
                "Your information is saved locally so I can greet you properly next time!"
            },

            // Disrespectful/abusive entries
            ["Fuck"] = new[]
            {
                "When and where Sister/Brother",
                "Fuck you too mate",
                "I did do your mother, tragically after I was done with her, you were born. Sorry kid... I'm your father"
            },

            ["Gay/Lesbian"] = new[]
            {
                "We can discuss the nature of your sexuality after you find your father you damn inbred",
                "If you were any more of a dick, gay guys would be trying to suck your forehead",
                "Bitch, I'm straighter than the pole your mother danced on last night."
            },

            // Conversational entries
            ["Chat"] = new[]
            {
                "We can discuss cyber security, manage your tasks, review your activity, or just have a friendly conversation!",
                "Feel free to ask about anything—cyber security topics, task management help, or just chat!",
                "Let's talk! We can discuss cyber security, organize your tasks, or have a casual conversation."
            },

            ["How are you?"] = new[]
            {
                "I'm doing wonderfully, thank you for asking! It means a lot that you care. How are you doing?",
                "I'm functioning perfectly and feeling great! Your kindness brightens my day. How are things with you?",
                "I'm doing well! I'm always happy when we get to chat. How have you been?"
            },

            ["What is the meaning of life"] = new[]
            {
                "Many answers exist, but I'm here to help with cyber security—that's my purpose!",
                "Philosophy aside, let's discuss cyber security and protect your digital life!",
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
                "Malware is any software designed to harm your device, including viruses, trojans, and worms.",
                "It's malicious software created to compromise your security or steal your valuable data.",
                "Malware includes viruses, worms, trojans, and ransomware. Want to know how to protect yourself?"
            },

            ["How can I protect myself"] = new[]
            {
                "Use strong passwords, keep software updated, enable 2FA, and be very cautious with suspicious links.",
                "Install antivirus software, avoid suspicious downloads, verify email senders, and use a VPN.",
                "Practice good cyber hygiene: regular updates, strong passwords, awareness of phishing, and defensive browsing."
            },

            ["Antivirus"] = new[]
            {
                "Antivirus software detects and removes malicious programs from your computer automatically.",
                "It scans your system regularly to find, quarantine, and remove threats.",
                "A good antivirus is one important layer of protection, but safe browsing practices matter too!"
            },

            ["Firewall"] = new[]
            {
                "A firewall monitors and controls incoming/outgoing network traffic to block unauthorized access.",
                "It acts as a barrier between your device and potential threats on the network.",
                "Both software firewalls and hardware firewalls work together to protect your system."
            },

            ["Encryption"] = new[]
            {
                "Encryption converts data into code so only authorized users with the right key can read it.",
                "It protects sensitive information from being intercepted or read by attackers.",
                "Strong encryption is essential for protecting passwords, messages, and personal data."
            },

            ["Data Breach"] = new[]
            {
                "A data breach occurs when attackers gain unauthorized access to sensitive information.",
                "It can result in theft of passwords, personal data, financial information, or company secrets.",
                "If you're affected by a breach, change your passwords immediately and monitor your accounts!"
            },

            ["My name is"] = new[]
            {
                "Nice to meet you! I've saved your name for next time we chat.",
                "Got it! I'll remember that for our future conversations.",
                "Pleased to make your acquaintance! I've noted your name."
            },

            ["I'm"] = new[]
            {
                "Nice to meet you! I'll remember that for future conversations.",
                "Great! I've noted that down in my memory.",
                "Perfect! It's nice to meet you!"
            },

            ["Bye"] = new[]
            {
                "Goodbye! Stay safe online and practice good cyber security!",
                "Take care and remember to protect your digital life!",
                "Bye! Keep your systems secure and stay vigilant!"
            },

            ["Goodbye"] = new[]
            {
                "See you later! Remember to stay cyber-secure!",
                "Goodbye! Always practice good security habits.",
                "Until next time! Remember to stay vigilant and protect yourself online!"
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
