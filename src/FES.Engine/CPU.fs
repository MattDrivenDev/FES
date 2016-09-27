/// Central Processing Unit
/// Ricoh produced an NMOS processor based on the 6502, the 2A03. The chip differed from a
/// standard 6502 in that it had the ability to handle sound, serving as pAPU (pseudo-Audio
/// Processing Unit) as well as CPU, and that it lacked a Binary Coded Decimal (BCD) mode
/// which allowed representing each digit using 4 bits. For the purposes of programming, the
/// 2A03 uses the same instruction set as the standard 6502 which is shown in figure 2-1. The
/// 6502 is a little endian processor which means that addresses are stored in memory least
/// significant byte first, for example the address $1234 would be stored in memory as $34 at
/// memory location x and $12 at memory location (x + 1). 
module CPU

/// Addressing Modes
/// The 6502 has several different addressing modes, providing different ways to access
/// memory locations. There are also addressing modes which operate on the contents of
/// registers, rather than memory. In total there are 13 different addressing modes on the 6502.
/// Some instructions can use more than one different addressing mode.
type AddressingMode = 

  /// Some instructions operate directly on the contents of the accumulator. The only instructions
  /// to use this addressing mode are the shift instructions, ASL (Arithmetic Shift Left), LSR
  /// (Logical Shift Right), ROL (Rotate Left) and ROR (Rotate Right). 
  | Accumulator

  /// In absolute addressing, the address of the data to operate on is specified by the two
  /// operands supplied, least significant byte first. 
  | Absolute  

  /// Indexed absolute addressing takes two operands, forming a 16-bit address, least significant
  /// byte first, and adds the value of a register to it to give the address where the data can be
  /// found. For example, if the operands are bb and cc, the address of the data will be ccbb + X. 
  | AbsoluteIndexedX
  | AbsoluteIndexedY

  /// Instructions which use immediate addressing operate directly on a constant supplied as an
  /// operand to the instruction. Immediate instructions are indicated by prefacing the operand
  /// with #, for example AND #$12.
  | Immediate

  /// Many instructions do not require access to operands stored in memory. Examples of implied
  /// instructions are CLD (Clear Decimal Mode) and NOP (No Operation). 
  | Implied

  /// Indirect addressing takes two operands, forming a 16-bit address, which identifies the least
  /// significant byte of another address which is where the data can be found. For example if the
  /// operands are bb and cc, and ccbb contains xx and ccbb + 1 contains yy, then the real target
  /// address is yyxx. On the 6502, only JMP (Jump) uses this addressing mode and an example
  /// is JMP ($1234).
  | Indirect

  /// Indexed indirect (also known as pre-indexed) addressing takes a single byte as an operand
  /// and adds the value of the X register to it (with wraparound) to give the address of the least
  /// significant byte of the target address. For example, if the operand is bb, 00bb is xx and 00bb
  /// + 1 is yy then the data can be found at yyxx. An example of this addressing mode is AND ($12,X). 
  | IndirectPreIndexedX

  /// Indirect indexed (also known as post-indexed) addressing takes a single operand which
  /// gives the zero page address of the least significant byte of a 16-bit address which is then
  /// added to the Y register to give the target address. For example, if the operand is bb, 00bb is
  /// xx and 00bb + 1 is yy, then the data can be found at yyxx. An example of this addressing
  /// mode is AND ($12),Y. 
  | IndirectPostIndexedY

  /// Relative addressing is used in branch instructions. This addressing mode causes the value
  /// of the program counter to change if a certain condition is met. The condition is dependant on
  /// the instruction. The program counter increments by two regardless of the outcome of the
  /// condition but if the condition is true the single operand is added to the program counter to
  /// give the new value. For this purpose, the operand is interpreted as a signed byte, that is in
  /// the range -128 to 127 to allow forward and backward branching. An example of this
  /// addressing mode is BCS *+5 where * represents the current value of the program counter.
  | Relative

  /// Zero page addressing uses a single operand which serves as a pointer to an address in zero
  /// page ($0000-$00FF) where the data to be operated on can be found. By using zero page
  /// addressing, only one byte is needed for the operand, so the instruction is shorter and,
  /// therefore, faster to execute than with addressing modes which take two operands
  | ZeroPage

  /// Indexed zero page addressing takes a single operand and adds the value of a register to it to
  /// give an address in zero page ($0000-$00FF) where the data can be found.
  | ZeroPageIndexedX
  | ZeroPageIndexedY

/// Data type encapsulating a 6502 CPU instruction.
type Instruction =  
  | BRK | ORA | ASL | PHP | BPL | CLC | JSR | AND | BIT | ROL | PLP | BMI
  | SEC | RTI | EOR | LSR | PHA | JMP | BVC | CLI | RTS | ADC | ROR | PLA
  | BVS | SEI | STA | STY | STX | DEY | TXA | BCC | TYA | TXS | LDY | LDA
  | LDX | TAY | TAX | BCS | CLV | TSX | CPY | CMP | DEC | INY | DEX | BNE
  | CLD | CPX | SBC | INC | INX | NOP | BEQ | SED

/// The 6502 Instruction Set is arranged as an array which can be accessed 
/// directly by the opcode read from memory during the CPU cycle. We'll access 
/// this from a helper function (interpret) below so we can perform some 
/// logging etc.
let private InstructionSet = 
  [| (*0x00*) Some (BRK, Implied)
     (*0x01*) Some (ORA, IndirectPreIndexedX)
     (*0x02*) None
     (*0x03*) None
     (*0x04*) None
     (*0x05*) Some (ORA, ZeroPage)
     (*0x06*) Some (ASL, ZeroPage)
     (*0x07*) None
     (*0x08*) Some (PHP, Implied)
     (*0x09*) Some (ORA, Immediate)
     (*0x0A*) Some (ASL, Accumulator)
     (*0x0B*) None
     (*0x0C*) None
     (*0x0D*) Some (ORA, Absolute)
     (*0x0E*) Some (ASL, Absolute)
     (*0x0F*) None
              
     (*0x10*) Some (BPL, Relative)
     (*0x11*) Some (ORA, IndirectPostIndexedY)
     (*0x12*) None
     (*0x13*) None
     (*0x14*) None
     (*0x15*) Some (ORA, ZeroPageIndexedX)
     (*0x16*) Some (ASL, ZeroPageIndexedX)
     (*0x17*) None
     (*0x18*) Some (CLC, Implied)
     (*0x19*) Some (ORA, AbsoluteIndexedY)
     (*0x1A*) None
     (*0x1B*) None
     (*0x1C*) None
     (*0x1D*) Some (ORA, AbsoluteIndexedX)
     (*0x1E*) Some (ASL, AbsoluteIndexedX)
     (*0x1F*) None
              
     (*0x20*) Some (JSR, Absolute)
     (*0x21*) Some (AND, IndirectPreIndexedX)
     (*0x22*) None
     (*0x23*) None
     (*0x24*) Some (BIT, ZeroPage)
     (*0x25*) Some (AND, ZeroPage)
     (*0x26*) Some (ROL, ZeroPage)
     (*0x27*) None
     (*0x28*) Some (PLP, Implied)
     (*0x29*) Some (AND, Immediate)
     (*0x2A*) Some (ROL, Accumulator)
     (*0x2B*) None
     (*0x2C*) Some (BIT, Absolute)
     (*0x2D*) Some (AND, Absolute)
     (*0x2E*) Some (ROL, Absolute)
     (*0x2F*) None
              
     (*0x30*) Some (BMI, Relative)
     (*0x31*) Some (AND, IndirectPostIndexedY)
     (*0x32*) None
     (*0x33*) None
     (*0x34*) None
     (*0x35*) Some (AND, ZeroPageIndexedX)
     (*0x36*) Some (ROL, ZeroPageIndexedX)
     (*0x37*) None
     (*0x38*) Some (SEC, Implied)
     (*0x39*) Some (AND, AbsoluteIndexedY)
     (*0x3A*) None
     (*0x3B*) None
     (*0x3C*) None
     (*0x3D*) Some (AND, AbsoluteIndexedX)
     (*0x3E*) Some (ROL, AbsoluteIndexedX)
     (*0x3F*) None
              
     (*0x40*) Some (RTI, Implied)
     (*0x41*) Some (EOR, IndirectPreIndexedX)
     (*0x42*) None
     (*0x43*) None
     (*0x44*) None
     (*0x45*) Some (EOR, ZeroPage)
     (*0x46*) Some (LSR, ZeroPage)
     (*0x47*) None
     (*0x48*) Some (PHA, Implied)
     (*0x49*) Some (EOR, Immediate)
     (*0x4A*) Some (LSR, Accumulator)
     (*0x4B*) None
     (*0x4C*) Some (JMP, Absolute)
     (*0x4D*) Some (EOR, Absolute)
     (*0x4E*) Some (LSR, Absolute)
     (*0x4F*) None
              
     (*0x50*) Some (BVC, Relative)
     (*0x51*) Some (EOR, IndirectPostIndexedY)
     (*0x52*) None
     (*0x53*) None
     (*0x54*) None
     (*0x55*) Some (EOR, ZeroPageIndexedX)
     (*0x56*) Some (LSR, ZeroPageIndexedX)
     (*0x57*) None
     (*0x58*) Some (CLI, Implied)
     (*0x59*) Some (EOR, AbsoluteIndexedY)
     (*0x5A*) None
     (*0x5B*) None
     (*0x5C*) None
     (*0x5D*) Some (EOR, AbsoluteIndexedX)
     (*0x5E*) Some (LSR, AbsoluteIndexedX)
     (*0x5F*) None
              
     (*0x60*) Some (RTS, Implied)
     (*0x61*) Some (ADC, IndirectPreIndexedX)
     (*0x62*) None
     (*0x63*) None
     (*0x64*) None
     (*0x65*) Some (ADC, ZeroPage)
     (*0x66*) Some (ROR, ZeroPage)
     (*0x67*) None
     (*0x68*) Some (PLA, Implied)
     (*0x69*) Some (ADC, Immediate)
     (*0x6A*) Some (ROR, Accumulator)
     (*0x6B*) None
     (*0x6C*) Some (JMP, Indirect)
     (*0x6D*) Some (ADC, Absolute)
     (*0x6E*) Some (ROR, Absolute)
     (*0x6F*) None
              
     (*0x70*) Some (BVS, Relative)
     (*0x71*) Some (ADC, IndirectPostIndexedY)
     (*0x72*) None
     (*0x73*) None
     (*0x74*) None
     (*0x75*) Some (ADC, ZeroPageIndexedX)
     (*0x76*) Some (ROR, ZeroPageIndexedX)
     (*0x77*) None
     (*0x78*) Some (SEI, Implied)
     (*0x79*) Some (ADC, AbsoluteIndexedY)
     (*0x7A*) None
     (*0x7B*) None
     (*0x7C*) None
     (*0x7D*) Some (ADC, AbsoluteIndexedX)
     (*0x7E*) Some (ROR, AbsoluteIndexedX)
     (*0x7F*) None
              
     (*0x80*) None
     (*0x81*) Some (STA, IndirectPreIndexedX)
     (*0x82*) None
     (*0x83*) None
     (*0x84*) Some (STY, ZeroPage)
     (*0x85*) Some (STA, ZeroPage)
     (*0x86*) Some (STX, ZeroPage)
     (*0x87*) None
     (*0x88*) Some (DEY, Implied)
     (*0x89*) None
     (*0x8A*) Some (TXA, Implied)
     (*0x8B*) None
     (*0x8C*) Some (STY, Absolute)
     (*0x8D*) Some (STA, Absolute)
     (*0x8E*) Some (STX, Absolute)
     (*0x8F*) None
              
     (*0x90*) Some (BCC, Relative)
     (*0x91*) Some (STA, IndirectPostIndexedY)
     (*0x92*) None
     (*0x93*) None
     (*0x94*) Some (STY, ZeroPageIndexedX)
     (*0x95*) Some (STA, ZeroPageIndexedX)
     (*0x96*) Some (STX, ZeroPageIndexedX)
     (*0x97*) None
     (*0x98*) Some (TYA, Implied)
     (*0x99*) Some (STA, AbsoluteIndexedY)
     (*0x9A*) Some (TXS, Implied)
     (*0x9B*) None
     (*0x9C*) None
     (*0x9D*) Some (STA, AbsoluteIndexedX)
     (*0x9E*) None
     (*0x9F*) None
              
     (*0xA0*) Some (LDY, Immediate)
     (*0xA1*) Some (LDA, IndirectPreIndexedX)
     (*0xA2*) Some (LDX, Immediate)
     (*0xA3*) None
     (*0xA4*) Some (LDY, ZeroPage)
     (*0xA5*) Some (LDA, ZeroPage)
     (*0xA6*) Some (LDX, ZeroPage)
     (*0xA7*) None
     (*0xA8*) Some (TAY, Implied)
     (*0xA9*) Some (LDA, Immediate)
     (*0xAA*) Some (TAX, Implied)
     (*0xAB*) None
     (*0xAC*) Some (LDY, Absolute)
     (*0xAD*) Some (LDA, Absolute)
     (*0xAE*) Some (LDX, Absolute)
     (*0xAF*) None
              
     (*0xB0*) Some (BCS, Relative)
     (*0xB1*) Some (LDA, IndirectPostIndexedY)
     (*0xB2*) None
     (*0xB3*) None
     (*0xB4*) Some (LDY, ZeroPageIndexedX)
     (*0xB5*) Some (LDA, ZeroPageIndexedX)
     (*0xB6*) Some (LDX, ZeroPageIndexedY)
     (*0xB7*) None
     (*0xB8*) Some (CLV, Implied)
     (*0xB9*) Some (LDA, AbsoluteIndexedY)
     (*0xBA*) Some (TSX, Implied)
     (*0xBB*) None
     (*0xBC*) Some (LDY, AbsoluteIndexedX)
     (*0xBD*) Some (LDA, AbsoluteIndexedX)
     (*0xBE*) Some (LDX, AbsoluteIndexedY)
     (*0xBF*) None
              
     (*0xC0*) Some (CPY, Immediate)
     (*0xC1*) Some (CMP, IndirectPreIndexedX)
     (*0xC2*) None
     (*0xC3*) None
     (*0xC4*) Some (CPY, ZeroPage)
     (*0xC5*) Some (CMP, ZeroPage)
     (*0xC6*) Some (DEC, ZeroPage)
     (*0xC7*) None
     (*0xC8*) Some (INY, Implied)
     (*0xC9*) Some (CMP, Immediate)
     (*0xCA*) Some (DEX, Implied)
     (*0xCB*) None
     (*0xCC*) Some (CPY, Absolute)
     (*0xCD*) Some (CMP, Absolute)
     (*0xCE*) Some (DEC, Absolute)
     (*0xCF*) None
              
     (*0xD0*) Some (BNE, Relative)
     (*0xD1*) Some (CMP, IndirectPostIndexedY)
     (*0xD2*) None
     (*0xD3*) None
     (*0xD4*) None
     (*0xD5*) Some (CMP, ZeroPageIndexedX)
     (*0xD6*) Some (DEC, ZeroPageIndexedX)
     (*0xD7*) None
     (*0xD8*) Some (CLD, Implied)
     (*0xD9*) Some (CMP, AbsoluteIndexedY)
     (*0xDA*) None
     (*0xDB*) None
     (*0xDC*) None
     (*0xDD*) Some (CMP, AbsoluteIndexedX)
     (*0xDE*) Some (DEC, AbsoluteIndexedX)
     (*0xDF*) None
              
     (*0xE0*) Some (CPX, Immediate)
     (*0xE1*) Some (SBC, IndirectPreIndexedX)
     (*0xE2*) None
     (*0xE3*) None
     (*0xE4*) Some (CPX, ZeroPage)
     (*0xE5*) Some (SBC, ZeroPage)
     (*0xE6*) Some (INC, ZeroPage)
     (*0xE7*) None
     (*0xE8*) Some (INX, Implied)
     (*0xE9*) Some (SBC, Immediate)
     (*0xEA*) Some (NOP, Implied)
     (*0xEB*) None
     (*0xEC*) Some (CPX, Absolute)
     (*0xED*) Some (SBC, Absolute)
     (*0xEE*) Some (INC, Absolute)
     (*0xEF*) None
              
     (*0xF0*) Some (BEQ, Relative)
     (*0xF1*) Some (SBC, IndirectPostIndexedY)
     (*0xF2*) None
     (*0xF3*) None
     (*0xF4*) None
     (*0xF5*) Some (SBC, ZeroPageIndexedX)
     (*0xF6*) Some (INC, ZeroPageIndexedX)
     (*0xF7*) None
     (*0xF8*) Some (SED, Implied)
     (*0xF9*) Some (SBC, AbsoluteIndexedY)
     (*0xFA*) None
     (*0xFB*) None
     (*0xFC*) None
     (*0xFD*) Some (SBC, AbsoluteIndexedX)
     (*0xFE*) Some (INC, AbsoluteIndexedX)
     (*0xFF*) None |]

/// Maps an byte encoded opcode into a CPU instruction
let interpret (opcode:byte) = 
  let log (instruction, addressingMode) = 
    printfn "instruction: %A" instruction
    printfn "addressing mode: %A" addressingMode
  hex opcode |> printfn "opcode: %s"  
  let result = InstructionSet.[int opcode]
  Option.iter log result  
  result

/// Set the interrupt disable flag to true/one
let setInterruptDisable() = 
  Memory.P.I := true

/// Sets the decimal mode flag to false/zero
let clearDecimalMode() = 
  Memory.P.D := false

/// Loads a byte of memory into the accumulator setting the zero and negative
/// flags as appropriate
let loadAccumulator mode =
  match mode with
  | Immediate ->
    let data = Memory.read()
    Memory.incrementPC()
    Memory.A := data
    Memory.P.Z := !Memory.A = 0uy
  | _ -> () // TODO: other addressing modes

/// Tick's the CPU through one cycle - reading byte's from memory to perform 
/// CPU instructions
let cycle() = 
  printfn "- CPU Cycle --------------------------------------"
  let opcode = Memory.read()
  Memory.incrementPC()
  match interpret opcode with
  | None -> ()
  | Some (instruction, addressingMode) ->
    match instruction with
    | SEI -> setInterruptDisable()
    | CLD -> clearDecimalMode()
    | LDA -> loadAccumulator addressingMode
    | _ -> ()