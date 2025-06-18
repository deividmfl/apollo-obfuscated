#!/usr/bin/env python3
"""
Docker Name Compatibility Fixer
Fixes all references to use lowercase 'systemcore' for Docker compatibility
"""
import os
import re
from pathlib import Path

def fix_docker_names():
    """Fix all Docker-related naming issues"""
    
    # Files that need Docker name fixes
    files_to_fix = [
        "phantom_agent_new/Payload_Type/systemcore/systemcore/mythic/agent_functions/builder.py",
        "phantom_agent_new/Payload_Type/systemcore/__init__.py",
        "phantom_agent_new/Payload_Type/systemcore/main.py"
    ]
    
    # Check each file and apply fixes
    for file_path in files_to_fix:
        if os.path.exists(file_path):
            try:
                with open(file_path, 'r', encoding='utf-8') as f:
                    content = f.read()
                
                original_content = content
                
                # Apply Docker-specific fixes
                replacements = [
                    # Make sure payload type name is lowercase for Docker
                    ('name = "SystemCore"', 'name = "systemcore"'),
                    ('name="SystemCore"', 'name="systemcore"'),
                    # Fix any remaining uppercase references in paths
                    ('SystemCore/agent_code', 'systemcore/agent_code'),
                    ('/SystemCore/', '/systemcore/'),
                    ('SystemCore\\agent_code', 'systemcore\\agent_code'),
                    ('\\SystemCore\\', '\\systemcore\\'),
                ]
                
                for old, new in replacements:
                    content = content.replace(old, new)
                
                # Save if changes were made
                if content != original_content:
                    with open(file_path, 'w', encoding='utf-8') as f:
                        f.write(content)
                    print(f"Fixed Docker names in: {file_path}")
                    
            except Exception as e:
                print(f"Error fixing {file_path}: {e}")
    
    print("Docker name compatibility fixes complete")
    print("The agent can now be built with: docker build -t ghcr.io/mythicagents/systemcore:latest .")

if __name__ == "__main__":
    fix_docker_names()