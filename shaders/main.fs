#version 330 core

uniform sampler2D colorTex;     /*  Base color texture */
uniform sampler2D normalTex;    /*  Normal texture for normal mapping */
uniform sampler2D bumpTex;      /*  Bump texture for bump mapping */

in vec2 uvCoord;

uniform bool lightOn;
uniform int numLights;
uniform vec4 ambient;
uniform vec4 diffuse;
uniform vec4 specular;
uniform int specularPower;

/*  These could be in view space or tangent space */
in vec3 lightDir[10];
in vec3 viewDir;
in vec3 normal;

uniform bool normalMappingOn;   /*  whether normal mapping is on */
uniform bool parallaxMappingOn; /*  whether parallax mapping is on */


out vec4 fragColor;

void main(void)
{
    if (!lightOn)
    {
        fragColor = vec4(texture(colorTex, uvCoord).rgb, 1.0);
        return;
    }

    vec4 total = vec4(0.0f);

    //@TODO Assignment4
    /*  If normal mapping is off, then use the interpolated normal from vertex shader.
        Otherwise, sample the normal from normal map.

        If parallax mapping is off, then uvCoord should be used for all the texture sampling.
        Otherwise, we should use the adjusted uvCoord.
        Adjusted uvCoord is set to be uvCoord + h * viewDir.xy,
        where h is height from bump map * 0.15 - 0.005.

        For lighting, Blinn-Phong model should be used.
    */
    
	vec3 p_Nrm;
	vec3 view = normalize(viewDir);
	vec2 p_UV = uvCoord;

	if(parallaxMappingOn)
	{
		float h = texture(bumpTex, p_UV).r * 0.15f - 0.005f;
		p_UV = (h * view.xy) + p_UV;
	}

	if(normalMappingOn)
	{
		p_Nrm = texture(normalTex, p_UV).rgb;
		p_Nrm = (p_Nrm * 2.0f) - vec3(1.0f, 1.0f, 1.0f);
	}
	else
	{
		p_Nrm = normal;
	}


	p_Nrm = normalize(p_Nrm);

	for(int i = 0; i < numLights; ++i)
	{
		vec3 norm_lightVF = normalize(lightDir[i]);

		vec4 diffcol = diffuse * max(dot(p_Nrm, norm_lightVF), 0.0f);
		float diffuse_i = max(0.0f, dot(p_Nrm,norm_lightVF));

		vec3 halfDir = normalize(norm_lightVF + view);
		float specular_i = max(dot(halfDir, p_Nrm), 0.0f);
		specular_i =  pow(specular_i, specularPower);

		vec4 c_specular = specular * specular_i;
		vec4 c_diffuse = diffuse * diffuse_i;
		total += c_diffuse + c_specular;
	}

	total += ambient;
    fragColor = total * texture(colorTex, p_UV);
}