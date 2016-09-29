# FES - F# Entertainment System

An _attempt_ to build a NES emulator using F# and MonoGame. 

Goodness knows why.

## Reference

* [NES DEV Wiki](https://wiki.nesdev.com/w/index.php/Nesdev_Wiki)
* [Nintendo Entertainment System Documentation](http://nesdev.com/NESDoc.pdf)
* [Programming Manual](http://users.telenet.be/kim1-6502/6502/proman.html)
* [6502 Instruction Set](http://e-tradition.net/bytes/6502/6502_instruction_set.html)
* [6502 Instruction Reference](http://obelisk.me.uk/6502/reference.html)
* [6502 Algorithms](http://obelisk.me.uk/6502/algorithms.html)
* [Video: Reverse Engineering the MOS 6502 CPU](https://www.youtube.com/watch?v=fWqBmmPQP40)

## Journal

In reverse date order - this will suit fine until I can be bothered to use a real blog (if ever). 

### 29/09/16

So I followed up on the CPU instruction set as I thought last time... and done some rather cosmetic refactoring so that the code is organised in such a way that it more closely follows the documentation found online (see above). I think that `F#` shows some merit here because of it's ability to be super terse.

The program now outputs some data to the logs while running... 

```
- CPU CYCLE -------------------------------- BEGIN
reading memory: 0x4A
opcode: 0x19
instruction: ORA AbY
- CPU CYCLE ---------------------------------- END
- CPU CYCLE -------------------------------- BEGIN
reading memory: 0x4B
opcode: 0x8E
instruction: STX Abs
- CPU CYCLE ---------------------------------- END
- CPU CYCLE -------------------------------- BEGIN
reading memory: 0x4C
opcode: 0xEE
instruction: INC Abs
- CPU CYCLE ---------------------------------- END
```

At the moment the program is running through the program data from RAM, which is incorrect - I need to flesh out the `MemoryMap` a little more so that it can map addresses to both RAM and the 1 or 2 banks of PRG DATA in the ROM itself. There is some further mapping that will also need to be done for that later as well, and as far as I know some swapping of banks of memory in the ROM for games larger than two 16KB blocks of PRG DATA.

There [seems some debate](http://forums.nesdev.com/viewtopic.php?t=3677) where the `PC` should start - either `0x8000` which is where the 1st bank of PGR DATA seems to start, or at some kind of *initializer* location in memory that runs a few instructions before jumping to the correct starting location for the ROM itself.

Lunchtimes need to be longer...

### 27/09/16

Mood: :smile:

Really enjoying this.

Still struggling over how I want to map data (`opcode`) to action (`instruction`) - and what that instruction should *actually* be.

I thought that I was happy with using types as a sum-type lends itself to it nicely. But have considered the following too:

```fsharp
type Instruction = unit -> unit
```

Where an example instruction:

```fsharp
val LDA = AddressingMode -> unit
```

...could be partially applied within the `InstructionSet` with the appropriate `AddressingMode`...

```fsharp
let InstructionSet = 
  [| ....
     (*0xA9*) LDA Immediate // = unit -> unit 
     ....|]
```

Since all (afaik) instructions will either have no effect on memory or will mutate existing memory all instructions essentially boil down to a `unit -> unit` function (`void` method with no parameters). It's a case of working out how to not need those parameters or make sure they are baked in already within the instruction set.

All this of course doesn't help with **getting it done** - but you know? 

### 26/09/16

Mood: :smile:

Just wanted to kind of play with a few ideas of how to interpret ROM bytecode into CPU instructions.

Decided I think on using a data type (`F#` Sum Type) in order to represent those instructions. Each available instruction in the 6502 CPU will be accessed via a function that looks into the array (in theory the opcode will map directly as an index on that array).

### 21/09/16

Mood: :smile:

Listened to the audio from the [Reverse Engineering the MOS 6502 CPU](https://www.youtube.com/watch?v=fWqBmmPQP40) video for a while this morning, had my mind blown. So many things that I don't understand but are still super interesting.

Added a few more files/modules that are basically empty at the moment which help serve as placeholders for the structure of this project. I'm starting to think I've bitten off way more than I can chew - but the subject is really interesting so I'll continue to plod onwards while I'm enjoying the process.

I plan to get to grips with the instruction set and associated opcodes next - from what I can tell not only will there be far more than I had to implement in the [Chip-8]() project I did recently; and seems like the situation is made more complicated with _unofficial_ instructions that some NES games will take advantage of. 