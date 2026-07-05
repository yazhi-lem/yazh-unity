// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// Yazhi Agent - Conversational Interface
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

class YazhAgent {
    constructor() {
        this.chatMessages = document.getElementById('chatMessages');
        this.chatInput = document.getElementById('chatInput');
        this.sendBtn = document.getElementById('sendBtn');
        this.messageQueue = [];
        this.isProcessing = false;
        
        // Knowledge base
        this.knowledge = {
            adhan: {
                name: 'Adhan',
                emoji: '🧠',
                description: 'Tamil Language Model. Our sovereign alternative to OpenAI/GPT. 30K tokens, trained on Tamil corpus.',
                status: 'Training',
                progress: '60%'
            },
            yazh: {
                name: 'Yazh',
                emoji: '🎮',
                description: 'XR Pet Companion for kids 6–14. Tamil-first, on-device AI, Barracuda ONNX runtime.',
                status: 'Production',
                progress: '12-week timeline active'
            },
            capitol: {
                name: 'Capitol',
                emoji: '🔐',
                description: 'Identity & Mesh Commons. OAuth layer, P2P mesh authentication, sovereignty-first.',
                status: 'Core',
                progress: 'Deployed on Zorba'
            },
            lion: {
                name: 'Lion',
                emoji: '📡',
                description: 'Mesh Communications Layer. Sovereign, encrypted, no cloud dependency.',
                status: 'Prototype',
                progress: 'Integration phase'
            }
        };
        
        this.conversationContext = [];
        this.initEventListeners();
        this.welcomeMessage();
    }
    
    initEventListeners() {
        this.sendBtn.addEventListener('click', () => this.sendMessage());
        this.chatInput.addEventListener('keydown', (e) => {
            if (e.key === 'Enter' && !e.shiftKey) {
                e.preventDefault();
                this.sendMessage();
            }
        });
    }
    
    welcomeMessage() {
        const greeting = 'Namaskar. I am Yazhi — built for sovereignty, run on our Zorba. What shall we build today?\n\nAsk about Landscapes (Adhan, Yazh, Capitol, Lion), Stories, or next steps.';
        this.addMessage(greeting, 'bot', true);
    }
    
    sendMessage() {
        const text = this.chatInput.value.trim();
        if (!text) return;
        
        this.addMessage(text, 'user');
        this.chatInput.value = '';
        this.chatInput.focus();
        
        this.isProcessing = true;
        this.sendBtn.disabled = true;
        
        // Simulate agent thinking
        setTimeout(() => this.generateResponse(text), 400);
    }
    
    generateResponse(userInput) {
        const lower = userInput.toLowerCase();
        let response = '';
        
        // Landscape queries
        if (lower.includes('adhan')) {
            response = this.getLandscapeInfo('adhan');
        } else if (lower.includes('yazh')) {
            response = this.getLandscapeInfo('yazh');
        } else if (lower.includes('capitol')) {
            response = this.getLandscapeInfo('capitol');
        } else if (lower.includes('lion')) {
            response = this.getLandscapeInfo('lion');
        } 
        // Story / status queries
        else if (lower.includes('story') || lower.includes('status') || lower.includes('update')) {
            response = this.getStatusUpdate();
        }
        // Mission / next steps
        else if (lower.includes('mission') || lower.includes('next') || lower.includes('task') || lower.includes('build')) {
            response = this.getMissionBrief();
        }
        // General sovereign ethos
        else if (lower.includes('sovereign') || lower.includes('privacy') || lower.includes('p2p') || lower.includes('cloud')) {
            response = this.getSovereigntyPrinciples();
        }
        // Help
        else if (lower.includes('help') || lower.includes('what can')) {
            response = this.getHelp();
        }
        // Default
        else {
            response = this.getDefault(userInput);
        }
        
        this.addMessage(response, 'bot');
        this.isProcessing = false;
        this.sendBtn.disabled = false;
    }
    
    getLandscapeInfo(landscape) {
        const info = this.knowledge[landscape];
        return `${info.emoji} **${info.name}**\n\n${info.description}\n\nStatus: ${info.status}\nProgress: ${info.progress}`;
    }
    
    getStatusUpdate() {
        return `📊 **Latest Updates**\n\n✓ Jun 13 — Yazh production pipeline optimized. WebGL dashboard live.\n\n✓ Jun 12 — Adhan inference engine tuned for RPi5. Barracuda runtime integrated.\n\n✓ Jun 11 — Capitol mesh auth deployed on Zorba. iOS simulator testing complete.\n\n✓ Jun 10 — Yazh production spec finalized: 12 weeks, 8 roles, 66 assets.\n\nAll landscapes are moving at velocity.`;
    }
    
    getMissionBrief() {
        return `🎯 **Current Mission Brief**\n\n**Phase 1 (Weeks 1–4):** Stabilize Yazh production. Get alpha build onto iOS/Android test devices. Finalize Adhan 30K inference pipeline.\n\n**Phase 2 (Weeks 5–8):** Launch Capitol identity layer. Integrate Lion mesh comms. Begin Yazh multiplayer testing.\n\n**Phase 3 (Weeks 9–12):** Market prep. App Store submission. Community onboarding.\n\n**Immediate:** Build momentum. Deploy this dashboard. Iterate hard. No cloud, no compromise.`;
    }
    
    getSovereigntyPrinciples() {
        return `🛡️ **Yazhi Sovereignty Ethos**\n\n**Zero-Cloud:** All processing on-device or P2P mesh. No central servers. No surveillance.\n\n**Tamil-First:** Adhan speaks Tamil. Yazh dreams Tamil. We build for our mothers, not for Silicon Valley.\n\n**Open Source:** Code lives in public. Meshes are transparent. We trust protocol, not corporation.\n\n**On-Device AI:** 30K model runs on RPi5 and mobile. No latency, no dependencies, no data leak.\n\n**P2P Mesh:** Capitol and Lion enable direct human-to-human connection. No intermediaries.\n\nThis is the Yazhi way.`;
    }
    
    getHelp() {
        return `ℹ️ **How to use this dashboard:**\n\n• Ask about **Landscapes** — Adhan, Yazh, Capitol, Lion\n• Request **Stories** — see recent updates and milestones\n• Check **Mission** — next priorities and timeline\n• Discuss **Sovereignty** — our ethos, principles, vision\n• Type anything else — I'll help however I can\n\nThis dashboard runs on Yazhi infrastructure. Pure. Fast. Sovereign.`;
    }
    
    getDefault(input) {
        const responses = [
            `That's an interesting question. Let me think... 🤔 We're building Yazhi as a sovereign alternative to centralized systems. Every line of code, every design decision is aligned with that mission. What would you like to explore?`,
            `I like your thinking. The Yazhi ecosystem connects four key landscapes: Adhan (intelligence), Yazh (experience), Capitol (identity), and Lion (connection). They form a mesh — each stronger with the others. Which one calls to you?`,
            `Good energy. We're moving at high velocity right now. The production timeline is packed, but every sprint gets us closer to an actual sovereign alternative. What's your role in this?`,
            `That resonates. Yazhi is built on principles: sovereignty, Tamil-first, P2P, and zero-cloud. If you want to dive deep into any of those, I'm here.`
        ];
        return responses[Math.floor(Math.random() * responses.length)];
    }
    
    addMessage(text, role, isInitial = false) {
        const messageEl = document.createElement('div');
        messageEl.className = `message ${role}`;
        
        const contentEl = document.createElement('div');
        contentEl.className = 'message-content';
        
        // Simple markdown-like formatting
        const formatted = text
            .replace(/\*\*(.*?)\*\*/g, '<strong>$1</strong>')
            .replace(/__(.*?)__/g, '<em>$1</em>')
            .replace(/\n/g, '<br>');
        
        contentEl.innerHTML = formatted;
        messageEl.appendChild(contentEl);
        this.chatMessages.appendChild(messageEl);
        
        // Auto-scroll to bottom
        setTimeout(() => {
            this.chatMessages.parentElement.scrollTop = this.chatMessages.parentElement.scrollHeight;
        }, 50);
    }
}

// Initialize on DOM ready
document.addEventListener('DOMContentLoaded', () => {
    const agent = new YazhAgent();
});
