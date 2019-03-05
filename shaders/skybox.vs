#version 330 core

uniform mat4 viewMat;

out vec3 texCoord;

void main(void)
{
    //@TODO Assignment5
    /*  Render background using a full-screen quad defined as the far plane in NDC frame.

        This quad texture coordinates are defined in world-frame as the 4 back corners 
        of a 2x2x2 box symmetric about the world origin.
    */
    /*  gl_Position... */
    
    float amt = 1.0f;
    if(gl_VertexID == 0)
    {
        gl_Position = vec4(-1.0f, -1.0f, 1.0f, 1.0f);
        texCoord= transpose(mat3(viewMat)) * vec3(-1.0f, -1.0f, -1.0f);
    }
    else if(gl_VertexID == 1)
    {
        gl_Position = vec4(1.0f, -1.0f, 1.0f, 1.0f);
        texCoord= transpose(mat3(viewMat)) * vec3(1.0f, -1.0f, -1.0f);
    }
    else if(gl_VertexID == 2)
    {
        gl_Position = vec4(-1.0f, 1.0f, 1.0f, 1.0f);
        texCoord= transpose(mat3(viewMat)) * vec3(-1.0f, 1.0f, -1.0f);
    }
    else if(gl_VertexID == 3)
    {
        gl_Position = vec4(1.0f, 1.0f, 1.0f, 1.0f);
        texCoord= transpose(mat3(viewMat)) * vec3(1.0f, 1.0f, -1.0f);
    }
}