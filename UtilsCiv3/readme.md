## ReadCivData.UtilsCiv3 Static Methods

- `byte[] ReadFile(string pathName)` - Given a string path, returns a byte array of the file contents. It will auto-decompress Civ3-compressed files
- `byte[] Decompress(byte[] compressedBytes)` - The byte array Civ3 (Blast / PKWare DCM) decompressor
- `string GetCiv3Path()` - Reads the Windows registry for the Civ3 install location; note that Windows registry can be emulated with other platforms' CLRs; see dotnet or mono docs for details
- `string Civ3MediaPath(string relPath, string relModPath = "")` - Used for finding the appropriate art/media file path, starts by searching for the relPath under the relModPath, then "Conquests", then "civ3PTW", then the original location, and that's how Civ3 searches for media (reModPath is skipped if `""`)
