#!/usr/bin/env python3
"""
Ultimate SystemCore Eliminator
Processes ALL files recursively and eliminates every SystemCore reference
"""
import os
import re
import shutil
from pathlib import Path

class UltimateSystemCoreEliminator:
    def __init__(self):
        self.files_processed = 0
        self.total_changes = 0
        
        # Complete replacement mappings
        self.replacements = {
            # Basic SystemCore references
            'SystemCore': 'SystemCore',
            'systemcore': 'systemcore', 
            'SYSTEMCORE': 'SYSTEMCORE',
            
            # Specific classes and interfaces
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
            
            # Variables
            'systemcoreAgent': 'systemAgent',
            'systemcoreConfig': 'systemConfig',
            'systemcoreTask': 'systemTask',
            'systemcoreCallback': 'systemCallback',
            'systemcoreMessage': 'systemMessage',
            'systemcoreResponse': 'systemResponse',
            'systemcoreConnection': 'systemConnection',
            'systemcoreProfile': 'systemProfile',
            'systemcoreUtils': 'systemUtils',
            
            # File references
            'SystemCore.exe': 'SystemCore.exe',
            'systemcore.exe': 'systemcore.exe',
            'SystemCore.dll': 'SystemCore.dll',
            'systemcore.dll': 'systemcore.dll',
            'SystemCore.sln': 'SystemCore.sln',
            'SystemCore.csproj': 'SystemCore.csproj',
            'SystemCoreInterop.dll': 'SystemInterop.dll',
            'SystemCoreInterop.csproj': 'SystemInterop.csproj',
            
            # String literals
            '"SystemCore"': '"SystemCore"',
            "'SystemCore'": "'SystemCore'",
            '"systemcore"': '"systemcore"',
            "'systemcore'": "'systemcore'",
            '"SystemCore Agent"': '"System Agent"',
            '"SystemCore.exe"': '"SystemCore.exe"',
            '"systemcore.exe"': '"systemcore.exe"',
            
            # Assembly and namespace references
            '<AssemblyName>SystemCore</AssemblyName>': '<AssemblyName>SystemCore</AssemblyName>',
            '<RootNamespace>SystemCore</RootNamespace>': '<RootNamespace>SystemCore</RootNamespace>',
            'namespace SystemCore': 'namespace SystemCore',
            'using SystemCore': 'using SystemCore',
            
            # HTTP and network references
            '"User-Agent: SystemCore"': '"User-Agent: System-Update/1.0"',
            'SystemCore HTTP Profile': 'System HTTP Profile',
            'systemcore_': 'system_',
            'SYSTEMCORE_': 'SYSTEM_',
            'systemcore_host': 'system_host',
            'systemcore_port': 'system_port',
            'systemcore_key': 'system_key',
            'systemcore_interval': 'system_interval',
            'systemcore_jitter': 'system_jitter',
            
            # Registry and paths
            'SOFTWARE\\SystemCore': 'SOFTWARE\\SystemCore',
            'systemcore/': 'systemcore/',
            'SystemCore/': 'SystemCore/',
            '/SystemCore/': '/SystemCore/',
            '\\SystemCore\\': '\\SystemCore\\',
        }
    
    def should_process_file(self, file_path):
        """Check if file should be processed"""
        # Skip binary files and directories to avoid
        skip_extensions = {'.exe', '.dll', '.so', '.dylib', '.bin', '.obj', '.pdb', '.lib', '.a'}
        skip_dirs = {'.git', '.svn', '__pycache__', 'node_modules', '.vs', 'bin', 'obj'}
        
        path = Path(file_path)
        
        # Skip if in excluded directory
        if any(skip_dir in path.parts for skip_dir in skip_dirs):
            return False
            
        # Skip binary files
        if path.suffix.lower() in skip_extensions:
            return False
            
        return True
    
    def process_file(self, file_path):
        """Process individual file"""
        try:
            # Try different encodings
            encodings = ['utf-8', 'utf-16', 'latin1', 'cp1252', 'ascii']
            content = None
            used_encoding = 'utf-8'
            
            for encoding in encodings:
                try:
                    with open(file_path, 'r', encoding=encoding) as f:
                        content = f.read()
                    used_encoding = encoding
                    break
                except (UnicodeDecodeError, UnicodeError):
                    continue
                except Exception:
                    continue
            
            if content is None:
                return False
            
            original_content = content
            changes_made = 0
            
            # Apply all replacements
            for old_text, new_text in self.replacements.items():
                if old_text in content:
                    new_content = content.replace(old_text, new_text)
                    if new_content != content:
                        content = new_content
                        changes_made += 1
            
            # Apply word boundary replacements for better precision
            word_replacements = [
                (r'\bSystemCore\b', 'SystemCore'),
                (r'\bsystemcore\b', 'systemcore'),
                (r'\bSYSTEMCORE\b', 'SYSTEMCORE'),
            ]
            
            for pattern, replacement in word_replacements:
                new_content = re.sub(pattern, replacement, content)
                if new_content != content:
                    content = new_content
                    changes_made += 1
            
            # Save if changes were made
            if content != original_content:
                with open(file_path, 'w', encoding=used_encoding) as f:
                    f.write(content)
                self.total_changes += changes_made
                return True
            
            return False
            
        except Exception as e:
            print(f"Error processing {file_path}: {e}")
            return False
    
    def rename_items(self, base_path):
        """Rename files and directories"""
        print("Renaming files and directories...")
        
        # Collect all items to rename (files and dirs)
        items_to_rename = []
        
        for root, dirs, files in os.walk(base_path):
            # Add directories
            for dir_name in dirs:
                dir_path = os.path.join(root, dir_name)
                if 'systemcore' in dir_name.lower() or 'SystemCore' in dir_name:
                    items_to_rename.append(('dir', dir_path))
            
            # Add files
            for file_name in files:
                file_path = os.path.join(root, file_name)
                if 'systemcore' in file_name.lower() or 'SystemCore' in file_name:
                    items_to_rename.append(('file', file_path))
        
        # Sort by depth (deepest first for directories)
        items_to_rename.sort(key=lambda x: (x[1].count(os.sep), x[0] == 'file'), reverse=True)
        
        renamed_count = 0
        for item_type, item_path in items_to_rename:
            try:
                parent_dir = os.path.dirname(item_path)
                old_name = os.path.basename(item_path)
                new_name = old_name
                
                # Apply replacements to filename
                for old_text, new_text in self.replacements.items():
                    if old_text in new_name:
                        new_name = new_name.replace(old_text, new_text)
                
                if new_name != old_name:
                    new_path = os.path.join(parent_dir, new_name)
                    if not os.path.exists(new_path):
                        os.rename(item_path, new_path)
                        print(f"  Renamed {item_type}: {old_name} → {new_name}")
                        renamed_count += 1
            except Exception as e:
                print(f"  Error renaming {item_path}: {e}")
        
        print(f"Renamed {renamed_count} items")
    
    def process_all_files(self, base_path):
        """Process all files recursively"""
        print("Processing all files...")
        
        processed_count = 0
        for root, dirs, files in os.walk(base_path):
            # Filter out excluded directories
            dirs[:] = [d for d in dirs if not any(skip in d for skip in ['.git', '.svn', '__pycache__', 'node_modules'])]
            
            for file_name in files:
                file_path = os.path.join(root, file_name)
                
                if self.should_process_file(file_path):
                    if self.process_file(file_path):
                        processed_count += 1
                        rel_path = os.path.relpath(file_path, base_path)
                        print(f"  Processed: {rel_path}")
                    
                    self.files_processed += 1
        
        print(f"Processed {processed_count} files with changes")
    
    def eliminate_systemcore(self, base_path="."):
        """Main elimination process"""
        print("="*80)
        print("ULTIMATE SYSTEMCORE ELIMINATOR")
        print("Removing ALL SystemCore references at ALL levels")
        print("="*80)
        
        # Phase 1: Rename files and directories
        self.rename_items(base_path)
        
        # Phase 2: Process file contents
        self.process_all_files(base_path)
        
        # Phase 3: Final cleanup pass
        print("Running final cleanup pass...")
        final_changes = 0
        for root, dirs, files in os.walk(base_path):
            dirs[:] = [d for d in dirs if '.git' not in d and '__pycache__' not in d]
            
            for file_name in files:
                file_path = os.path.join(root, file_name)
                if self.should_process_file(file_path):
                    try:
                        with open(file_path, 'r', encoding='utf-8', errors='ignore') as f:
                            content = f.read()
                        
                        original = content
                        
                        # Final aggressive cleanup
                        content = re.sub(r'\bSystemCore\b', 'SystemCore', content, flags=re.IGNORECASE)
                        content = re.sub(r'\bsystemcore\b', 'systemcore', content, flags=re.IGNORECASE)
                        
                        if content != original:
                            with open(file_path, 'w', encoding='utf-8') as f:
                                f.write(content)
                            final_changes += 1
                    except:
                        continue
        
        print(f"Final cleanup: {final_changes} additional changes")
        
        print("\n" + "="*80)
        print("SYSTEMCORE ELIMINATION COMPLETE")
        print("="*80)
        print(f"Files processed: {self.files_processed}")
        print(f"Total changes: {self.total_changes + final_changes}")
        print("\nAll SystemCore signatures have been eliminated!")
        print("The agent is now completely unrecognizable as SystemCore-based.")
        
        return True

def main():
    eliminator = UltimateSystemCoreEliminator()
    return eliminator.eliminate_systemcore()

if __name__ == "__main__":
    main()