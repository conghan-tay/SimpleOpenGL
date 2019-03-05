#version 330 core

/*  These vertex attributes are in model space */
layout (location = 0) in vec3 pos;
layout (location = 1) in vec3 nrm;
layout (location = 2) in vec3 tan;
layout (location = 3) in vec3 bitan;
layout (location = 4) in vec2 uv;

uniform mat4 mvMat;     /*  model-view matrix for positions */
uniform mat4 nmvMat;    /*  model-view matrix for normals */
uniform mat4 projMat;   /*  projection matrix */

uniform bool lightOn;           /*  whether lighting should be applied */
uniform int  numLights;
uniform vec3 lightPosVF[10];    /*  light pos already in view frame */

uniform bool normalMappingOn;   /*  whether normal mapping should be applied */


out vec2 uvCoord;

/*  Output vectors:
    - If normalMapping is on then these vectors are in tangent space.
    - Otherwise they are in view space
*/
out vec3 lightDir[10];
out vec3 viewDir;
out vec3 normal;


void main(void) 
{
    vec4 posVF = mvMat * vec4(pos, 1.0);

    /*    For object transformation */
    gl_Position = projMat * posVF;

    /*  For object texturing */
    uvCoord = uv;


    if (lightOn)
    {
        //@TODO Assignment4
        /*  Compute view direction, light direction and normal in view space */
        /*  ... */

        vec3 pos_view = (mvMat * vec4(pos,1.0f)).xyz;
        viewDir = normalize(-pos_view);

        for(int i = 0; i < numLights; ++i)
        {
            lightDir[i] = normalize(lightPosVF[i] - pos_view);
        }

       normal = (nmvMat * vec4(nrm,0.0f)).xyz;
       normal = normalize(normal);

 //@TODO Assignment4
            /*  If normal mapping is used, bring view dir and light dir to tangent space. 
                To do this, the TBN matrix needs to be used.
                Normal doesn't need to be transformed since we'll obtain it directly
                from the normal texture in the fragment shader.
            */
            /*  ... */
       
        if (normalMappingOn)    
        {
            //@TODO Assignment4
            /*  If normal mapping is used, bring view dir and light dir to tangent space. 
                To do this, the TBN matrix needs to be used.
                Normal doesn't need to be transformed since we'll obtain it directly
                from the normal texture in the fragment shader.
            */

            
            vec3 t_tan = (nmvMat * vec4(tan, 0.0f)).xyz;
            t_tan = normalize(t_tan);
            vec3 t_bi = (nmvMat * vec4(bitan, 0.0f)).xyz;
            t_bi = normalize(t_bi);

            mat4 tbn = mat4(0.0f);
            tbn[0] = vec4(t_tan, 0.0f);
            tbn[1] = vec4(t_bi, 0.0f);
            tbn[2] = vec4(normal, 0.0f);
            tbn[3] = vec4(0.0f,0.0f,0.f,1.0f);

            mat4 i_tbn = transpose(tbn);
            viewDir = (i_tbn * vec4(viewDir,0.0f)).xyz;

            for(int i = 0; i < numLights; i++)
				lightDir[i] = (i_tbn * vec4(lightDir[i],0.0f)).xyz;
                
        }
    }
}