# Lua Handler Pattern Example

This example is to model a pattern of letting a user write a Lua handler function which can then be called by the C# program.

The default LuaCiv3.Script is hard-sandboxed to disallow file and io access, and filespace-touching constructors for the data objects has been eliminated. Hopefully this should allow untrused Lua scripts to run safely.

## Dev Notes

I wound up having to import more than I thought, so I'll be rethinking how I want to structure my projects and whether I should continue to separate to avoid unused code or to consolidate and/or proxy for ease of programming.

The SavData object has a lot of features to be added, hence a rather simplistic handeler output for now.
