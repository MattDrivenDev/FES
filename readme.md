# FES - F# Entertainment System

An _attempt_ to build a NES emulator using F# and MonoGame. 

Goodness knows why.

## Reference

* [NES DEV Wiki](https://wiki.nesdev.com/w/index.php/Nesdev_Wiki)
* [Nintendo Entertainment System Documentation](http://nesdev.com/NESDoc.pdf)
* [Programming Manual](http://users.telenet.be/kim1-6502/6502/proman.html)
* [6502 Instruction Set](http://obelisk.me.uk/6502/instructions.html)
* [6502 Instruction Reference](http://obelisk.me.uk/6502/reference.html)
* [6502 Algorithms](http://obelisk.me.uk/6502/algorithms.html)
* [Video: Reverse Engineering the MOS 6502 CPU](https://www.youtube.com/watch?v=fWqBmmPQP40)

## Journal

In reverse date order - this will suit fine until I can be bothered to use a real blog (if ever). 

### 21/09/16

Mood: :smile:

Listened to the audio from the [Reverse Engineering the MOS 6502 CPU](https://www.youtube.com/watch?v=fWqBmmPQP40) video for a while this morning, had my mind blown. So many things that I don't understand but are still super interesting.

Added a few more files/modules that are basically empty at the moment which help serve as placeholders for the structure of this project. I'm starting to think I've bitten off way more than I can chew - but the subject is really interesting so I'll continue to plod onwards while I'm enjoying the process.

I plan to get to grips with the instruction set and associated opcodes next - from what I can tell not only will there be far more than I had to implement in the [Chip-8]() project I did recently; and seems like the situation is made more complicated with _unofficial_ instructions that some NES games will take advantage of. 