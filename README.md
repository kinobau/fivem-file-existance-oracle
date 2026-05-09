# fivem-file-existence-oracle

  A file-existence side channel in FiveM's C# / Mono sandbox via `Assembly.LoadFrom` exception types.

  This method was originally discovered/found and made by me.

  ## Background

  FiveM client-side C# resources run in a custom Mono with CoreCLR Security Transparency Level 2 + IL Verification. The
  standard file I/O surface is locked down:

  - `System.IO.File.Exists` / `ReadAllBytes` / `ReadAllText` — callable but virtualized (always return `false` / throw
  `FileNotFoundException` for real paths)
  - `System.Diagnostics.Process`, `Marshal.*`, `Microsoft.Win32.Registry`, `System.Net.Sockets`, P/Invoke — blocked or
  stripped
  - Most of `System.Environment` (`UserName`, `MachineName`, etc.) — stripped from the Mono build entirely

  A malicious resource has no obvious way to ask *"does file X exist on this user's machine?"*.

  ## The leak

  `System.Reflection.Assembly.LoadFrom` is **not** virtualized. The exception type leaks file presence:

  - Returns `Assembly` — file exists, is a valid managed assembly
  - `BadImageFormatException` — file exists, not a managed assembly
  - `FileLoadException` — file exists, couldn't load (locked, ACL'd, already loaded)
  - `FileNotFoundException` — file does not exist *(also: directories, and active-user profile-root files like
  `NTUSER.DAT`)*
