## Dev Notes

As of this commit, I am realizing my idea to simplify things isn't going to work as imagined. I thought I wanted to encapsulate the save into a Lua table instead of globals, but I'm not sure how to pass a non-global value to Lua...without a global function to return it.

I guess that would at least allow multiple civ 3 references in the same environment, but I'm not sure how much use I have for that. ... Actually I can think of a few, so I'll probably figure that out.

I also thought if I just registered SavData as a user type that it would register the member classes. Nope, so I still have to register every user data class I wish for Lua to access.

I had also imagined a possible Lua-as-handler function pattern where I'd run a script to define a Lua function and then call it from C# with the SavData object as a parameter, but I'm not sure how to do it or if it's even possible. (Update: Yes, see https://www.moonsharp.org/tutorial2.html step 2)

I'll probably consolidate to one MoonSharp.Interpreter.Script child, and have methods and/or constructor flags for how to set up the global environment.