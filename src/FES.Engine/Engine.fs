namespace FES.Engine

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics


type EmulatorLoop() as this =
  inherit Game()
  
  let mutable gdm = Unchecked.defaultof<GraphicsDeviceManager>
  let mutable sb = Unchecked.defaultof<SpriteBatch>

  do
    gdm <- new GraphicsDeviceManager(this)
    this.Content.RootDirectory <- "Content"
  
  override this.Initialize() = ()
  
  override this.LoadContent() =
    sb <- new SpriteBatch(this.GraphicsDevice)
  
  override this.Update(time) = ()
  
  override this.Draw(time) = ()