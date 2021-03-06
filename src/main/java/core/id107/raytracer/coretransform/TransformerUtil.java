package core.id107.raytracer.coretransform;

import core.id107.raytracer.RTEvent;
import net.minecraft.client.multiplayer.WorldClient;
import net.minecraft.world.chunk.Chunk;
import net.minecraftforge.common.MinecraftForge;

public class TransformerUtil {

	public static boolean doRasterRender = false;
	
	/**
	 * Called from asm modified code:
	 * {@link net.minecraft.client.Minecraft#loadWorld() loadWorld}
	 */
	public static void onWorldLoad(WorldClient worldClient) {
		if (worldClient != null) {
			MinecraftForge.EVENT_BUS.post(new RTEvent.WorldLoadEvent.Load());
		} else {
			MinecraftForge.EVENT_BUS.post(new RTEvent.WorldLoadEvent.UnLoad());
		}
	}
	
	/**
	 * Called from asm modified code:
	 * {@link net.minecraft.world.chunk.Chunk#setBlockState() setBlockState}
	 */
	public static void onChunkModified(Chunk chunk) {
		MinecraftForge.EVENT_BUS.post(new RTEvent.ChunkModifiedEvent(chunk));
	}
}
