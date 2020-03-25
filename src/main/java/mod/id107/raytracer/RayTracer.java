package mod.id107.raytracer;

import net.minecraftforge.common.ForgeVersion;
import net.minecraftforge.common.MinecraftForge;
import net.minecraftforge.fml.common.Mod;
import net.minecraftforge.fml.common.Mod.EventHandler;
import net.minecraftforge.fml.common.event.FMLInitializationEvent;

@Mod(modid = RayTracer.MOD_ID, name = RayTracer.MOD_NAME, version = RayTracer.MOD_VERSION, useMetadata = true)
public class RayTracer {

	public static final String MOD_ID = "raytracer";
    public static final String MOD_NAME = "Ray Tracer";
    public static final String MOD_VERSION = "0.0.0";
    
    public static final String RESOURCE_PREFIX = MOD_ID.toLowerCase() + ':';
    
    @EventHandler
    public void init(FMLInitializationEvent event)
    {
        Log.info("" + ForgeVersion.mcVersion);
        MinecraftForge.EVENT_BUS.register(new mod.id107.raytracer.RTEventHandler());
    }
}
