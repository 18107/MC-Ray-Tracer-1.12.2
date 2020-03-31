#version 430

#define M_PI 3.141592

#define CHUNK_SIZE 16*16*16

#define CHUNK_WIDTH 16 //must be a power of 2

#define WORLD_HEIGHT_CHUNKS 16

#define TEXTURE_RESOLUTION 16

const mat4 rotation[48] = mat4[](
  mat4(1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1),
  mat4(-1,0,0,1, 0,1,0,0, 0,0,1,0, 0,0,0,1),
  mat4(0,-1,0,1, 1,0,0,0, 0,0,1,0, 0,0,0,1),
  mat4(0,1,0,0, 1,0,0,0, 0,0,1,0, 0,0,0,1),
  mat4(0,1,0,0, -1,0,0,1, 0,0,1,0, 0,0,0,1),
  mat4(0,-1,0,1, -1,0,0,1, 0,0,1,0, 0,0,0,1),
  mat4(-1,0,0,1, 0,-1,0,1, 0,0,1,0, 0,0,0,1),
  mat4(1,0,0,0, 0,-1,0,1, 0,0,1,0, 0,0,0,1),
  mat4(0,0,-1,1, 0,1,0,0, 1,0,0,0, 0,0,0,1),
  mat4(0,0,1,0, 0,1,0,0, 1,0,0,0, 0,0,0,1),
  mat4(0,-1,0,1, 0,0,-1,1, 1,0,0,0, 0,0,0,1),
  mat4(0,1,0,0, 0,0,-1,1, 1,0,0,0, 0,0,0,1),
  mat4(0,1,0,0, 0,0,1,0, 1,0,0,0, 0,0,0,1),
  mat4(0,-1,0,1, 0,0,1,0, 1,0,0,0, 0,0,0,1),
  mat4(0,0,1,0, 0,-1,0,1, 1,0,0,0, 0,0,0,1),
  mat4(0,0,-1,1, 0,-1,0,1, 1,0,0,0, 0,0,0,1),
  mat4(0,0,1,0, 0,1,0,0, -1,0,0,1, 0,0,0,1),
  mat4(0,0,-1,1, 0,1,0,0, -1,0,0,1, 0,0,0,1),
  mat4(0,-1,0,1, 0,0,1,0, -1,0,0,1, 0,0,0,1),
  mat4(0,1,0,0, 0,0,1,0, -1,0,0,1, 0,0,0,1),
  mat4(0,1,0,0, 0,0,-1,1, -1,0,0,1, 0,0,0,1),
  mat4(0,-1,0,1, 0,0,-1,1, -1,0,0,1, 0,0,0,1),
  mat4(0,0,-1,1, 0,-1,0,1, -1,0,0,1, 0,0,0,1),
  mat4(0,0,1,0, 0,-1,0,1, -1,0,0,1, 0,0,0,1),
  mat4(-1,0,0,1, 0,1,0,0, 0,0,-1,1, 0,0,0,1),
  mat4(1,0,0,0, 0,1,0,0, 0,0,-1,1, 0,0,0,1),
  mat4(0,-1,0,1, -1,0,0,1, 0,0,-1,1, 0,0,0,1),
  mat4(0,1,0,0, -1,0,0,1, 0,0,-1,1, 0,0,0,1),
  mat4(0,1,0,0, 1,0,0,0, 0,0,-1,1, 0,0,0,1),
  mat4(0,-1,0,1, 1,0,0,0, 0,0,-1,1, 0,0,0,1),
  mat4(1,0,0,0, 0,-1,0,1, 0,0,-1,1, 0,0,0,1),
  mat4(-1,0,0,1, 0,-1,0,1, 0,0,-1,1, 0,0,0,1),
  mat4(1,0,0,0, 0,0,-1,1, 0,1,0,0, 0,0,0,1),
  mat4(-1,0,0,1, 0,0,-1,1, 0,1,0,0, 0,0,0,1),
  mat4(0,0,1,0, 1,0,0,0, 0,1,0,0, 0,0,0,1),
  mat4(0,0,-1,1, 1,0,0,0, 0,1,0,0, 0,0,0,1),
  mat4(0,0,-1,1, -1,0,0,1, 0,1,0,0, 0,0,0,1),
  mat4(0,0,1,0, -1,0,0,1, 0,1,0,0, 0,0,0,1),
  mat4(-1,0,0,1, 0,0,1,0, 0,1,0,0, 0,0,0,1),
  mat4(1,0,0,0, 0,0,1,0, 0,1,0,0, 0,0,0,1),
  mat4(1,0,0,0, 0,0,1,0, 0,-1,0,1, 0,0,0,1),
  mat4(-1,0,0,1, 0,0,1,0, 0,-1,0,1, 0,0,0,1),
  mat4(0,0,-1,1, 1,0,0,0, 0,-1,0,1, 0,0,0,1),
  mat4(0,0,1,0, 1,0,0,0, 0,-1,0,1, 0,0,0,1),
  mat4(0,0,1,0, -1,0,0,1, 0,-1,0,1, 0,0,0,1),
  mat4(0,0,-1,1, -1,0,0,1, 0,-1,0,1, 0,0,0,1),
  mat4(-1,0,0,1, 0,0,-1,1, 0,-1,0,1, 0,0,0,1),
  mat4(1,0,0,0, 0,0,-1,1, 0,-1,0,1, 0,0,0,1)
);

in vec2 texcoord;

uniform vec3 cameraPos;
uniform vec3 cameraDir;
uniform float fovx;
uniform float fovy;
uniform int renderDistance;
uniform int chunkHeight;
uniform bool spherical;
uniform bool stereoscopic3d;
uniform float eyeWidth;

uniform sampler2D tex;

out vec4 color;

struct BlockData {
  int id;
  int rotation;
  int lightLevel;
  int unused;
};

layout(std430, binding = 2) buffer worldChunks
{
  int location[];
};

layout(std430, binding = 3) buffer chunk
{
  BlockData blockData[];
};

layout(std430, binding = 4) buffer iddata
{
  int idData[];
};

layout(std430, binding = 5) buffer voxelData
{
  vec4 voxelColor[];
};

layout(std430, binding = 6) buffer textureData
{
  vec4 textureColor[];
};

vec3 rotate(vec3 camera, vec3 ray) {
  //rotate z
  float x = cos(camera.z)*ray.x + sin(camera.z)*ray.y;
  float y = cos(camera.z)*ray.y - sin(camera.z)*ray.x;
  ray.x = x;
  ray.y = y;

  //rotate x
  y = cos(camera.x)*ray.y - sin(camera.x)*ray.z;
  float z = cos(camera.x)*ray.z + sin(camera.x)*ray.y;
  ray.y = y;
  ray.z = z;

  //rotate y
  x = cos(camera.y)*ray.x - sin(camera.y)*ray.z;
  z = cos(camera.y)*ray.z + sin(camera.y)*ray.x;
  ray.x = x;
  ray.z = z;

  return ray;
}

//Gets the direction of the ray assuming a flat screen
vec3 getRayFlat(float fovx, float fovy, bool stereoscopic) {
  vec3 ray;
  if (stereoscopic) {
    if (texcoord.x < 0.5) {
      ray = vec3((-1+texcoord.x*4)*tan(fovy/2)*fovx/fovy, (-1+texcoord.y*2)*tan(fovy/2), -1);
    } else {
      ray = vec3((-3+texcoord.x*4)*tan(fovy/2)*fovx/fovy, (-1+texcoord.y*2)*tan(fovy/2), -1);
    }
  } else {
    ray = vec3((-1+texcoord.x*2)*tan(fovy/2)*fovx/fovy, (-1+texcoord.y*2)*tan(fovy/2), -1);
  }
  return rotate(cameraDir, ray);
}

//Gets the direction of the ray assuming a spherical screen
vec3 getRaySphere(float fovx, float fovy, bool stereoscopic) {
  vec3 ray;
  if (stereoscopic) {
    if (texcoord.y < 0.5) {
      ray = vec3(-sin((texcoord.x)*fovx)*sin((texcoord.y*2)*fovy),
        -cos((texcoord.y*2)*fovy),
        cos((texcoord.x)*fovx)*sin((texcoord.y*2)*fovy));
    } else {
      ray = vec3(-sin((texcoord.x)*fovx)*sin((texcoord.y*2-1)*fovy),
        -cos((texcoord.y*2-1)*fovy),
        cos((texcoord.x)*fovx)*sin((texcoord.y*2-1)*fovy));
      }
  } else {
    ray = vec3(-sin((texcoord.x)*fovx)*sin((texcoord.y)*fovy),
      -cos((texcoord.y)*fovy),
      cos((texcoord.x)*fovx)*sin((texcoord.y)*fovy));
  }
  return rotate(cameraDir, ray);
}

bool traceBlock(int id, vec3 nearestVoxel, ivec4 currentVoxel, mat4 rotate, vec3 vinc, ivec3 iinc) {
  ivec4 rotated;
  while (true) { //TODO condition
    if (nearestVoxel.x < nearestVoxel.y) {
      if (nearestVoxel.x < nearestVoxel.z) {
        nearestVoxel.x += vinc.x;
        currentVoxel.x += iinc.x;
        if (currentVoxel.x < 0 || currentVoxel.x >= TEXTURE_RESOLUTION) {
          return false;
        }
      } else {
        nearestVoxel.z += vinc.z;
        currentVoxel.z += iinc.z;
        if (currentVoxel.z < 0 || currentVoxel.z >= TEXTURE_RESOLUTION) {
          return false;
        }
      }
    } else {
      if (nearestVoxel.y < nearestVoxel.z) {
        nearestVoxel.y += vinc.y;
        currentVoxel.y += iinc.y;
        if (currentVoxel.y < 0 || currentVoxel.y >= TEXTURE_RESOLUTION) {
          return false;
        }
      } else {
        nearestVoxel.z += vinc.z;
        currentVoxel.z += iinc.z;
        if (currentVoxel.z < 0 || currentVoxel.z >= TEXTURE_RESOLUTION) {
          return false;
        }
      }
    }
    rotated = ivec4(currentVoxel*rotate);
    color = voxelColor[id*TEXTURE_RESOLUTION*TEXTURE_RESOLUTION*TEXTURE_RESOLUTION +
        rotated.z*TEXTURE_RESOLUTION*TEXTURE_RESOLUTION +
        rotated.y*TEXTURE_RESOLUTION + rotated.x];
    if (color.a >= 0.5) {
      return true;
    }
  }
}

bool setupTraceFirstBlock(int blockId, vec3 inc, ivec3 iinc, vec3 point, int blockRotation) {
  ivec4 currentVoxel = ivec4(floor(mod(point, 1)*TEXTURE_RESOLUTION), TEXTURE_RESOLUTION-1);

  vec3 dist;
  if (iinc.x < 0) {
    dist.x = abs(floor(point.x*TEXTURE_RESOLUTION)/TEXTURE_RESOLUTION - point.x);
  } else {
    dist.x = abs(ceil(point.x*TEXTURE_RESOLUTION)/TEXTURE_RESOLUTION - point.x);
  }
  if (iinc.y < 0) {
    dist.y = abs(floor(point.y*TEXTURE_RESOLUTION)/TEXTURE_RESOLUTION - point.y);
  } else {
    dist.y = abs(ceil(point.y*TEXTURE_RESOLUTION)/TEXTURE_RESOLUTION - point.y);
  }
  if (iinc.z < 0) {
    dist.z = abs(floor(point.z*TEXTURE_RESOLUTION)/TEXTURE_RESOLUTION - point.z);
  } else {
    dist.z = abs(ceil(point.z*TEXTURE_RESOLUTION)/TEXTURE_RESOLUTION - point.z);
  }

  vec3 nearestVoxel = abs(dist*inc);

  vec3 nearestSide = abs(mod(point, 1) - 0.5);
  int side;
  ivec4 sideVector;
  if (nearestSide.x > nearestSide.y) {
    if (nearestSide.x > nearestSide.z) {
      if (mod(point.x, 1) > 0.5) {
        side = 0;
      } else {
        side = 1;
      }
    } else {
      if (mod(point.z, 1) > 0.5) {
        side = 4;
      } else {
        side = 5;
      }
    }
  } else {
    if (nearestSide.y > nearestSide.z) {
      if (mod(point.y, 1) > 0.5) {
        side = 2;
      } else {
        side = 3;
      }
    } else {
      if (mod(point.z, 1) > 0.5) {
        side = 4;
      } else {
        side = 5;
      }
    }
  }

  bool isVoxel = bool(idData[blockId*6*3 + side*3]);
  if (isVoxel) {
    int voxId = idData[blockId*6*3 + side*3 + 1];
    int iRotate = idData[blockId*6*3 + side*3 + 2];
    mat4 rotate = rotation[blockRotation]*rotation[iRotate];

    //trace voxel
    return traceBlock(voxId, nearestVoxel, currentVoxel, rotate, inc/TEXTURE_RESOLUTION, iinc);
  } else {
    return false;
  }
}

bool setupTraceBlock(int id, vec3 nearestCube, vec3 inc, ivec3 iinc, ivec3 current, ivec3 last, mat4 rotate) {
  vec3 nearestVoxel;
  ivec4 currentVoxel;

  //get current location
  if (current.x != last.x) {
    float plane = nearestCube.x - inc.x;
    float distanceY = (nearestCube.y - plane)/inc.y;
    float distanceZ = (nearestCube.z - plane)/inc.z;
    currentVoxel.y = int(floor(TEXTURE_RESOLUTION*((iinc.y+1)/2) - distanceY*iinc.y*TEXTURE_RESOLUTION));
    currentVoxel.z = int(floor(TEXTURE_RESOLUTION*((iinc.z+1)/2) - distanceZ*iinc.z*TEXTURE_RESOLUTION));
    currentVoxel.x = (TEXTURE_RESOLUTION-1)*((-iinc.x+1)/2);
    currentVoxel.w = (TEXTURE_RESOLUTION-1);

    nearestVoxel.x = plane + inc.x/TEXTURE_RESOLUTION;
    nearestVoxel.y = nearestCube.y - inc.y*floor(distanceY*TEXTURE_RESOLUTION)/TEXTURE_RESOLUTION;
    nearestVoxel.z = nearestCube.z - inc.z*floor(distanceZ*TEXTURE_RESOLUTION)/TEXTURE_RESOLUTION;
  }
  else if (current.z != last.z) {
    float plane = nearestCube.z - inc.z;
    float distanceX = (nearestCube.x - plane)/inc.x;
    float distanceY = (nearestCube.y - plane)/inc.y;
    currentVoxel.x = int(floor(TEXTURE_RESOLUTION*((iinc.x+1)/2) - distanceX*iinc.x*TEXTURE_RESOLUTION));
    currentVoxel.y = int(floor(TEXTURE_RESOLUTION*((iinc.y+1)/2) - distanceY*iinc.y*TEXTURE_RESOLUTION));
    currentVoxel.z = (TEXTURE_RESOLUTION-1)*((-iinc.z+1)/2);
    currentVoxel.w = (TEXTURE_RESOLUTION-1);

    nearestVoxel.z = plane + inc.z/TEXTURE_RESOLUTION;
    nearestVoxel.x = nearestCube.x - inc.x*floor(distanceX*TEXTURE_RESOLUTION)/TEXTURE_RESOLUTION;
    nearestVoxel.y = nearestCube.y - inc.y*floor(distanceY*TEXTURE_RESOLUTION)/TEXTURE_RESOLUTION;
  }
  else { //if (current.y != last.y)
    float plane = nearestCube.y - inc.y;
    float distanceX = (nearestCube.x - plane)/inc.x;
    float distanceZ = (nearestCube.z - plane)/inc.z;
    currentVoxel.x = int(floor(TEXTURE_RESOLUTION*((iinc.x+1)/2) - distanceX*iinc.x*TEXTURE_RESOLUTION));
    currentVoxel.z = int(floor(TEXTURE_RESOLUTION*((iinc.z+1)/2) - distanceZ*iinc.z*TEXTURE_RESOLUTION));
    currentVoxel.y = (TEXTURE_RESOLUTION-1)*((-iinc.y+1)/2);
    currentVoxel.w = (TEXTURE_RESOLUTION-1);

    nearestVoxel.y = plane + inc.y/TEXTURE_RESOLUTION;
    nearestVoxel.x = nearestCube.x - inc.x*floor(distanceX*TEXTURE_RESOLUTION)/TEXTURE_RESOLUTION;
    nearestVoxel.z = nearestCube.z - inc.z*floor(distanceZ*TEXTURE_RESOLUTION)/TEXTURE_RESOLUTION;
  }

  ivec4 rotated = ivec4(currentVoxel*rotate);

  color = voxelColor[id*TEXTURE_RESOLUTION*TEXTURE_RESOLUTION*TEXTURE_RESOLUTION +
      rotated.z*TEXTURE_RESOLUTION*TEXTURE_RESOLUTION +
      rotated.y*TEXTURE_RESOLUTION + rotated.x];
  if (color.a >= 0.5) {
    return true;
  }

  //trace voxel
  return traceBlock(id, nearestVoxel, currentVoxel, rotate, inc/TEXTURE_RESOLUTION, iinc);
}

bool drawTexture(int blockId, int side, vec4 texVector, int blockRotation, int light) {
  float texX;
  float texY;
  int texId = idData[blockId*6*3 + side*3 + 1];

  texVector = texVector*rotation[blockRotation];
  switch (side) {
  case 0:
    texX = 1-texVector.z;
    texY = texVector.y;
    break;
  case 1:
    texX = texVector.z;
    texY = texVector.y;
    break;
  case 2:
    texX = 1-texVector.z;
    texY = texVector.x;
    break;
  case 3:
    texX = texVector.z;
    texY = texVector.x;
    break;
  case 4:
    texX = texVector.x;
    texY = texVector.y;
    break;
  case 5:
    texX = 1-texVector.x;
    texY = texVector.y;
    break;
  }

  color = textureColor[texId*TEXTURE_RESOLUTION*TEXTURE_RESOLUTION +
    int(floor(texY*TEXTURE_RESOLUTION))*TEXTURE_RESOLUTION +
    int(floor(texX*TEXTURE_RESOLUTION))] *
    vec4((light+1)/16.0, (light+1)/16.0, (light+1)/16.0, 1);
  if (color.a < 0.5) {
    return false;
  }
  return true;
}

int nextCube(inout vec3 nearestCube, inout ivec3 current, inout ivec3 chunkPos,
  inout int chunkId, inout int lastChunkId, vec3 inc, ivec3 iinc, int worldWidth) {
  lastChunkId = chunkId;

  if (nearestCube.x < nearestCube.y) {
    if (nearestCube.x < nearestCube.z) {
      nearestCube.x += inc.x;
      current.x += iinc.x;
      if ((current.x%CHUNK_WIDTH) != current.x) {
        current.x %= CHUNK_WIDTH;
        chunkPos.x += iinc.x;
        if (chunkPos.x == worldWidth || chunkPos.x == -1) {
          discard;
          return -1;
        }
        chunkId = location[chunkPos.z*worldWidth*WORLD_HEIGHT_CHUNKS + chunkPos.x*WORLD_HEIGHT_CHUNKS + chunkPos.y];
      }
    } else {
      nearestCube.z += inc.z;
      current.z += iinc.z;
      if ((current.z%CHUNK_WIDTH) != current.z) {
        current.z %= CHUNK_WIDTH;
        chunkPos.z += iinc.z;
        if (chunkPos.z == worldWidth || chunkPos.z == -1) {
          discard;
          return -1;
        }
        chunkId = location[chunkPos.z*worldWidth*WORLD_HEIGHT_CHUNKS + chunkPos.x*WORLD_HEIGHT_CHUNKS + chunkPos.y];
      }
    }
  } else {
    if (nearestCube.y < nearestCube.z) {
      nearestCube.y += inc.y;
      current.y += iinc.y;
      if ((current.y%CHUNK_WIDTH) != current.y) {
        current.y %= CHUNK_WIDTH;
        chunkPos.y += iinc.y;
        if (chunkPos.y == WORLD_HEIGHT_CHUNKS || chunkPos.y == -1) {
          discard;
          return -1;
        }
        chunkId = location[chunkPos.z*worldWidth*WORLD_HEIGHT_CHUNKS + chunkPos.x*WORLD_HEIGHT_CHUNKS + chunkPos.y];
      }
    } else {
      nearestCube.z += inc.z;
      current.z += iinc.z;
      if ((current.z%CHUNK_WIDTH) != current.z) {
        current.z %= CHUNK_WIDTH;
        chunkPos.z += iinc.z;
        if (chunkPos.z == worldWidth || chunkPos.z == -1) {
          discard;
          return -1;
        }
        chunkId = location[chunkPos.z*worldWidth*WORLD_HEIGHT_CHUNKS + chunkPos.x*WORLD_HEIGHT_CHUNKS + chunkPos.y];
      }
    }
  }

  if (chunkId != 0) {
    return blockData[(chunkId-1)*CHUNK_SIZE + current.y*CHUNK_WIDTH*CHUNK_WIDTH + current.z*CHUNK_WIDTH + current.x].id;
  } else {
    return 0;
  }
}

//TODO make this redundant
bool setupDrawBlock(int blockId, int blockRotation, ivec3 current,
  vec3 nearestCube, vec3 inc, ivec3 iinc, int lastChunkId) {
  ivec3 last = current;
  if (nearestCube.x-inc.x > nearestCube.y-inc.y) {
    if (nearestCube.x-inc.x > nearestCube.z-inc.z) {
      last.x = current.x-iinc.x;
    } else {
      last.z = current.z-iinc.z;
    }
  } else {
    if (nearestCube.y-inc.y > nearestCube.z-inc.z) {
      last.y = current.y-iinc.y;
    } else {
      last.z = current.z-iinc.z;
    }
  }
  int side;
  vec4 sideVector;
  float texX;
  float texY;
  vec4 texVector;
  int light = 15;
  if (lastChunkId != 0) {
    light = blockData[(lastChunkId-1)*CHUNK_SIZE +
      ((last.y%CHUNK_WIDTH)*CHUNK_WIDTH*CHUNK_WIDTH) +
      ((last.z%CHUNK_WIDTH)*CHUNK_WIDTH) +
      (last.x%CHUNK_WIDTH)].lightLevel;
  }

  if (current.x != last.x) {
    float plane = nearestCube.x - inc.x;
    float distanceY = (nearestCube.y - plane)/inc.y;
    float distanceZ = (nearestCube.z - plane)/inc.z;
    texY = 1-(iinc.y+1)/2 + distanceY*iinc.y;
    texX = (iinc.z+1)/2 - distanceZ*iinc.z;
    texVector = vec4(0,texY,texX,1);
    if (current.x > last.x) {
      //side = 1;
      sideVector = vec4(-1,0,0,0);
    } else {
      //side = 0;
      sideVector = vec4(1,0,0,0);
    }
  } else if (current.y != last.y) {
    float plane = nearestCube.y - inc.y;
    float distanceX = (nearestCube.x - plane)/inc.x;
    float distanceZ = (nearestCube.z - plane)/inc.z;
    texY = 1-(iinc.x+1)/2 + distanceX*iinc.x;
    texX = (iinc.z+1)/2 - distanceZ*iinc.z;
    texVector = vec4(texY,0,texX,1);
    if (current.y > last.y) {
      //side = 3;
      sideVector = vec4(0,-1,0,0);
    } else {
      //side = 2;
      sideVector = vec4(0,1,0,0);
    }
  } else { //if (current.z != last.z)
    float plane = nearestCube.z - inc.z;
    float distanceY = (nearestCube.y - plane)/inc.y;
    float distanceX = (nearestCube.x - plane)/inc.x;
    texY = 1-(iinc.y+1)/2 + distanceY*iinc.y;
    texX = (iinc.x+1)/2 - distanceX*iinc.x;
    texVector = vec4(texX,texY,0,1);
    if (current.z > last.z) {
      //side = 5;
      sideVector = vec4(0,0,-1,0);
    } else {
      //side = 4;
      sideVector = vec4(0,0,1,0);
    }
  }

  sideVector = sideVector*rotation[blockRotation];
  if (sideVector.x == 1)
    side = 0;
  else if (sideVector.x == -1)
    side = 1;
  else if (sideVector.y == 1)
    side = 2;
  else if (sideVector.y == -1)
    side = 3;
  else if (sideVector.z == 1)
    side = 4;
  else
    side = 5;

  bool isVoxel = bool(idData[blockId*6*3 + side*3]);
  if (isVoxel) {
    int voxId = idData[blockId*6*3 + side*3 + 1];
    int rotate = idData[blockId*6*3 + side*3 + 2];
    return setupTraceBlock(voxId, nearestCube, inc, iinc, current, last, rotation[blockRotation]*rotation[rotate]);
  } else {
    return drawTexture(blockId, side, texVector, blockRotation, /*light*/ 15); //TODO
  }
}

void trace(ivec3 current, vec3 nearestCube, vec3 point, vec3 dir, ivec3 chunkPos, vec3 inc, ivec3 iinc) {
  int worldWidth = renderDistance*2+1;
  int chunkId = location[chunkPos.z*worldWidth*WORLD_HEIGHT_CHUNKS + chunkPos.x*WORLD_HEIGHT_CHUNKS + chunkPos.y];
  //TODO if player is above the world
  int lastChunkId = chunkId; //used for light levels
  int blockId = 0;
  int blockRotation = 0;
  if (chunkId != 0) {
    blockId = blockData[(chunkId-1)*CHUNK_SIZE + current.y*CHUNK_WIDTH*CHUNK_WIDTH + current.z*CHUNK_WIDTH + current.x].id;
    blockRotation = blockData[(chunkId-1)*CHUNK_SIZE + current.y*CHUNK_WIDTH*CHUNK_WIDTH + current.z*CHUNK_WIDTH + current.x].rotation;
  }
  bool blockFound = false;
  if (blockId != 0) {
    blockFound = setupTraceFirstBlock(blockId, inc, iinc, point, blockRotation);
  }
  while (!blockFound) {
    blockId = 0;
    while (blockId == 0) {
      blockId = nextCube(nearestCube, current, chunkPos, chunkId, lastChunkId, inc, iinc, worldWidth);
    }
    blockRotation = blockData[(chunkId-1)*CHUNK_SIZE + current.y*CHUNK_WIDTH*CHUNK_WIDTH + current.z*CHUNK_WIDTH + current.x].rotation;
    blockFound = setupDrawBlock(blockId, blockRotation, current, nearestCube, inc, iinc, lastChunkId);
  }
}

void main(void) {
  vec3 dir;
  if (spherical) {
    dir = getRaySphere(2*M_PI, M_PI, stereoscopic3d);
  } else {
    dir = getRayFlat(fovx, fovy, stereoscopic3d);
  }
  vec3 point = vec3(cameraPos);
  ivec3 chunkPos = ivec3(renderDistance, chunkHeight, renderDistance);

  //3D
  if (stereoscopic3d) {
    if (spherical) {
      if (texcoord.y < 0.5) {
        point.x -= (eyeWidth/2)*dir.z;
        point.z += (eyeWidth/2)*dir.x;
      } else {
        point.x += (eyeWidth/2)*dir.z;
        point.z -= (eyeWidth/2)*dir.x;
      }
    } else {// !spherical
      if (texcoord.x < 0.5) {
        point.x -= (eyeWidth/2)*cos(cameraDir.y);
        point.z -= (eyeWidth/2)*sin(cameraDir.y);
      } else {
        point.x += (eyeWidth/2)*cos(cameraDir.y);
        point.z += (eyeWidth/2)*sin(cameraDir.y);
      }
    }
    //Readjust chunkPos for 3d offset
    ivec3 cameraFloor = ivec3(floor(cameraPos/CHUNK_WIDTH));
    ivec3 pointFloor = ivec3(floor(point/CHUNK_WIDTH));
    if (cameraFloor.x > pointFloor.x) {
      chunkPos.x--;
    } else if (cameraFloor.x < pointFloor.x) {
      chunkPos.x++;
    }
    if (cameraFloor.z > pointFloor.z) {
      chunkPos.z--;
    } else if (cameraFloor.z < pointFloor.z) {
      chunkPos.z++;
    }
  }

  ivec3 current = ivec3(floor(point))%CHUNK_WIDTH;

  //number of steps to the next cube
  vec3 nearestCube;
  {
    //temp variable
    ivec3 nextDir;
    if (dir.x > 0) {
        nextDir.x = int(ceil(point.x));
    } else {
        nextDir.x = int(floor(point.x));
    }
    if (dir.y > 0) {
        nextDir.y = int(ceil(point.y));
    } else {
        nextDir.y = int(floor(point.y));
    }
    if (dir.z > 0) {
        nextDir.z = int(ceil(point.z));
    } else {
        nextDir.z = int(floor(point.z));
    }

    nearestCube = abs((nextDir - point)/dir);
  }
  //number of steps across a cube
  vec3 inc = abs(1/dir);

  //Allows for incrementing/decrementing without calculating which one every time.
  ivec3 iinc;
  if (dir.x > 0) {
      iinc.x = 1;
  } else {
      iinc.x = -1;
  }
  if (dir.y > 0) {
      iinc.y = 1;
  } else {
      iinc.y = -1;
  }
  if (dir.z > 0) {
      iinc.z = 1;
  } else {
      iinc.z = -1;
  }

  trace(current, nearestCube, point, dir, chunkPos, inc, iinc);
}
