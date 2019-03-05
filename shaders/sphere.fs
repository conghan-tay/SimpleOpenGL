#version 330 core

uniform samplerCube texCube;

uniform mat4 viewMat;

uniform int sphereRef;

in vec3 view, normal;


out vec4 fragColor;


void main(void)
{
    //@TODO Assignment5
    /*  Compute reflected/refracted vectors in view frame for higher accuracy, 
        then transform them back to world frame for texture sampling.

        If sphereRef = 0 then there's only reflection.
        If sphereRef = 1 then there's only refraction.
        If sphereRef = 2 then there are both reflection and refraction.
        In that case, assume that reflection accounts for 70% of the color.
		
		The refractive index for the sphere is 1.5. For simplicity, we also
		assume that each ray going into the sphere is refracted only once.
    */

    vec4 reflect_c = vec4(0.0f);
    vec4 refract_c = vec4(0.0f);
    vec4 total= vec4(0.0f);
    
    if(sphereRef == 0)
    {
        vec3 reflected = reflect(normalize(view),normalize(normal));
        reflected = mat3(inverse(viewMat)) * reflected;
        reflect_c = texture(texCube, reflected);
        total = reflect_c;
    }
    else if(sphereRef == 1)
    {
        vec3 refracted = refract(normalize(view),normalize(normal), 1.0f/1.5f);
        refracted = mat3(inverse(viewMat)) * refracted;
        refract_c = texture(texCube, refracted);
        total = refract_c;
    }
    else if(sphereRef == 2)
    {
        vec3 reflected = reflect(normalize(view),normalize(normal));
        reflected = mat3(inverse(viewMat)) * reflected;
        reflect_c = texture(texCube, reflected);

        vec3 refracted = refract(normalize(view),normalize(normal), 1.0f/1.5f);
        refracted = mat3(inverse(viewMat)) * refracted;
        refract_c = texture(texCube, refracted);

         total = 0.7f * reflect_c + 0.3f * refract_c;
    }
    fragColor = total;
    /*  ... */
}