#version 400 core

in vec3 aPosition;
in vec3 aNormal;
in vec2 aTexCoord;

out vec3 Normal;
out vec3 Position;
out vec2 TexCoord;

uniform mat4 matModel;
uniform mat4 matView;
uniform mat4 matProj;

void main(void)
{
    vec4 posWorld = matModel * vec4(aPosition, 1.0);
    gl_Position = matProj * matView * posWorld;

    Normal = (matModel * vec4(aNormal,1.0)).xyz;
    Position = posWorld.xyz;
    TexCoord = aTexCoord;
}