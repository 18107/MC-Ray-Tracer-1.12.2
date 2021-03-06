package core.id107.raytracer.coretransform.classtransformers;

import static org.objectweb.asm.Opcodes.ALOAD;
import static org.objectweb.asm.Opcodes.INVOKESTATIC;

import org.objectweb.asm.Type;
import org.objectweb.asm.tree.ClassNode;
import org.objectweb.asm.tree.InsnList;
import org.objectweb.asm.tree.MethodInsnNode;
import org.objectweb.asm.tree.MethodNode;
import org.objectweb.asm.tree.VarInsnNode;

import core.id107.raytracer.coretransform.CLTLog;
import core.id107.raytracer.coretransform.TransformerUtil;
import core.id107.raytracer.coretransform.classtransformers.name.ClassName;
import core.id107.raytracer.coretransform.classtransformers.name.MethodName;
import core.id107.raytracer.coretransform.classtransformers.name.Names;
import net.minecraft.client.multiplayer.WorldClient;

public class MinecraftTransformer extends ClassTransformer {

	@Override
	public ClassName getClassName() {
		return Names.Minecraft;
	}
	
	@Override
	public MethodTransformer[] getMethodTransformers() {
		MethodTransformer loadWorldTransformer = new MethodTransformer() {
			
			@Override
			public MethodName getMethodName() {
				return Names.Minecraft_loadWorld;
			}
			
			@Override
			public void transform(ClassNode classNode, MethodNode method, boolean obfuscated) {
				CLTLog.info("Found method: " + method.name + " " + method.desc);
				CLTLog.info("begining at start of method " + getMethodName().debug());
				
				//TransformerUtil.onWorldLoad(WorldClient worldClientIn)
				InsnList toInsert = new InsnList();
				toInsert.add(new VarInsnNode(ALOAD, 1)); //worldClientIn
				toInsert.add(new MethodInsnNode(INVOKESTATIC, Type.getInternalName(TransformerUtil.class),
						"onWorldLoad", "(L" + Type.getInternalName(WorldClient.class) + ";)V", false));
				method.instructions.insertBefore(method.instructions.getFirst(), toInsert);
			}
		};
		
		return new MethodTransformer[] {loadWorldTransformer};
	}
}
