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

/// Program Counter
let PC = ref 0us

/// Stack Pointer
let SP = ref 0uy

/// Accumulator
let A = ref 0uy

/// Index Register X
let X = ref 0uy

/// Index Register Y
let Y = ref 0uy

/// Carry Flag 
let C = ref false

/// Zero Flag
let Z = ref false

/// Interupt Disable
let I = ref false

/// Decimal Mode
let D = ref false

/// Break Command
let B = ref false

/// Overflow
let V = ref false

/// Negative
let N = ref false

/// Increments the program counter (PC) by 1
let tick() = PC := !PC + 1us

/// CPU takes the next byte from memory at the location of the PC, which is then incremented
let take() = 
  let b = !PC |> Memory.read
  tick(); b

/// Addressing Modes
/// The 6502 has several different addressing modes, providing different ways to access
/// memory locations. There are also addressing modes which operate on the contents of
/// registers, rather than memory. In total there are 13 different addressing modes on the 6502.
/// Some instructions can use more than one different addressing mode.
type AddressingMode = | Acc | Abs | AbX | AbY | Imm | Imp | Ind | InX | InY | Rel | Zpg | ZpX | ZpY

/// Add with Carry
let ADC mode () = 
  printfn "instruction: ADC %A" mode

/// Logical AND
let AND mode () = 
  printfn "instruction: AND %A" mode

/// Arithmetic Shift Left
let ASL mode () = 
  printfn "instruction: ASL %A" mode

/// Branch if Carry Clear
let BCC mode () = 
  printfn "instruction: BCC %A" mode

/// Branch if Carry Set
let BCS mode () = 
  printfn "instruction: BCS %A" mode

/// Branch if Equal
let BEQ mode () = 
  printfn "instruction: BEQ %A" mode

/// Bit Test
let BIT mode () = 
  printfn "instruction: BIT %A" mode

/// Branch if Minus
let BMI mode () = 
  printfn "instruction: BMI %A" mode

/// Branch if Not Equal
let BNE mode () = 
  printfn "instruction: BNE %A" mode

/// Branch if Positive
let BPL mode () = 
  printfn "instruction: BPL %A" mode

/// Force Interrupt
let BRK mode () = 
  printfn "instruction: BRK %A" mode

/// Branch if Overflow Clear
let BVC mode () = 
  printfn "instruction: BVC %A" mode

/// Branch if Overflow Set
let BVS mode () = 
  printfn "instruction: BVS %A" mode

/// Clear Carry Flag
let CLC mode () = 
  printfn "instruction: CLC %A" mode

/// Clear Decimal Mode
let CLD mode () = 
  printfn "instruction: CLD %A" mode

/// Clear Interrupt Disable
let CLI mode () = 
  printfn "instruction: CLI %A" mode

/// Clear Overflow Flag
let CLV mode () = 
  printfn "instruction: CLV %A" mode

/// Compare
let CMP mode () = 
  printfn "instruction: CMP %A" mode

/// Compare X Register
let CPX mode () = 
  printfn "instruction: CPX %A" mode

/// Compare Y Register
let CPY mode () = 
  printfn "instruction: CPY %A" mode

/// Decrement Memory
let DEC mode () = 
  printfn "instruction: DEC %A" mode

/// Decrement X Register
let DEX mode () = 
  printfn "instruction: DEX %A" mode

/// Decrement Y Register
let DEY mode () = 
  printfn "instruction: DEY %A" mode

/// Exclusive OR
let EOR mode () = 
  printfn "instruction: EOR %A" mode

/// Increment Memory
let INC mode () = 
  printfn "instruction: INC %A" mode

/// Increment X Register
let INX mode () = 
  printfn "instruction: INX %A" mode

/// Increment Y Register
let INY mode () = 
  printfn "instruction: INY %A" mode

/// Jump
let JMP mode () = 
  printfn "instruction: JMP %A" mode

/// Jump to Subroutine
let JSR mode () = 
  printfn "instruction: JSR %A" mode

/// Load Accumulator
let LDA mode () = 
  printfn "instruction: LDA %A" mode

/// Load X Register
let LDX mode () = 
  printfn "instruction: LDX %A" mode

/// Load Y Register
let LDY mode () = 
  printfn "instruction: LDY %A" mode

/// Logical Shift Right
let LSR mode () = 
  printfn "instruction: LSR %A" mode

/// No Operation
let NOP mode () = 
  printfn "instruction: NOP %A" mode

/// Logical Inclusive OR
let ORA mode () = 
  printfn "instruction: ORA %A" mode

/// Push Accumulator
let PHA mode () = 
  printfn "instruction: PHA %A" mode

/// Push Processor Status
let PHP mode () = 
  printfn "instruction: PHP %A" mode
 
/// Pull Accumulator
let PLA mode () = 
  printfn "instruction: PLA %A" mode

/// Pull Processor Status
let PLP mode () = 
  printfn "instruction: PLP %A" mode

/// Rotate Left
let ROL mode () = 
  printfn "instruction: ROL %A" mode

/// Rotate Right
let ROR mode () = 
  printfn "instruction: ROR %A" mode

/// Return from Interrupt
let RTI mode () = 
  printfn "instruction: RTI %A" mode

/// Return from Subroutine
let RTS mode () = 
  printfn "instruction: RTS %A" mode

/// Subtract with Carry
let SBC mode () = 
  printfn "instruction: SBC %A" mode

/// Set Carry Flag
let SEC mode () = 
  printfn "instruction: SEC %A" mode

/// Set Decimal Flag
let SED mode () = 
  printfn "instruction: SED %A" mode

/// Set Interrupt Disable
let SEI mode () = 
  printfn "instruction: SEI %A" mode

/// Store Accumulator
let STA mode () = 
  printfn "instruction: STA %A" mode

/// Store X Register
let STX mode () = 
  printfn "instruction: STX %A" mode

/// Store Y Register
let STY mode () = 
  printfn "instruction: STY %A" mode

/// Transfer Accumulator to X
let TAX mode () = 
  printfn "instruction: TAX %A" mode
  
/// Transfer Accumulator to Y
let TAY mode () = 
  printfn "instruction: TAY %A" mode

/// Transfer Stack Pointer to X
let TSX mode () = 
  printfn "instruction: TSX %A" mode

/// Transfer X to Accumulator
let TXA mode () = 
  printfn "instruction: TXA %A" mode

/// Transfer X to Stack Pointer
let TXS mode () = 
  printfn "instruction: TXS %A" mode

/// Transfer Y to Accumulator
let TYA mode () = 
  printfn "instruction: TYA %A" mode 

/// Just helps with the formatting of the Instruction Set below
let private _______ = ignore

/// The 6502 Instruction Set is arranged as an array which can be accessed 
/// directly by the opcode read from memory during the CPU cycle. We'll access 
/// this from a helper function (interpret) below so we can perform some 
/// logging etc.
/// NOTE: Layout is just so that it more closely resembles the instruction set grid here: 
/// http://e-tradition.net/bytes/6502/6502_instruction_set.html
let private InstructionSet = 
  [| BRK Imp;  ORA InX;  _______;  _______;  _______;  ORA Zpg;  ASL Zpg;  _______;  PHP Imp;  ORA Imm;  ASL Acc;  _______;  _______;  ORA Abs;  ASL Abs;  _______     
     BPL Rel;  ORA InY;  _______;  _______;  _______;  ORA ZpX;  ASL ZpX;  _______;  CLC Imp;  ORA AbY;  _______;  _______;  _______;  ORA AbX;  ASL AbX;  _______     
     JSR Abs;  AND InX;  _______;  _______;  BIT Zpg;  AND Zpg;  ROL Zpg;  _______;  PLP Imp;  AND Imm;  ROL Acc;  _______;  BIT Abs;  AND Abs;  ROL Abs;  _______     
     BMI Rel;  AND InY;  _______;  _______;  _______;  AND ZpX;  ROL ZpX;  _______;  SEC Imp;  AND AbY;  _______;  _______;  _______;  AND AbX;  ROL AbX;  _______     
     RTI Imp;  EOR InX;  _______;  _______;  _______;  EOR Zpg;  LSR Zpg;  _______;  PHA Imp;  EOR Imm;  LSR Acc;  _______;  JMP Abs;  EOR Abs;  LSR Abs;  _______     
     BVC Rel;  EOR InY;  _______;  _______;  _______;  EOR ZpX;  LSR ZpX;  _______;  CLI Imp;  EOR AbY;  _______;  _______;  _______;  EOR AbX;  LSR AbX;  _______     
     RTS Imp;  ADC InX;  _______;  _______;  _______;  ADC Zpg;  ROR Zpg;  _______;  PLA Imp;  ADC Imm;  ROR Acc;  _______;  JMP Ind;  ADC Abs;  ROR Abs;  _______     
     BVS Rel;  ADC InY;  _______;  _______;  _______;  ADC ZpX;  ROR ZpX;  _______;  SEI Imp;  ADC AbY;  _______;  _______;  _______;  ADC AbX;  ROR AbX;  _______     
     _______;  STA InX;  _______;  _______;  STY Zpg;  STA Zpg;  STX Zpg;  _______;  DEY Imp;  _______;  TXA Imp;  _______;  STY Abs;  STA Abs;  STX Abs;  _______     
     BCC Rel;  STA InY;  _______;  _______;  STY ZpX;  STA ZpX;  STX ZpX;  _______;  TYA Imp;  STA AbY;  TXS Imp;  _______;  _______;  STA AbX;  _______;  _______     
     LDY Imm;  LDA InX;  LDX Imm;  _______;  LDY Zpg;  LDA Zpg;  LDX Zpg;  _______;  TAY Imp;  LDA Imm;  TAX Imp;  _______;  LDY Abs;  LDA Abs;  LDX Abs;  _______     
     BCS Rel;  LDA InY;  _______;  _______;  LDY ZpX;  LDA ZpX;  LDX ZpY;  _______;  CLV Imp;  LDA AbY;  TSX Imp;  _______;  LDY AbX;  LDA AbX;  LDX AbY;  _______ 
     CPY Imm;  CMP InX;  _______;  _______;  CPY Zpg;  CMP Zpg;  DEC Zpg;  _______;  INY Imp;  CMP Imm;  DEX Imp;  _______;  CPY Abs;  CMP Abs;  DEC Abs;  _______     
     BNE Rel;  CMP InY;  _______;  _______;  _______;  CMP ZpX;  DEC ZpX;  _______;  CLD Imp;  CMP AbY;  _______;  _______;  _______;  CMP AbX;  DEC AbX;  _______     
     CPX Imm;  SBC InX;  _______;  _______;  CPX Zpg;  SBC Zpg;  INC Zpg;  _______;  INX Imp;  SBC Imm;  NOP Imp;  _______;  CPX Abs;  SBC Abs;  INC Abs;  _______     
     BEQ Rel;  SBC InY;  _______;  _______;  _______;  SBC ZpX;  INC ZpX;  _______;  SED Imp;  SBC AbY;  _______;  _______;  _______;  SBC AbX;  INC AbX;  _______ |]

/// Maps an byte encoded opcode into a CPU instruction
let interpret (opcode:byte) = 
  hex opcode |> printfn "opcode: %s"  
  InstructionSet.[int opcode]
  
/// Tick's the CPU through one cycle - reading byte's from memory to perform 
/// CPU instructions
let cycle() = 
  printfn "- CPU CYCLE -------------------------------- BEGIN"  
  take() 
  |> interpret 
  |> exec  
  printfn "- CPU CYCLE ---------------------------------- END"