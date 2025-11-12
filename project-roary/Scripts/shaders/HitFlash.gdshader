shader_type canvas_item;

uniform bool flash = false;

void fragment() {
	vec4 texture_color = texture(TEXTURE, UV);
	
	if (flash && texture_color.a > 0.0){
		COLOR = vec4(0.961, 0.961, 0.961, texture_color.a);
	}
	else{
		COLOR = texture_color;
	}
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
