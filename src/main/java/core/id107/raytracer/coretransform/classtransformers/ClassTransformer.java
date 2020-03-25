package core.id107.raytracer.coretransform.classtransformers;

import org.objectweb.asm.tree.ClassNode;
import org.objectweb.asm.tree.MethodNode;

import core.id107.raytracer.coretransform.classtransformers.name.ClassName;
import core.id107.raytracer.coretransform.classtransformers.name.MethodName;

public abstract class ClassTransformer {

	private static ClassTransformer[] transformers;
	
	static {
		//Put all of the class transformers here
		transformers = new ClassTransformer[] {new ChunkTransformer(), new MinecraftTransformer()};
	}
	
	public abstract ClassName getClassName();
	
	public abstract MethodTransformer[] getMethodTransformers();
	
	public static ClassTransformer[] getClassTransformers() {
		return transformers;
	}
	
	public static abstract class MethodTransformer {
		public abstract void transform(ClassNode classNode, MethodNode method, boolean obfuscated);
		public abstract MethodName getMethodName();
	}
}
