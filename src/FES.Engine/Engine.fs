namespace FES.Engine

open System.IO
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics


type EmulatorLoop() as this =
  inherit Game()
  
  let mutable gdm = Unchecked.defaultof<GraphicsDeviceManager>
  let mutable sb = Unchecked.defaultof<SpriteBatch>

  do
    gdm <- new GraphicsDeviceManager(this)
    this.Content.RootDirectory <- "Content"
  
  override this.Initialize() =     
    File.ReadAllBytes("test.nes")
    |> ROM.load    
  
  override this.LoadContent() =
    sb <- new SpriteBatch(this.GraphicsDevice)
  
  override this.Update(time) = 
    CPU.cycle()
  
  override this.Draw(time) = ()