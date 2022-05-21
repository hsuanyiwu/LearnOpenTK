#version 400 core

in vec3 Normal;
in vec3 Position;

out vec4 Color;

uniform vec3 lightPos;
uniform vec3 lightColor;

void main(void)
{
    // ambient
    float ambient = 0.4;
    vec3 ambientColor = lightColor * ambient;

    // diffuse
    vec3 norm = normalize(Normal);
    vec3 tolight = normalize(lightPos - Position);

    float diffuse = dot(norm, tolight);
    diffuse = max(diffuse, 0.0);
    vec3 diffuseColor = lightColor * diffuse;

    // result
    vec3 result = ambientColor + diffuseColor;
    Color = vec4(result, 1);
}

