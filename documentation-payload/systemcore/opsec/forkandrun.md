+++
title = "Fork and Run Commands"
chapter = false
weight = 102
+++

## What is Fork and Run?

"Fork and Run" is an agent architecture that spawns sacrificial processes in a suspended state to inject shellcode into.

## Fork and Run in SystemCore

SystemCore uses the fork and run architecture for a variety of jobs. These jobs will all first spawn a new process specified by the [`spawnto_x86`](/agents/SystemCore/commands/spawnto_x86) or [`spawnto_x64`](/agents/SystemCore/commands/spawnto_x64) commands. The parent process of these new processes is specified by the [`ppid`](/agents/SystemCore/commands/ppid/) command. Once the process is spawned, SystemCore will use the currently set injection technique to inject into the remote process.

The following commands use the fork and run architecture:

- [`execute_assembly`](/agents/SystemCore/commands/execute_assembly/)
- [`mimikatz`](/agents/SystemCore/commands/mimikatz/)
- [`powerpick`](/agents/SystemCore/commands/powerpick/)
- [`printspoofer`](/agents/SystemCore/commands/printspoofer/)
- [`pth`](/agents/SystemCore/commands/pth/)
- [`dcsync`](/agents/SystemCore/commands/pth/)
- [`spawn`](/agents/SystemCore/commands/spawn/)
- [`execute_pe`](/agents/SystemCore/commands/execute_pe/)