package mod.id107.raytracer;

import org.lwjgl.input.Keyboard;
import org.lwjgl.opengl.Display;
import org.lwjgl.opengl.GL11;
import org.lwjgl.opengl.GL15;
import org.lwjgl.opengl.GL20;

import mod.id107.raytracer.gui.RTSettings;
import mod.id107.raytracer.world.WorldLoader;
import net.minecraft.client.Minecraft;
import net.minecraft.entity.Entity;

public class RenderUtil {

	private static Shader shader;
	public static WorldLoader worldLoader;
	public static boolean pauseRendering = false;
	
	public static void createShader() {
		if (shader == null) {
			shader = new Shader();
			shader.createShaderProgram();
			if (worldLoader == null) {
				worldLoader = new WorldLoader();
			}
		}
	}
	
	public static void destroyShader() {
		if (shader != null) {
			worldLoader = null;
			shader.deleteShaderProgram();
			shader = null;
		}
	}
	
	public static void runShader() {
		Minecraft mc = Minecraft.getMinecraft();
		
		//TODO remove
		if (Keyboard.isKeyDown(Keyboard.KEY_NUMPAD5)) {
			destroyShader();
			createShader();
		}
		
		//Use shader program
		GL20.glUseProgram(shader.getShaderProgram());
		
		//TODO third person view
		Entity entity = mc.getRenderViewEntity();
		float partialTicks = mc.getRenderPartialTicks();
		double entityPosX = entity.lastTickPosX + (entity.posX - entity.lastTickPosX) * (double)partialTicks;
        double entityPosY = entity.lastTickPosY + entity.getEyeHeight() + (entity.posY - entity.lastTickPosY) * (double)partialTicks;
        double entityPosZ = entity.lastTickPosZ + (entity.posZ - entity.lastTickPosZ) * (double)partialTicks;
        float fov = (float) Math.toRadians(mc.entityRenderer.getFOVModifier(partialTicks, true));
		
		//Set uniform values
		int texUniform = GL20.glGetUniformLocation(shader.getShaderProgram(), "tex");
		GL20.glUniform1i(texUniform, 0);
		int cameraPosUniform = GL20.glGetUniformLocation(shader.getShaderProgram(), "cameraPos");
		GL20.glUniform3f(cameraPosUniform, (float)entityPosX%16, (float)entityPosY%16, (float)entityPosZ%16);
		int cameraDirUniform = GL20.glGetUniformLocation(shader.getShaderProgram(), "cameraDir");
		GL20.glUniform3f(cameraDirUniform, -(float)Math.toRadians(entity.rotationPitch), (float)Math.toRadians(180+entity.rotationYaw), 0);
		int fovyUniform = GL20.glGetUniformLocation(shader.getShaderProgram(), "fovy");
		GL20.glUniform1f(fovyUniform, fov);
		int fovxUniform = GL20.glGetUniformLocation(shader.getShaderProgram(), "fovx");
		GL20.glUniform1f(fovxUniform, fov*Display.getWidth()/(float)Display.getHeight());
		int sphericalUniform = GL20.glGetUniformLocation(shader.getShaderProgram(), "spherical");
		GL20.glUniform1i(sphericalUniform, RTSettings.spherical ? 1 : 0);
		int stereoscopicUniform = GL20.glGetUniformLocation(shader.getShaderProgram(), "stereoscopic3d");
		GL20.glUniform1i(stereoscopicUniform, RTSettings.stereoscopic ? 1 : 0);
		int eyeWidthUniform = GL20.glGetUniformLocation(shader.getShaderProgram(), "eyeWidth");
		GL20.glUniform1f(eyeWidthUniform, 0.063f); //TODO input eyeWidth option
		
		if (!pauseRendering) {
			if (worldLoader == null) {
				worldLoader = new WorldLoader();
			}
			if (worldLoader.dimension != mc.world.provider.getDimension()) {
				worldLoader.dimension = mc.world.provider.getDimension();
			}
			worldLoader.updateWorld(entityPosX, entityPosY, entityPosZ, shader);

			//Setup view
			GL11.glMatrixMode(GL11.GL_PROJECTION);
			GL11.glPushMatrix();
			GL11.glLoadIdentity();
			GL11.glOrtho(-1, 1, -1, 1, -1, 1);
			GL11.glMatrixMode(GL11.GL_MODELVIEW);
			GL11.glPushMatrix();
			GL11.glLoadIdentity();

			//Bind vbo and texture
			GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, shader.getVbo());
			GL20.glEnableVertexAttribArray(0);
			GL20.glVertexAttribPointer(0, 2, GL11.GL_BYTE, false, 0, 0L);
			GL11.glBindTexture(GL11.GL_TEXTURE_2D, 8);

			//Render
			GL11.glDrawArrays(GL11.GL_TRIANGLES, 0, 6);

			//Reset vbo and texture
			GL11.glBindTexture(GL11.GL_TEXTURE_2D, 0);
			GL20.glDisableVertexAttribArray(0);
			GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, 0);

			//Reset view
			GL11.glMatrixMode(GL11.GL_PROJECTION);
			GL11.glPopMatrix();
			GL11.glMatrixMode(GL11.GL_MODELVIEW);
			GL11.glPopMatrix();
		}
		
		//Stop using shader program
		GL20.glUseProgram(0);
	}
}
