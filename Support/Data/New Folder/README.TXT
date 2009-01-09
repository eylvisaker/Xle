
Getting Started...
------------------

  64COPY is an all-in-one file converter and DOS maintainenance package.
Its intended use is to convert the various files that exist for the C64
emulators. Files like D64, T64, LNX, ARK, ARC and SDA can be converted to
whatever format you need for your favorite emulator, or for use on the real
C64. You still need to get whatever emulator package you want in order to
use the files you have as 64COPY does not provide *any* emulator
functionality.

  Now, seeing as you have already unpacked the archive that 64COPY comes
in, all you have to do is either enter "64COPY" or "64MAIN" to get the
program started.

  The small "64COPY" is a loader, which will execute the main program
"64MAIN". The benefit to using the loader is that when you execute
operations from the 64COPY command-line, the program will be un-loaded from
memory before the operation begins, thus freeing up most of the memory.

  Using "64MAIN", you are using the program in "resident" mode, where it
will stay in memory all the time, making executing command-line operations
much more difficult.

  The only really necessary file is "64MAIN.EXE". All the other support
files will be created when either the program needs them (.EXT), or they
will be created when you exit the program.


Here are some of the functions that 64COPY handles internally...

  * Windows95 (and later versions) long filename compliant

  * Convert D64, T64, LNX, SDA, ARK, ARC, LBR, X64 to other formats (F11)

  * Create & format image files (D64/X64/T64/LNX)

  * CheckDisk (D/X64), CheckTape (T64) and CheckZip (ZipCode), to verify
    and correct errors in these archives (on ALT-F3)

  * 6502 disassembler (on SHIFT-F4)

  * D64/X64 disk editor (on ALT-F4, when in a D/X64)

  * D64/X64 directory customizer (on SHIFT-F2)

  * D64 BAM and error-byte editor

  * PETASCII to ASCII text file conversion (on SHIFT-F9)

  * DOS Text file editor (on F4)

  * DOS file viewer (on F3)

  * HEX editor (on ALT-F4)

  * DOS copy/move/delete/attribute commands

  * Fully user configurable (colors, settings)

  * Fully editable panel layout

  * Five keyboard macro strings, with 200 keystrokes each


-------------------------------------------------------------------------------

FAQ and answers
---------------

  Most of the problems that you might encounter on startup are problems I
have not been able to duplicate, but I hope I have fixed. Here are the most
common questions I have been asked, or most common problems I have had to
fix.



Q. How does the HELP work (and where is it)?

A. The HELP for 64COPY is all online. Run the program first, and press the
   F1 key to bring up the HELP window. You will either be in a topic or the
   main index. If you are in a function (converting files, text editor),
   calling HELP will bring it up for that topic. If you are sitting at the
   command-line, then HELP will come up with the index of topics. Either
   way, you can still get the index, and search for what you want from
   there.

   If the HELP file doesn't exist (64COPY.HLP) then help will not be
   available, but you will get a warning that help cannot be found, and
   where it should be. If the HELP file version is wrong, you will also get
   a warning.



Q. When I start the program, the screen saver starts up almost immediately.
   What can I do to stop this?

A. This was due to a bug in the "inactive time" calculation for when the
   screen saver will activate. If this happens (and it shouldn't anymore),
   there are several ways to prevent this...

   * Goto the configuration page (ALT-F6) called "Screen Saver" and disable
     the saver (the best method).

   * Move the mouse to the bottom right corner of the screen. This is the
     "save never" corner, and will disable the saver (not too useful if you
     want to use the mouse)



Q. When I am doing XXX operation, the program hangs. Why?

A. This is most difficult to answer as a number of factors are coming into
   play. It may be that there is a bug that you have found, and I will have
   to track down. It would be best if you can duplicate the problem, as I
   will not be able to fix it (easily) if I cannot see it happen.

   It seems that since I changed the program from a standard EXE to an
   overlayed EXE, many before unseen programming bugs becamse visible, and
   are *very* hard to find. I have tracked down many of them but, to be
   sure, some still remain.

   If you can duplicate the problem, mail me with the all the details,
   order of operation, etc. If you don't have the latest version, I would
   recommend getting that first, as I may have fixed it already (and not be
   aware of it!)



Q. When I am doing XXX operation, I see garbage on the screen, but
   everything seems to work ok. Why?

A. Once again, like the previous question, this requires that it be
   duplicatable. If it is, mail me with what you were doing, where on
   screen the corruption starts and ends, etc. Even if I cannot duplicate
   it, I can likely attempt to fix it.



Q. Are you going to support the new GCR formats coming out for C64S and
   CCS64?

A. Quite simply, no. Miha has not released the layout of his format, and I
   have only heard of the one for CCS64. In either case, after lengthy
   discussions with other users and utility authors, I have come to the
   conclusion it is not worth supporting. If you have a program which can
   only be run from a GCR image, there should be *no* reason to have to see
   it or edit the image as working with the image will likely cause
   corruption.



Q. What about supporting the "Six-pack" ZipCode format?

A. Maybe, but this format also falls into the previous category (GCR
   images) of protected software, and as far as I can see, there's no
   reason to manipulate and convert them. Also, simply because it shares
   the name ZipCode with the 4-file ZipCode format, the files are totally
   different, and would require another sub-routine to implement.



Q. What will happen if I delete the "64COPY.INI" file?

A. Nothing bad will occur. If you delete it when in the program, then the
   settings from the INI (which where read when the program started) will
   be written back out. If you wish to remove the INI to restore the
   program defaults, it should be done when not in the program.

   When you start the program up again, it will detect that the file is
   missing, and use default values. The same applies to the "64COPY.CLR"
   (colors), "64COPY.MAC" (macros) and "64COPY.MNU" (user menu). If these
   are missing, internal defaults are used.



Q. How come if I change the screen height (Alt-F9), and then quit and
   restart the program, the screen height is not what I changed it to?

A. When 64COPY is started for the *firt time, the screen height is
   recorded, and 64COPY will always start up in that mode, unless you
   change the height setting in the Configuration window (Alt-F6), Visual
   options page, Screen Height section.



Q. 64COPY won't recognize the F11/F12 keys on the keyboard. Why?

A. It seems that there are some (rare) BIOS's that are not 100% compatible
   with the method I use for scanning the keyboard (for the extended keys),
   and therefore I can't fix this. However, there are ways around this.
   Since 64COPY has a working MENU system (using the F9 key, or with the
   mouse), you can use the menus to accomplish nearly all that would
   normally need to be done with the keyboard.



Q. How can I contact the author?

A. For now, my email address is "schepers@ist.uwaterloo.ca". It would be
   the best way to get me. My present snail-mail address is:

      Peter Schepers
      4-1227 Nellis Street
      Woodstock, Ontario, Canada
      N4T 1N8

