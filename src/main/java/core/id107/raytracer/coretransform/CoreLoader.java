package core.id107.raytracer.coretransform;

import java.util.Map;

import net.minecraftforge.fml.common.Mod;
import net.minecraftforge.fml.relauncher.IFMLLoadingPlugin;

@IFMLLoadingPlugin.MCVersion("1.12.2")
@IFMLLoadingPlugin.TransformerExclusions(value = "core.id107.raytracer.coretransform.")
@IFMLLoadingPlugin.Name(CoreLoader.NAME)
@IFMLLoadingPlugin.SortingIndex(value = 999)
public class CoreLoader implements IFMLLoadingPlugin {

	public static final String NAME = "Ray Tracer";
	public static boolean isObfuscated = false;
	
	@Override
	public String[] getASMTransformerClass() {
		return new String[]{CoreTransformer.class.getName()};
	}
	
	@Override
	public String getModContainerClass() {
		return null;
	}
	
	@Override
	public String getSetupClass() {
		return null;
	}
	
	@Override
	public void injectData(Map<String, Object> data) {
		isObfuscated = (Boolean) data.get("runtimeDeobfuscationEnabled");
	}
	
	@Override
	public String getAccessTransformerClass() {
		return null;
	}
}
