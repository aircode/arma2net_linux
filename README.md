arma2net_linux
==============

sources from ScottNZ modified to compile it in monodevelop 4.0
sources from ScottNZ modified to compile it in monodevelop 4.0 Distribution; Linux Mint Qiana "Cinnamon" arch; amd64

Mono JIT compiler version 3.2.8 (Debian 3.2.8+dfsg-4ubuntu1) Copyright (C) 2002-2014 Novell, Inc, Xamarin Inc and Contributors. www.mono-project.com TLS: __thread SIGSEGV: altstack Notifications: epoll Architecture: amd64 Disabled: none Misc: softdebug LLVM: supported, not enabled. GC: sgen

the following packets additional installed:

via synaptic; libart2.0-cil (2.24.2-3) libgnome-vfs2.0-cil (2.24.2-3) libgnome2.24-cil (2.24.2-3) libicsharpcode-nrefactory-cecil5.0-cil (5.3.0+20130718.73b6d0f-1) libicsharpcode-nrefactory-csharp5.0-cil (5.3.0+20130718.73b6d0f-1) libicsharpcode-nrefactory5.0-cil (5.3.0+20130718.73b6d0f-1) libmono-addins-cil-dev (1.0+git20130406.adcd75b-3) libmono-addins-msbuild-cil-dev (1.0+git20130406.adcd75b-3) libmono-addins-msbuild0.2-cil (1.0+git20130406.adcd75b-3) libmono-cecil-vb0.9-cil (3.0~pre20130627.4dcc70f-1) libmono-debugger-soft-cil (0+20131201.3459502-1) libmono-debugging-cil (0+20131201.3459502-1) libmono-debugging-soft-cil (0+20131201.3459502-1) libmono-microsoft-visualbasic10.0-cil (3.0~pre20130627.4dcc70f-1) libmono-microsoft-visualbasic11.0-cil (3.0~pre20130627.4dcc70f-1) libmono-microsoft-visualbasic8.0-cil (3.0~pre20130627.4dcc70f-1) libmonosgen-2.0-dev (3.2.8+dfsg-4ubuntu1) mono-basic-dbg (3.0~pre20130627.4dcc70f-1) mono-dbg (3.2.8+dfsg-4ubuntu1) mono-dmcs (3.2.8+dfsg-4ubuntu1) mono-gmcs (3.2.8+dfsg-4ubuntu1) mono-runtime-boehm (3.2.8+dfsg-4ubuntu1) mono-vbnc (3.0~pre20130627.4dcc70f-1) monodevelop (4.0.12+dfsg-2)

via shell: Commandline: apt-get install mono-mcs Commandline: apt-get install mono-devel Commandline: apt-get install mono-complete

Next step get the sources;

    git clone https://github.com/ScottNZ/Arma2NET.git
    cd Arma2NET
    git checkout linux
    change the "tools" version in the *.csproj files from 12.0 > 4.0 -line 2 looks after change -You need to change this line in the files; Arma2Net.Addins.csproj, DateTimeAddin.csproj, TestAddin.csproj -to change the files, open it in a Text editor
    Open the Arma2Net.sln in monodevelop, it complain about Scotts unidentified project but this is anyway.
    debug or realease to compile, after this you get Arma2Net.Addins.dll, DateTimeAddin.dll, TestAddin.dll which are located in the debug or release folders.
    now copy the Addins;
    copy all the files into the Linux folder >>>>>>Linux folder (inside of Arma2NET) needed! structure<<<<<<<<< Linux>Arma2Net.Addins.dll Linux>DateTimeAddin.dll Linux>TestAddin.dll Linux>DateTimeAddin.dll Linux>Addins>Arma2Net.Addins.dll Linux>Addins>DateTimeAddin.dll Linux>Addins>TestAddin.dll Linux>Addins>DateTime.Addin>DateTimeAddin.dll
    open a terminal and "make" in "Linux" folder, build a pretty working Arma2Net.so and RVExtensionTest
    now ./RVExtensionTest and ask for DateTime now

I think this ll help everybody build, additional i can drop the compiled & src package on github. If anyone got a simple testmission for arma3, it would be gentle to share me. thx

compiled i testet it on Ubuntu 14.04 LTS Server additional packages needed are mono-complete & libmonoboehm-2.0-1 runs pretty :> 
