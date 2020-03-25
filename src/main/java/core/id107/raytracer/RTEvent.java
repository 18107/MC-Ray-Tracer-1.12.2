package core.id107.raytracer;

import net.minecraft.world.chunk.Chunk;
import net.minecraftforge.fml.common.eventhandler.Event;

public class RTEvent extends Event {

	public static class WorldLoadEvent extends RTEvent {
		public static class Load extends WorldLoadEvent {
			
		}
		
		public static class UnLoad extends WorldLoadEvent {
			
		}
	}
	
	public static class ChunkModifiedEvent extends RTEvent {
		private final Chunk chunk;
		
		public ChunkModifiedEvent(Chunk chunk) {
			this.chunk = chunk;
		}
		
		public Chunk getChunk() {
			return this.chunk;
		}
	}
}
