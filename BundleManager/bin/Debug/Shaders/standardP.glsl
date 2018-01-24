#version 450 core
//#extension GL_ARB_explicit_attrib_location: enable
//#extension GL_ARB_explicit_uniform_location : enable

in vec2 vs_textureCoordinate;
uniform sampler2D textureObject;
out vec4 color;


void main(void)
{
	//color = texelFetch(textureObject, ivec2(vs_textureCoordinate.x, vs_textureCoordinate.y), 0);
	color = texture(textureObject, vs_textureCoordinate);
	//color = vec4(1.0, 1.0, 1.0, 1.0);
}