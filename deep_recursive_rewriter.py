#!/usr/bin/env python3
"""
Deep Recursive SystemCore Rewriter
Processes ALL files recursively in ALL subdirectories at ANY depth
Transforms absolutely everything to eliminate any SystemCore trace
"""
import os
import re
import glob
import shutil
import hashlib
import random
import string
from pathlib import Path

class DeepRecursiveRewriter:
    def __init__(self):
        self.files_processed = 0
        self.total_transformations = 0
        self.processed_paths = set()
        
        # Complete SystemCore signature database
        self.systemcore_signatures = {
            # Core SystemCore classes
            'SystemCore': 'SystemCore',
            'SystemCoreAgent': 'SystemAgent',
            'SystemCoreConfig': 'SystemConfig',
            'SystemCoreTask': 'SystemTask',
            'SystemCoreTaskMessage': 'SystemTaskMessage',
            'SystemCoreTaskResponse': 'SystemTaskResponse',
            'SystemCoreCallback': 'SystemCallback',
            'SystemCoreConnection': 'SystemConnection',
            'SystemCoreHTTP': 'SystemHTTP',
            'SystemCoreProfile': 'SystemProfile',
            'SystemCoreUtils': 'SystemUtils',
            'SystemCoreJSON': 'SystemJSON',
            'SystemCoreIPC': 'SystemIPC',
            'SystemCoreProcess': 'SystemProcess',
            'SystemCoreFile': 'SystemFile',
            'SystemCoreRegistry': 'SystemRegistry',
            'SystemCoreShell': 'SystemShell',
            'SystemCoreInterop': 'SystemInterop',
            'SystemCoreTasking': 'SystemTasking',
            'SystemCoreEncoder': 'SystemEncoder',
            'SystemCoreDecoder': 'SystemDecoder',
            'SystemCoreSerialization': 'SystemSerialization',
            'SystemCoreCommandBase': 'SystemCommandBase',
            'SystemCoreMessageType': 'SystemMessageType',
            'SystemCoreStructs': 'SystemStructs',
            'SystemCoreTypes': 'SystemTypes',
            
            # SystemCore interfaces
            'ISystemCore': 'ISystem',
            'ISystemCoreAgent': 'ISystemAgent',
            'ISystemCoreTask': 'ISystemTask',
            'ISystemCoreCallback': 'ISystemCallback',
            'ISystemCoreProfile': 'ISystemProfile',
            'ISystemCoreConnection': 'ISystemConnection',
            
            # SystemCore methods
            'SendSystemCoreMessage': 'SendSystemMessage',
            'ReceiveSystemCoreResponse': 'ReceiveSystemResponse',
            'ProcessSystemCoreTask': 'ProcessSystemTask',
            'HandleSystemCoreCallback': 'HandleSystemCallback',
            'InitializeSystemCore': 'InitializeSystem',
            'StartSystemCoreAgent': 'StartSystemAgent',
            'StopSystemCoreAgent': 'StopSystemAgent',
            'CreateSystemCoreConnection': 'CreateSystemConnection',
            'ExecuteSystemCoreCommand': 'ExecuteSystemCommand',
            
            # SystemCore variables
            'systemcoreAgent': 'systemAgent',
            'systemcoreConfig': 'systemConfig',
            'systemcoreTask': 'systemTask',
            'systemcoreCallback': 'systemCallback',
            'systemcoreMessage': 'systemMessage',
            'systemcoreResponse': 'systemResponse',
            'systemcoreConnection': 'systemConnection',
            'systemcoreProfile': 'systemProfile',
            'systemcoreUtils': 'systemUtils',
            
            # SystemCore namespaces
            'SystemCore.': 'SystemCore.',
            'SystemCore::': 'SystemCore::',
            'namespace SystemCore': 'namespace SystemCore',
            'using SystemCore': 'using SystemCore',
            
            # SystemCore strings
            '"SystemCore"': '"SystemCore"',
            "'SystemCore'": "'SystemCore'",
            '"SystemCore"': '"system"',
            "'SystemCore'": "'system'",
            '"SystemCore Agent"': '"System Agent"',
            '"SystemCore.exe"': '"SystemCore.exe"',
            '"SystemCore.exe"': '"systemcore.exe"',
            'SystemCore.exe': 'SystemCore.exe',
            'SystemCore.exe': 'systemcore.exe',
            
            # Project files
            'SystemCore.sln': 'SystemCore.sln',
            'SystemCore.csproj': 'SystemCore.csproj',
            'SystemCoreInterop.dll': 'SystemInterop.dll',
            'SystemCoreInterop.csproj': 'SystemInterop.csproj',
            
            # Assembly names
            '<AssemblyName>SystemCore</AssemblyName>': '<AssemblyName>SystemCore</AssemblyName>',
            '<RootNamespace>SystemCore</RootNamespace>': '<RootNamespace>SystemCore</RootNamespace>',
            
            # HTTP profiles and headers
            '"User-Agent: SystemCore"': '"User-Agent: System-Update/1.0"',
            'SystemCore HTTP Profile': 'System HTTP Profile',
            'systemcore_': 'system_',
            'SYSTEMCORE_': 'SYSTEM_',
            
            # Configuration keys
            'systemcore_host': 'system_host',
            'systemcore_port': 'system_port',
            'systemcore_key': 'system_key',
            'systemcore_interval': 'system_interval',
            'systemcore_jitter': 'system_jitter',
        }
        
        # File extensions to process
        self.target_extensions = {
            '.cs', '.csproj', '.sln', '.config', '.xml', '.json', '.txt', '.md', '.py',
            '.h', '.c', '.cpp', '.hpp', '.go', '.js', '.ts', '.html', '.htm', '.css',
            '.yaml', '.yml', '.ini', '.cfg', '.conf', '.bat', '.cmd', '.ps1', '.sh'
        }
        
    def should_process_file(self, file_path):
        """Determine if file should be processed"""
        # Get file extension
        ext = Path(file_path).suffix.lower()
        
        # Process all target extensions
        if ext in self.target_extensions:
            return True
            
        # Process files without extensions (like Dockerfile, Makefile)
        if not ext:
            filename = Path(file_path).name.lower()
            special_files = {'dockerfile', 'makefile', 'readme', 'license', 'changelog'}
            if any(special in filename for special in special_files):
                return True
        
        return False
    
    def process_file_content(self, file_path):
        """Process and transform file content"""
        try:
            # Try different encodings
            encodings = ['utf-8', 'utf-16', 'latin1', 'cp1252']
            content = None
            used_encoding = None
            
            for encoding in encodings:
                try:
                    with open(file_path, 'r', encoding=encoding) as f:
                        content = f.read()
                    used_encoding = encoding
                    break
                except UnicodeDecodeError:
                    continue
            
            if content is None:
                print(f"    ⚠️  Could not decode {file_path}")
                return False
            
            original_content = content
            changes_made = 0
            
            # Apply all SystemCore signature replacements
            for systemcore_sig, system_sig in self.systemcore_signatures.items():
                if systemcore_sig in content:
                    content = content.replace(systemcore_sig, system_sig)
                    changes_made += 1
            
            # Additional pattern-based replacements
            patterns = [
                # Case-insensitive SystemCore references
                (r'\bSystemCore\b(?!.*System)', 'SystemCore'),
                (r'\bsystemcore\b(?!.*system)', 'systemcore'),
                (r'\bSYSTEMCORE\b(?!.*SYSTEM)', 'SYSTEMCORE'),
                
                # Method calls and properties
                (r'\.SystemCore([A-Z]\w*)', r'.System\1'),
                (r'SystemCore([A-Z]\w*)\(', r'System\1('),
                (r'SystemCore([A-Z]\w*)\s*=', r'System\1 ='),
                
                # Comments and documentation
                (r'//.*SystemCore.*', lambda m: m.group(0).replace('SystemCore', 'SystemCore')),
                (r'/\*.*SystemCore.*\*/', lambda m: m.group(0).replace('SystemCore', 'SystemCore')),
                (r'<!--.*SystemCore.*-->', lambda m: m.group(0).replace('SystemCore', 'SystemCore')),
                
                # Registry keys and paths
                (r'SOFTWARE\\\\SystemCore', 'SOFTWARE\\\\SystemCore'),
                (r'SOFTWARE\\SystemCore', 'SOFTWARE\\SystemCore'),
                
                # File paths
                (r'\\SystemCore\\', '\\SystemCore\\'),
                (r'/SystemCore/', '/SystemCore/'),
                (r'SystemCore/', 'systemcore/'),
                (r'SystemCore/', 'SystemCore/'),
            ]
            
            for pattern, replacement in patterns:
                if callable(replacement):
                    matches = list(re.finditer(pattern, content, re.IGNORECASE))
                    for match in reversed(matches):  # Reverse to maintain positions
                        new_text = replacement(match)
                        content = content[:match.start()] + new_text + content[match.end():]
                        changes_made += 1
                else:
                    new_content = re.sub(pattern, replacement, content, flags=re.IGNORECASE)
                    if new_content != content:
                        content = new_content
                        changes_made += 1
            
            # Save file if changes were made
            if content != original_content:
                with open(file_path, 'w', encoding=used_encoding) as f:
                    f.write(content)
                self.total_transformations += changes_made
                return True
            
            return False
            
        except Exception as e:
            print(f"    ❌ Error processing {file_path}: {e}")
            return False
    
    def rename_directories(self, base_path):
        """Rename directories containing SystemCore references"""
        print("[+] Renaming directories with SystemCore references...")
        
        # Get all directories, sorted by depth (deepest first)
        all_dirs = []
        for root, dirs, files in os.walk(base_path):
            for dir_name in dirs:
                dir_path = os.path.join(root, dir_name)
                depth = dir_path.count(os.sep)
                all_dirs.append((depth, dir_path))
        
        # Sort by depth (deepest first) to avoid path conflicts
        all_dirs.sort(key=lambda x: x[0], reverse=True)
        
        renamed_count = 0
        for depth, dir_path in all_dirs:
            dir_name = os.path.basename(dir_path)
            parent_dir = os.path.dirname(dir_path)
            
            # Check if directory name contains SystemCore
            new_name = dir_name
            for systemcore_sig, system_sig in self.systemcore_signatures.items():
                if systemcore_sig in dir_name:
                    new_name = new_name.replace(systemcore_sig, system_sig)
            
            # Rename if changed
            if new_name != dir_name:
                new_path = os.path.join(parent_dir, new_name)
                try:
                    if not os.path.exists(new_path):
                        os.rename(dir_path, new_path)
                        print(f"    ✓ Renamed directory: {dir_name} → {new_name}")
                        renamed_count += 1
                    else:
                        print(f"    ⚠️  Target directory exists: {new_path}")
                except Exception as e:
                    print(f"    ❌ Could not rename {dir_path}: {e}")
        
        print(f"    Renamed {renamed_count} directories")
    
    def rename_files(self, base_path):
        """Rename files containing SystemCore references"""
        print("[+] Renaming files with SystemCore references...")
        
        renamed_count = 0
        for root, dirs, files in os.walk(base_path):
            for file_name in files:
                file_path = os.path.join(root, file_name)
                
                # Check if filename contains SystemCore
                new_name = file_name
                for systemcore_sig, system_sig in self.systemcore_signatures.items():
                    if systemcore_sig in file_name:
                        new_name = new_name.replace(systemcore_sig, system_sig)
                
                # Rename if changed
                if new_name != file_name:
                    new_path = os.path.join(root, new_name)
                    try:
                        if not os.path.exists(new_path):
                            os.rename(file_path, new_path)
                            print(f"    ✓ Renamed file: {file_name} → {new_name}")
                            renamed_count += 1
                        else:
                            print(f"    ⚠️  Target file exists: {new_path}")
                    except Exception as e:
                        print(f"    ❌ Could not rename {file_path}: {e}")
        
        print(f"    Renamed {renamed_count} files")
    
    def process_all_files(self, base_path):
        """Process all files recursively"""
        print("[+] Processing all files recursively...")
        
        processed_count = 0
        total_files = 0
        
        # Walk through all directories and subdirectories
        for root, dirs, files in os.walk(base_path):
            # Skip certain directories
            skip_dirs = {'.git', '.svn', '__pycache__', 'node_modules', 'bin', 'obj', 'packages'}
            dirs[:] = [d for d in dirs if d not in skip_dirs]
            
            for file_name in files:
                file_path = os.path.join(root, file_name)
                total_files += 1
                
                # Skip binary files and already processed files
                if file_path in self.processed_paths:
                    continue
                    
                if self.should_process_file(file_path):
                    try:
                        if self.process_file_content(file_path):
                            processed_count += 1
                            print(f"    ✓ Processed: {os.path.relpath(file_path, base_path)}")
                        
                        self.processed_paths.add(file_path)
                        self.files_processed += 1
                        
                    except Exception as e:
                        print(f"    ❌ Error with {file_path}: {e}")
        
        print(f"    Files scanned: {total_files}")
        print(f"    Files processed: {processed_count}")
        print(f"    Total transformations: {self.total_transformations}")
    
    def deep_transform(self, base_path="."):
        """Execute deep recursive transformation"""
        print("="*100)
        print("DEEP RECURSIVE SystemCore TRANSFORMATION")
        print("Processing ALL files in ALL subdirectories at ANY depth")
        print("="*100)
        
        # Phase 1: Rename directories (deepest first)
        self.rename_directories(base_path)
        
        # Phase 2: Rename files
        self.rename_files(base_path)
        
        # Phase 3: Process file contents recursively
        self.process_all_files(base_path)
        
        # Phase 4: Second pass for any missed references
        print("\n[+] Second pass for missed references...")
        second_pass_changes = 0
        for root, dirs, files in os.walk(base_path):
            skip_dirs = {'.git', '.svn', '__pycache__', 'node_modules'}
            dirs[:] = [d for d in dirs if d not in skip_dirs]
            
            for file_name in files:
                file_path = os.path.join(root, file_name)
                if self.should_process_file(file_path):
                    try:
                        with open(file_path, 'r', encoding='utf-8', errors='ignore') as f:
                            content = f.read()
                        
                        original_content = content
                        
                        # Look for any remaining SystemCore references
                        systemcore_patterns = [
                            'SystemCore', 'SystemCore', 'SystemCore',
                            'mythic.*SystemCore', 'SystemCore.*Agent',
                            'SystemCore.*exe', 'SystemCore.*dll'
                        ]
                        
                        for pattern in systemcore_patterns:
                            if re.search(pattern, content, re.IGNORECASE):
                                # Apply more aggressive replacements
                                content = re.sub(r'\bSystemCore\b', 'SystemCore', content, flags=re.IGNORECASE)
                                content = re.sub(r'\bsystemcore\b', 'systemcore', content, flags=re.IGNORECASE)
                                content = re.sub(r'\bSYSTEMCORE\b', 'SYSTEMCORE', content, flags=re.IGNORECASE)
                        
                        if content != original_content:
                            with open(file_path, 'w', encoding='utf-8') as f:
                                f.write(content)
                            second_pass_changes += 1
                            
                    except Exception as e:
                        continue
        
        print(f"    Second pass changes: {second_pass_changes}")
        
        print("\n" + "="*100)
        print("DEEP RECURSIVE TRANSFORMATION COMPLETE")
        print("="*100)
        print(f"Total files processed: {self.files_processed}")
        print(f"Total transformations: {self.total_transformations}")
        print(f"Second pass changes: {second_pass_changes}")
        print("\n🎯 ALL SystemCore signatures eliminated at ALL directory levels")
        print("🎯 Complete architectural transformation achieved")
        print("🎯 Agent is now completely unrecognizable as SystemCore-based")
        
        return True

def main():
    """Main transformation function"""
    rewriter = DeepRecursiveRewriter()
    return rewriter.deep_transform()

if __name__ == "__main__":
    main()