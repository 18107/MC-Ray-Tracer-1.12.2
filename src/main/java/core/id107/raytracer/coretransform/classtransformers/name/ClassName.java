package core.id107.raytracer.coretransform.classtransformers.name;

import core.id107.raytracer.coretransform.CoreLoader;

public class ClassName {

	private final String deobfuscatedName;
	private final String obfuscatedName;
	
	public ClassName(String deobfuscatedName, String obfuscatedName) {
		this.deobfuscatedName = deobfuscatedName;
		this.obfuscatedName = obfuscatedName;
	}
	
	public String getName() {
		return getName(CoreLoader.isObfuscated);
	}
	
	public String getName(boolean obfuscated) {
		if (obfuscated) {
			return obfuscatedName;
		} else {
			return deobfuscatedName;
		}
	}
	
	public String getInternalName() {
		return getInternalName(CoreLoader.isObfuscated);
	}
	
	public String getInternalName(boolean obfuscated) {
		if (obfuscated) {
			return obfuscatedName;
		} else {
			return deobfuscatedName.replace('.', '/');
		}
	}
	
	public String getNameAsDesc() {
		return getNameAsDesc(CoreLoader.isObfuscated);
	}
	
	public String getNameAsDesc(boolean obfuscated) {
		return "L" + getInternalName(obfuscated) + ";";
	}
	
	public String all() {
		return deobfuscatedName + "   " + obfuscatedName;
	}
}
