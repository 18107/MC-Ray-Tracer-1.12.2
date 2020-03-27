package core.id107.raytracer.coretransform.classtransformers;

import org.objectweb.asm.Opcodes;
import org.objectweb.asm.Type;
import org.objectweb.asm.tree.AbstractInsnNode;
import org.objectweb.asm.tree.ClassNode;
import org.objectweb.asm.tree.FieldInsnNode;
import org.objectweb.asm.tree.InsnList;
import org.objectweb.asm.tree.JumpInsnNode;
import org.objectweb.asm.tree.LabelNode;
import org.objectweb.asm.tree.LdcInsnNode;
import org.objectweb.asm.tree.MethodInsnNode;
import org.objectweb.asm.tree.MethodNode;

import core.id107.raytracer.coretransform.CLTLog;
import core.id107.raytracer.coretransform.TransformerUtil;
import core.id107.raytracer.coretransform.classtransformers.name.ClassName;
import core.id107.raytracer.coretransform.classtransformers.name.MethodName;
import core.id107.raytracer.coretransform.classtransformers.name.Names;

public class EntityRendererTransformer extends ClassTransformer {

	@Override
	public ClassName getClassName() {
		return Names.EntityRenderer;
	}

	@Override
	public MethodTransformer[] getMethodTransformers() {
		MethodTransformer updateCameraAndRenderTransformer = new MethodTransformer() {
			
			@Override
			public MethodName getMethodName() {
				return Names.EntityRenderer_renderWorldPass;
			}
			
			//Disable rendering terrain
			@Override
			public void transform(ClassNode classNode, MethodNode method, boolean obfuscated) {
				CLTLog.info("Found method: " + method.name + " " + method.desc);
				
				//Change if (this.mc.world != null) to if (this.mc.world != null && TransformerUtil.doRasterRender)
				for (AbstractInsnNode instruction : method.instructions.toArray()) {
					if (instruction.getOpcode() == Opcodes.LDC) {
						if ("terrain".equals(((LdcInsnNode)instruction).cst)) {
							CLTLog.info("Found LDC \"terrain\"");
							
							LabelNode label = new LabelNode();
							instruction = instruction.getNext();
							InsnList toInsert = new InsnList();
							
							toInsert.add(new FieldInsnNode(Opcodes.GETSTATIC, Type.getInternalName(TransformerUtil.class),
									"doRasterRender", "Z"));
							toInsert.add(new JumpInsnNode(Opcodes.IFEQ, label));
							method.instructions.insert(instruction, toInsert);
							
							//Insert label before if (flag && this.mc.objectMouseOver != null && !entity.isInsideOfMaterial(Material.WATER))
							int pop = 0;
							while (true) {
								if (instruction instanceof MethodInsnNode) {
									MethodInsnNode mInsn = (MethodInsnNode)instruction;
									if (mInsn.name.equals("popMatrix")) {
										pop++;
										if (pop == 2) {
											method.instructions.insert(instruction, label);
											break;
										}
									}
								}
								instruction = instruction.getNext();
								
							}
							break;
						}
					}
				}
			}
		};
		
		return new MethodTransformer[] {updateCameraAndRenderTransformer};
	}

}
