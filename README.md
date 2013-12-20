SkypeHistEncrypter
==================

Selective History Encrypter for Skype

==================

It's Windows only for now, but should be easy to update for other platforms if the Skype client uses the same SQLite database, I believe only the paths would be different.

Should be fully functional and void of most bugs, so give it a try.

If you want to test it safely with a copy of your data, I suggest making a copy of your Skype user profile folder (%APPDATA%\Skype\%SkypeUser%\) into a new same-level folder (so in %APPDATA%\Skype\), and using that copied folder name as your username in the initial login screen.

It was finished in almost one session (just a short nap before the end), so please don't judge the code. :)

Done in C# with .NET Framework 3.5, but you can try compiling it with Mono and SharpDevelop if you don't have Windows.