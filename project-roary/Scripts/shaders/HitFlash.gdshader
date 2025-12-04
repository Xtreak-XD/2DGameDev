shader_type canvas_item;

uniform bool flash = false;
uniform vec4 flash_color : source_color = vec4(1.0, 1.0, 1.0, 1.0);

void fragment() {
	vec4 texture_color = texture(TEXTURE, UV);

	if (flash && texture_color.a > 0.0){
		COLOR = vec4(flash_color.rgb, texture_color.a);
	}
	else{
		COLOR = texture_color;
	}
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
