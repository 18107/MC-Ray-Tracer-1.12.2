package core.id107.raytracer.coretransform.classtransformers.name;

public class Names {

	public static final ClassName Chunk = new ClassName("net.minecraft.world.chunk.Chunk", "axw");
	public static final MethodName Chunk_getBlockState = new MethodName("setBlockState", "func_177436_a", "a", "(Lnet/minecraft/util/math/BlockPos;Lnet/minecraft/block/state/IBlockState;)Lnet/minecraft/block/state/IBlockState;", "(Let;Lawt;)Lawt;");
	
	public static final ClassName EntityRenderer = new ClassName("net.minecraft.client.renderer.EntityRenderer", "buq");
	public static final MethodName EntityRenderer_renderWorldPass = new MethodName("renderWorldPass", "func_175068_a", "a", "(IFJ)V", "(IFJ)V");
	public static final MethodName EntityRenderer_updateCameraAndRender = new MethodName("updateCameraAndRender", "func_181560_a", "a", "(FJ)V", "(FJ)V");
	
	public static final ClassName Minecraft = new ClassName("net.minecraft.client.Minecraft", "bib");
	public static final MethodName Minecraft_loadWorld = new MethodName("loadWorld", "func_71353_a", "a", "(Lnet/minecraft/client/multiplayer/WorldClient;Ljava/lang/String;)V", "(Lbsb;Ljava/lang/String;)V");
}
