#!/usr/bin/env python3
"""
Yazhi WebGL Dashboard Server
Lightweight HTTP server for the Yazhi web-first dashboard
"""

import http.server
import socketserver
import os
from pathlib import Path

PORT = 8080
WEB_ROOT = Path(__file__).parent

class YazhHTTPHandler(http.server.SimpleHTTPRequestHandler):
    def __init__(self, *args, **kwargs):
        super().__init__(*args, directory=str(WEB_ROOT), **kwargs)
    
    def end_headers(self):
        # Add CORS headers for development
        self.send_header('Access-Control-Allow-Origin', '*')
        self.send_header('Cache-Control', 'no-cache, no-store, must-revalidate')
        super().end_headers()

if __name__ == '__main__':
    os.chdir(WEB_ROOT)
    
    with socketserver.TCPServer(("", PORT), YazhHTTPHandler) as httpd:
        print(f"""
╔═══════════════════════════════════════════════════╗
║   Yazhi WebGL Dashboard — Web-First Build        ║
╠═══════════════════════════════════════════════════╣
║  Server:  http://127.0.0.1:{PORT}                 ║
║  Root:    {WEB_ROOT}║
║                                                   ║
║  Serving Landscapes + Stories + Agent Chat       ║
╚═══════════════════════════════════════════════════╝
""")
        print(f"Press Ctrl+C to stop.\n")
        
        try:
            httpd.serve_forever()
        except KeyboardInterrupt:
            print("\n✓ Server stopped gracefully.")
            exit(0)
